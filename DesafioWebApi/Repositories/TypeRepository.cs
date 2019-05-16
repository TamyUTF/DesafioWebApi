using Dapper;
using DesafioWebApi.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DesafioWebApi.Repositories
{
    public class TypeRepository : AbstractRepository<TypePlan>
    {
        public TypeRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Create(TypePlan type)
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO PLAN_TYPES(NAME) VALUES (@Name)";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, type);
                db.Close();
                return result > 0;
            }
        }

        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM PLAN_TYPES WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, new { Id = id });
                db.Close();
                return result > 0;
            }
        }

        public override TypePlan FindById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM PLAN_TYPES WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var type = db.QueryFirst<TypePlan>(sQuery, new { Id = id });
                db.Close();
                return type;
            }
        }

        public override IEnumerable<TypePlan> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<TypePlan> types = db.Query<TypePlan>("SELECT * FROM PLAN_TYPES");
                return (List<TypePlan>)types;
            }
        }

        public override TypePlan GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection((ConnectionString)))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var type = db.QueryFirst<TypePlan>("SELECT * FROM PLAN_TYPES WHERE ID = IDENT_CURRENT('PLAN_TYPES')");
                return type;
            }
        }

        public override bool Update(TypePlan type)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sQuery = "UPDATE PLAN_TYPES SET NAME = @Name WHERE ID = @Id";
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute(sQuery, type);
                db.Close();
                return result > 0;
            }
        }
    }
}
