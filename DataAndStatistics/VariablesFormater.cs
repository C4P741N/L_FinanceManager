using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicObjects;
//using DBConnection;
using Microsoft.Data.SqlClient;
using static DataLibrary.BusinessLogic.StatisticsProcessor;

namespace DataAndStatistics
{
    public class VariablesFormater
    {
        private DataAndStatisticsProp oDsp = null;
        private L_Recepients rpsx = null;
        private DataMazematics math = null;
        private U_StatisticsProp uisprop = null;
        private Z_Formaters.Formaters xFormaters = null;
        public L_Recepients BeginFormatingVariables()
        {
            //oDbget = new DBRecordGet();
            oDsp = new DataAndStatisticsProp();
            math = new DataMazematics();
            uisprop = new U_StatisticsProp();
            xFormaters = new Z_Formaters.Formaters();

            rpsx = xx_StartGettingStufToFormater();

            return rpsx;
        }

        private L_Recepients xx_StartGettingStufToFormater()
        {
                                                                                        //double dCashSpent = 0;
                                                                                        //double dCashReceived = 0;
                                                                                        double dvCashBalance = 0;
                                                                                        double dCashBalance = 0;
                                                                                        double dvFulizaAmount = 0;
                                                                                        //double dFulizaAmount = 0;
                                                                                        double dvCharges = 0;
                                                                                        //double dCharges = 0;
                                                                                        double dvFulizaAmountPaid = 0;
                                                                                        //double dFulizaBorrowed = 0;
                                                                                        bool bIsDeposit = false;
            
            bool bCountLimitReached = false;

            L_Recepients rps = xx_GetCollectionValues();

            foreach (L_Recepient rp in rps)
            {

                double dCashSpent = 0;
                double dCashReceived = 0;
                double dTransactionCharges = 0;
                double dFulizaAmount = 0;
                double dFulizaBorrowed = 0;
                int nCount = 0;

                foreach (L_Transaction tr in rp.transactions)
                {
                    //math.BeginMazematics(tr,
                    //                    ref dvCashBalance,
                    //                    ref dvFulizaAmount,
                    //                    ref dvCharges,
                    //                    ref dvFulizaAmountPaid);

                    //xx_GetListOfRecepientsAndData(stuff);

                    long TrDate = xFormaters.DateConvertionFromLongToTicksVal(tr.TranactionDate);

                    DateTime date1 = new DateTime(2022, 2, 1);
                    DateTime date2 = new DateTime(2022, 2, 28);
                    long test = date1.Ticks;
                    long test2 = date2.Ticks;

                    var Day = new DateTime(TrDate).Day;
                    var Month = ((uint)new DateTime(TrDate).Month);
                    var Year = new DateTime(TrDate).Year;
                    var DayOfWeek = new DateTime(TrDate).DayOfWeek;
                    var DayOfYear = new DateTime(TrDate).DayOfYear;
                    var TimeOfDay = new DateTime(TrDate).TimeOfDay;

                    if (date1.Ticks <= TrDate && date2.Ticks >= TrDate)
                    {

                    }

                    bIsDeposit = tr.TranactionQuota == "Deposit";

                    if (tr.FulizaDebtBalance != 0 || tr.FulizaCharge != 0 || tr.FulizaBorrowed != 0)
                    {

                    }

                    if (bIsDeposit == false)
                    {
                        dCashSpent += tr.TransactionAmount;
                    }
                    if (bIsDeposit == true)
                    {
                        dCashReceived += tr.TransactionAmount;
                    }
                    
                    dTransactionCharges += tr.TranactionCost;

                    if (rp.RecepientName.Contains("Fuliza"))
                    {
                        
                    }

                    dFulizaAmount += tr.FulizaDebtBalance;
                    dFulizaBorrowed += tr.FulizaBorrowed;
                    
                    nCount++;

                    bCountLimitReached = nCount == rp.transactions.Count();

                    if (bCountLimitReached)
                    {
                        tr.TotalTransactionWithdrawn    = math.RoundingOf(dCashSpent);
                        tr.TotalTransactionDeposited    = math.RoundingOf(dCashReceived);
                        tr.TotalFulizaBorrowed          = math.RoundingOf(dFulizaBorrowed);
                        tr.TotalFulizaDebtBalance       = math.RoundingOf(dFulizaAmount);
                        tr.TotalFulizaCharge            = math.RoundingOf(dTransactionCharges);

                        rp.transactions.AddTransaction(tr);
                    }
                }
                //uisprop.CashSpent   = math.RoundingOf(dCashSpent);
                //uisprop.CashReceived = math.RoundingOf(dCashReceived);
                //uisprop.FulizaAmount = math.RoundingOf(dFulizaAmount);
                //uisprop.FulizaCharge = math.RoundingOf(dCharges);
                //uisprop.FulizaBorrowed = math.RoundingOf(dFulizaBorrowed);
            }

            return rps;
        }

