using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MSota.DataServer
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly string _connectionString;
        private IServiceProvider _serviceProvider;


        public SQLDataAccess(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connectionString = _serviceProvider.GetRequiredService<DbContext>()
                                                .GetService<IConfiguration>()
                                                .GetConnectionString("SQLConnectionString")?? "wrong assignment of the connection string"; //this was a roller coster
        }

        public List<T> LoadData<T>(string sql)
        {
            using (IDbConnection IDbCn = new SqlConnection(_connectionString))
            {
                return IDbCn.Query<T>(sql).ToList();
            }
        }

        public void SaveData(string strvQuery)
        {
            using (var oAdap = new SqlDataAdapter())
            using (var oCon = new SqlConnection(_connectionString))
            using (var oCmd = new SqlCommand(strvQuery, oCon))
            {
                oCmd.Connection = oCon;

                oAdap.InsertCommand = oCmd;

                oCon.Open();
                oCmd.ExecuteNonQuery();
            }
        }
    }
}
