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
    public class StatusRepository : AbstractRepository<Status>
    {
        public StatusRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Create(Status status)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO PLAN_STATUS(NAME) VALUES (@Name)";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, status);
                db.Close();
                return result > 0;
            }
        }

        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM PLAN_STATUS WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, new { Id = id });
                db.Close();
                return result > 0;
            }
        }

        public override Status FindById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM PLAN_STATUS WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var status = db.QueryFirst<Status>(sQuery, new { Id = id });
                db.Close();
                return status;
            }
        }

        public override IEnumerable<Status> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<Status> status = db.Query<Status>("SELECT * FROM PLAN_STATUS");
                return (List<Status>)status;
            }
        }

        public override Status GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection((ConnectionString)))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var status = db.QueryFirst<Status>("SELECT * FROM PLAN_STATUS WHERE ID = IDENT_CURRENT('PLAN_STATUS')");
                return status;
            }
        }

        public override bool Update(Status status)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE PLAN_STATUS SET NAME = @Name WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, status);
                db.Close();
                return result > 0;
            }
        }
    }
}
