using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MSota.DataLibrary
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly string _connectionString;
        private IServiceProvider _serviceProvider;


        public SQLDataAccess(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connectionString = _serviceProvider.GetRequiredService<DbContext>().GetService<IConfiguration>().GetConnectionString("SQLConnectionString");
        }

        public List<T> LoadData<T>(string sql)
        {
            using (IDbConnection IDbCn = new SqlConnection(_connectionString))
            {
                return IDbCn.Query<T>(sql).ToList();
            }
        }

        //public static List<T> LoadData<T>(string strvQuery)
        //{
        //    using (var oAdap = new SqlDataAdapter())
        //    using (var oCon = new SqlConnection(GetConnectionString()))
        //    using (var oCmd = new SqlCommand(strvQuery, oCon))
        //    {
        //        oCmd.Connection = oCon;

        //        oAdap.SelectCommand = oCmd;

        //        //oCon.Open();

        //        //oCmd.ExecuteReader();

        //        return oCon.Query<T>(strvQuery).ToList();
        //    }
        //}

        //public static int SaveData<T>(string sql, T data)
        //{
        //    using (IDbConnection IDbCn = new SqlConnection(GetConnectionString()))
        //    {
        //        return IDbCn.Execute(sql, data);
        //    }
        //}

        public void SaveData(string strvQuery)
        {
            try
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
            catch (Exception ex)
            {
            }
        }
    }
}