        private L_Recepients xx_GetCollectionValues()
        {
            var rpsList = new List<L_Recepients>();
            rpsx = new L_Recepients();

            var rpsStats = LoadRecepientsStatistics();
            var trsStats = LoadTransactionStatistics();

            if (rpsStats == null) return rpsx;

            foreach (var rpstat in rpsStats)
            {
                var rp = new L_Recepient();

                rp.RecepientId = rpstat.RecepientPhoneNo;
                rp.RecepientName = rpstat.RecepientName;
                rp.RecepientAccNo = rpstat.RecepientAccNo;

                foreach (var trstat in trsStats)
                {
                    if (trstat.RecepientPhoneNo == rp.RecepientId)
                    {
                        var tr = new L_Transaction();

                        tr.TransactionID = trstat.RecepientPhoneNo;
                        tr.TranactionQuota = trstat.Quota;
                        tr.TranactionCost = Convert.ToDouble(trstat.TransactionCost);
                        tr.TranactionDate = long.Parse(trstat.Date);
                        tr.TransactionAmount = Convert.ToDouble(trstat.CashAmount);
                        tr.FulizaBorrowed = Convert.ToDouble(trstat.FulizaBorrowed);
                        tr.FulizaCharge = Convert.ToDouble(trstat.FulizaCharge);
                        tr.FulizaDebtBalance = Convert.ToDouble(trstat.FulizaAmount);

                        rp.transactions.AddTransaction(tr);
                    }
                }

                rpsx.AddRecepient(rp);
            }

            return rpsx;
        }

        //private List<DataAndStatisticsProp> xx_Stuf()
        //{
        //    var oDataVal = new List<DataAndStatisticsProp>();

        //    var Stats = LoadStatistics();

        //    if (Stats != null)
        //    {
        //        //var prop = new DataAndStatisticsProp();

        //        foreach (var stat in Stats)
        //        {
        //            var prop = new DataAndStatisticsProp();

        //            prop.Code = stat.Code;
        //            prop.Date = stat.Date;
        //            prop.RecepientName = stat.RecepientName;
        //            prop.RecepientPhoneNo = stat.RecepientPhoneNo;
        //            prop.RecepientDate = stat.RecepientDate;
        //            prop.RecepientAccNo = stat.RecepientAccNo;
        //            prop.CashAmount = stat.CashAmount;
        //            prop.Balance = stat.Balance;
        //            prop.szProtocol = stat.szProtocol;
        //            prop.TransactionStatus = stat.TransactionStatus;
        //            prop.TransactionCost = stat.TransactionCost;
        //            prop.Address = stat.Address;
        //            prop.Type = stat.Type;
        //            prop.Subject = stat.Subject;
        //            prop.Body = stat.Body;
        //            prop.Toa = stat.Toa;
        //            prop.Sc_toa = stat.Sc_toa;
        //            prop.Service_center = stat.Service_center;
        //            prop.Read = stat.M_Read;
        //            prop.Locked = stat.Locked;
        //            prop.Date_sent = stat.Date_sent;
        //            prop.Status = stat.Status;
        //            prop.Sub_id = stat.Sub_id;
        //            prop.Readable_date = stat.Readable_date;
        //            prop.szContact_name = stat.szContact_name;
        //            prop.Quota = stat.Quota;
        //            prop.FulizaLimit = stat.FulizaLimit;
        //            prop.FulizaBorrowed = stat.FulizaBorrowed;
        //            prop.FulizaCharge = stat.FulizaCharge;
        //            prop.FulizaAmount = stat.FulizaAmount;

        //            oDataVal.Add(prop);
        //        }
                
        //    }

        //    return oDataVal;
        //}
    }
}
