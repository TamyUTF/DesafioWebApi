using Dapper;
using DesafioWebApi.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioWebApi.Repositories
{
    public class PlannerRepository : AbstractRepository<Planner>
    {
        public PlannerRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Create(Planner planner)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = @"INSERT INTO PLANS
                                (NAME, ID_user, ID_USER, ID_STATUS, START_DATE, END_DATE, DESCRIPTION, COST)
                                VALUES (@Name, @Iduser, @IdUser, @IdStatus, @StartDate, @EndDate, @Description, @Cost)";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, planner);
                db.Close();
                return (result > 0);
            }
        }

        public override bool Delete(int id)
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                string sQuery = "UPDATE PLANS SET REMOVED = 1 WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, new { Id = id });
                db.Close();
                return (result > 0);
            }

        }

        public override Planner FindById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var sQuery = db.Query<Planner, Status, TypePlan, User,Planner>(@"SELECT P.*, S.*,T.*, U.*
                                FROM PLANS P INNER JOIN PLAN_STATUS S ON P.ID_STATUS = S.ID
                                INNER JOIN PLAN_TYPES T ON P.ID_TYPE = T.ID
                                INNER JOIN USERS U ON P.ID_USER = U.ID
                                WHERE P.ID = @Id"
                , (P, S, T, U) =>
                {
                    P.Responsible = U;
                    P.Status = S;
                    P.Type = T;
                    return P;

                }, new { Id = id }, splitOn: "id,id,id,id").AsList();
                var queryInterested = db.Query<User>(@"SELECT U.*
                                                    FROM USERS U INNER JOIN PLAN_INTERESTED_USERS IU
                                                    ON U.ID = IU.ID_USER
                                                    WHERE ID_PLAN = @Id", new { Id = id });
                sQuery[0].Interested = (List<User>)queryInterested;
                return sQuery[0];
            }
        }

        public override IEnumerable<Planner> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
               if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var sQuery = db.Query<Planner, Status, TypePlan, User, Planner>(@"SELECT P.*, S.*,T.*, U.*
                                FROM PLANS P INNER JOIN PLAN_STATUS S ON P.ID_STATUS = S.ID
                                INNER JOIN PLAN_TYPES T ON P.ID_TYPE = T.ID
                                INNER JOIN USERS U ON P.ID_USER = U.ID"
                , (p,s,t,u) =>
                {
                    p.Responsible = u;
                    p.Type = t;
                    p.Status = s;
                    return p;
                   
                }, null, splitOn:"id,id,id,id").AsList();

                foreach (var item in sQuery)
                {
                    var queryInterested = db.Query<User>(@"SELECT U.*
                                                    FROM USERS U INNER JOIN PLAN_INTERESTED_USERS IU
                                                    ON U.ID = IU.ID_USER
                                                    WHERE ID_PLAN = @Id", new { Id = item.Id });
                    item.Interested = (List<User>)queryInterested;
                }
                return sQuery;
            }
        }



        public override Planner GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection((ConnectionString)))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var planner = db.QueryFirst<Planner>("SELECT * FROM PLAN_TYPES WHERE ID = IDENT_CURRENT('PLAN_TYPES')");
                return planner;
            }
        }

        public override bool Update(Planner planner)
        {

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                string sQuery = @"UPDATE PLANS SET NAME = @Name, ID_TYPE = @IdType, ID_USER = @IdUser, 
                                  ID_STATUS = @IdStatus, START_DATE = @StartDate, END_DATE = @EndDate, 
                                  DESCRIPTION = @Description, COST = @Cost
                                  WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, new {Name = planner.Name,
                                                    IdType = planner.Type.Id,
                                                    IdUser = planner.Responsible.Id,
                                                    IdStatus = planner.Status.Id,
                                                    StartDate = planner.StartDate,
                                                    EndDate = planner.EndDate,
                                                    Description = planner.Description,
                                                    Cost = planner.Cost,
                                                    Id = planner.Id});
                if (planner.Interested != null)
                {
                    result = UpdateInterested(planner);
                }
                db.Close();
                return result > 0;
            }
        }

        public int UpdateInterested(Planner planner)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var sQuery = @"IF EXISTS (SELECT * FROM PLAN_INTERESTED_USERS WHERE ID_PLAN = @Id)
                               BEGIN
                                    DELETE FROM PLAN_INTERESTED_USERS WHERE ID_PLAN = @Id
                               END";
                var result = db.Execute(sQuery, planner);
                if (planner.Interested != null)
                {
                    foreach (var interested in planner.Interested)
                    {
                        sQuery = @"INSERT INTO PLAN_INTERESTED_USERS (ID_PLAN, ID_USER)
                                   VALUES (@IdPlan, @IdUser)";
                        result = db.Execute(sQuery,new { IdPlan = planner.Id,
                                                         IdUser = interested.Id });
                    }
                }
                return result;
            }

        }
    }
}
