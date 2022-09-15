using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataLibrary.DataAccess
{
    public static class SQLDataAccess
    {
        public static string GetConnectionString()
        {

                                                                                    string szConn = string.Empty;
                                                                                    IConfiguration IConf = null;

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConf = builder.Build();
            szConn = IConf.GetValue<string>("ConnectionStrings:value"); //Connection string from .json

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

        public static void SaveData (string strvQuery)
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
