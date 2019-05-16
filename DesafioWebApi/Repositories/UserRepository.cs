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
    public class UserRepository : AbstractRepository<User>
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Create(User user)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = @"INSERT INTO USERS (NAME, LAST_CHANGED_DATE)
                                  VALUES(@Name, GETDATE())";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, user);
                db.Close();
                return result > 0;
            }
        }

        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE USERS SET REMOVED = 1 WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, new { Id = id });
                db.Close();
                return result > 0;
            }
        }

        public override User FindById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                string sQuery = "SELECT * FROM USERS WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var user = db.QueryFirst<User>(sQuery, new { Id = id });
                db.Close();
                return user;
            }
        }

        public override IEnumerable<User> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<User> users = db.Query<User>("SELECT * FROM USERS");
                return (List<User>)users;
            }
        }

        public override User GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection((ConnectionString)))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var user = db.QueryFirst<User>("SELECT * FROM USERS WHERE ID = IDENT_CURRENT('USERS')");
                return user;
            }
        }

        public override bool Update(User user)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = @"UPDATE USERS SET NAME = @Name,
                                                   REMOVED = @Removed,
                                                   CAN_CREATE_PLAN = @CanCreatePlan
                                  WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, user);
                db.Close();
                return result > 0;
            }
        }
    }
}
