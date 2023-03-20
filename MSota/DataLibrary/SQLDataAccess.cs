using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace MSota.DataLibrary
{
    public static class SQLDataAccess
    {
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            var IConf = builder.Build();
            var szConn = IConf.GetValue<string>("ConnectionStrings:value"); //Connection string from .json

            return szConn;
        }

        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection IDbCn = new SqlConnection(GetConnectionString()))
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

        public static void SaveData(string strvQuery)
        {
            using (var oAdap = new SqlDataAdapter())
            using (var oCon = new SqlConnection(GetConnectionString()))
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
