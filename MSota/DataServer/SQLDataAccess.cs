using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using MSota.JavaScriptObjectNotation;

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

        public void SaveJsonDataOverStoredProcedure(JsonBodyProps props)
        {
            using (var oAdap = new SqlDataAdapter())
            using (var oCon = new SqlConnection(_connectionString))
            using (var oCmd = new SqlCommand("Ms_DuplicateChecker_Json", oCon))
            {
                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.AddWithValue("@DocEntry", props.smsProps.DocEntry);
                oCmd.Parameters.AddWithValue("@TranId", props.smsProps.TranId);
                oCmd.Parameters.AddWithValue("@LongDate", props.LongDate);
                oCmd.Parameters.AddWithValue("@DocDateTime", props.DocDateTime);
                oCmd.Parameters.AddWithValue("@Recepient", props.smsProps.Recepient?.Replace("'", "''"));
                oCmd.Parameters.AddWithValue("@AccNo", props.smsProps.AccNo);
                oCmd.Parameters.AddWithValue("@TranAmount", props.smsProps.TranAmount);
                oCmd.Parameters.AddWithValue("@Balance", props.smsProps.Balance);
                oCmd.Parameters.AddWithValue("@Charges", props.smsProps.Charges);
                oCmd.Parameters.AddWithValue("@DocType", props.DocType);
                oCmd.Parameters.AddWithValue("@Service_center", props.Service_center);
                oCmd.Parameters.AddWithValue("@IsRead", props.IsRead);
                oCmd.Parameters.AddWithValue("@Quota", props.smsProps.Quota.ToString());
                oCmd.Parameters.AddWithValue("@TranType", (char)props.smsProps.TranType);
                oCmd.Parameters.AddWithValue("@Body", props.Body?.Replace("'", "''"));

                oAdap.InsertCommand = oCmd;

                oCon.Open();
                oCmd.ExecuteNonQuery();
            }
        }
    }
    }
