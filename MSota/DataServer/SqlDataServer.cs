using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using MSota.ExtensibleMarkupAtLarge;
using System.Collections.Generic;
using System.Globalization;
using MSota.BaseFormaters;
using System.Text.RegularExpressions;
using MSota.Models;
using Calendar = MSota.Models.Calendar;
using MSota.JavaScriptObjectNotation;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MSota.DataServer
{
    public class SqlDataServer : ISqlDataServer
    {
        private ISQLDataAccess _dataAccess;
        IFortmaterAtLarge _fortmater;

        public SqlDataServer(ISQLDataAccess dataAccess, IFortmaterAtLarge fortmater)
        {
            _dataAccess = dataAccess;
            _fortmater = fortmater;
        }

        public void PostData(JsonBodyProps props)
        {
            AddStatisticsToDb(props);
        }
        private void AddStatisticsToDb(JsonBodyProps props)
        {

            string szSQL = $@"EXECUTE Ms_DuplicateChecker_Json
                            '{props.smsProps.DocEntry}',
                            '{props.smsProps.TranId}',
                            {props.LongDate},
                            '{props.DocDateTime}',
                            '{props.smsProps.Recepient}',
                            '{props.smsProps.AccNo}',
                            {props.smsProps.TranAmount},
                            {props.smsProps.Balance},
                            {props.smsProps.Charges},
                            '{props.DocType}',
                            '{props.Service_center}',
                            {props.IsRead},
                            '{props.smsProps.Quota}',
                            '{props.Body}'";

            _dataAccess.SaveData(szSQL);

            //_CopyAndSaveCollectionsToRecepientsAndTransactions();
        }

        public void PostData(List<IXmlProps> x_vprop)
        {
            AddStatisticsToDb(x_vprop);
        }
        private void AddStatisticsToDb(List<IXmlProps> x_vprop)
        {
            foreach (IXmlProps prop in x_vprop)
            {
                string szSQL = "EXECUTE Ms_DuplicateChecker " //This part gives me joy

                                + "'" + prop.szCode + "'," + Environment.NewLine
                                + "'" + prop.szDate + "'," + Environment.NewLine
                                + "'" + prop.szRName + "'," + Environment.NewLine
                                + "'" + prop.szRPhoneNo + "'," + Environment.NewLine
                                //+ "'" + prop.szRecepientDate + "'," + Environment.NewLine
                                + "'" + prop.szRAccNo + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dCashAmount) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dBalance) + "'," + Environment.NewLine
                                + "'" + prop.szProtocol + "'," + Environment.NewLine
                                + "'" + prop.szPayBill_TillNo + "'," + Environment.NewLine
                                + "'" + prop.szTransactionCost + "'," + Environment.NewLine
                                + "'" + prop.szAddress + "'," + Environment.NewLine
                                + "'" + prop.szType + "'," + Environment.NewLine
                                + "'" + prop.szSubject + "'," + Environment.NewLine
                                + "'" + prop.szBody + "'," + Environment.NewLine
                                + "'" + prop.szToa + "'," + Environment.NewLine
                                + "'" + prop.szSc_toa + "'," + Environment.NewLine
                                + "'" + prop.szService_center + "'," + Environment.NewLine
                                + "'" + prop.szRead + "'," + Environment.NewLine
                                + "'" + prop.szLocked + "'," + Environment.NewLine
                                + "'" + prop.szDate_sent + "'," + Environment.NewLine
                                + "'" + prop.szStatus + "'," + Environment.NewLine
                                + "'" + prop.szSub_id + "'," + Environment.NewLine
                                + "'" + prop.szReadable_date + "'," + Environment.NewLine
                                + "'" + prop.szQuota + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dFulizaLimit) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dFulizaBorrowed) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dCharges) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dFulizaAmount) + "',"
                                + "'" + prop.szUniqueKey + "'"

                                ;

                _dataAccess.SaveData(szSQL);
            }
            _CopyAndSaveCollectionsToRecepientsAndTransactions();
        }

        private void _CopyAndSaveCollectionsToRecepientsAndTransactions()
        {
            string[] storedProcedures = { "RecepientsCopier", "TransactionsCopier", "FactionsCopier" };

            foreach (var procedure in storedProcedures)
            {
                string szSQL = $"EXECUTE {procedure}";
                _dataAccess.SaveData(szSQL);
            }
        }

        public List<TransactionModel> LoadTransactionStatistics(Calendar cal)
        {
            string szSQL = "EXECUTE TransactionStatistics" + Environment.NewLine
                            + " '" + cal.from   + "'," + Environment.NewLine
                            + " '" + cal.to     + "'";

            return _dataAccess.LoadData<TransactionModel>(szSQL);
        } 

        public List<FactionsModel> LoadFactionsStatistics(Calendar cal)
        {
            string szSQL = "EXECUTE FactionsStatistics" + Environment.NewLine
                            + " '" + cal.from + "'," + Environment.NewLine
                            + " '" + cal.to + "'";

            return _dataAccess.LoadData<FactionsModel>(szSQL);
        }

        public List<FactionListModel> LoadFactionsList(string FactionID)
        {
            string szSQL = "EXECUTE FactionsList" + " '" + FactionID + "'";

            return _dataAccess.LoadData<FactionListModel>(szSQL);
        }

    }
}
