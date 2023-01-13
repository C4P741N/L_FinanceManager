using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LogicObjects;
//using DBConnection;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
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
        private L_Transactions tr = null;

        public VariablesFormater() 
        {
            //oDbget = new DBRecordGet();
            oDsp = new DataAndStatisticsProp();
            math = new DataMazematics();
            uisprop = new U_StatisticsProp();
            xFormaters = new Z_Formaters.Formaters();
        }

        public L_Transactions BeginGettingTransactions()
        {
            return xx_GetTransactionCollectionValues();
        }

        private L_Transactions xx_GetTransactionCollectionValues()
        {
            L_Transactions trs = new L_Transactions();

            foreach (var dXml in LoadTransactionStatistics())
            {
                var tr = new L_Transaction();

                if (dXml.FulizaCharge.IsNullOrEmpty() == false)
                    dXml.TransactionCost = dXml.FulizaCharge;

                trs.TotalAmountTransacted += math.RoundingOf(Convert.ToDouble(dXml.CashAmount));
                trs.TotalLoanBorrowed += math.RoundingOf(Convert.ToDouble(dXml.FulizaBorrowed));
                trs.TotalCharge += math.RoundingOf(Convert.ToDouble(dXml.TransactionCost));

                tr.TransactionID = dXml.Code_ID;
                tr.TranactionQuota = dXml.Quota;
                tr.TranactionDate = long.Parse(dXml.Date);

                tr.TransactionAmount = math.RoundingOf(Convert.ToDouble(dXml.CashAmount));
                tr.TranactionCharge = math.RoundingOf(Convert.ToDouble(dXml.TransactionCost));
                tr.LoanBorrowed = math.RoundingOf(Convert.ToDouble(dXml.FulizaBorrowed));

                trs.AddTransaction(tr);
            }

            return trs;
        }

        //private L_Transactions xx_GetTransactions()
        //{
        //    L_Transactions trs = new L_Transactions();

        //    foreach (L_Transaction tr in xx_GetTransactionCollectionValues())
        //    {
        //        trs.TotalAmountTransacted += tr.TransactionAmount;
        //        trs.TotalLoanBorrowed += tr.LoanBorrowed;
        //        trs.TotalCharge += tr.TranactionCharge;


        //        tr.TranactionCharge = math.RoundingOf(tr.TranactionCharge);
        //        tr.TransactionAmount = math.RoundingOf(tr.TransactionAmount);

        //        trs.AddTransaction(tr);
        //    }

        //    return trs;
        //}

        private Transactor xx_StartGettingStufToFormater()
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

            Transactor transactor = new Transactor();

            foreach (L_Recepient rp in rps)
            {

                double dCashSpent = 0;
                double dCashReceived = 0;
                double dTransactionCharges = 0;
                double dFulizaAmount = 0;
                double dFulizaBorrowed = 0;
                int nCount = 0;

                transactor.TransactionsCount += 1;

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

                    //if (tr.FulizaDebtBalance != 0 || tr.FulizaCharge != 0 || tr.FulizaBorrowed != 0)
                    //{

                    //}

                    if (bIsDeposit == false)
                    {
                        dCashSpent += tr.TransactionAmount;
                    }
                    if (bIsDeposit == true)
                    {
                        dCashReceived += tr.TransactionAmount;
                    }
                    
                    dTransactionCharges += tr.TranactionCharge;

                    

                    //dFulizaAmount += tr.FulizaDebtBalance;
                    dFulizaBorrowed += tr.LoanBalance;
                    
                    nCount++;

                    bCountLimitReached = nCount == rp.transactions.Count();

                    if (bCountLimitReached)
                    {
                        rp.TotalTransactionWithdrawn    = math.RoundingOf(dCashSpent);
                        rp.TotalTransactionDeposited    = math.RoundingOf(dCashReceived);

                        if (rp.RecepientName.Contains("Fuliza"))
                        {
                            transactor.TotalLoanBorrowed = math.RoundingOf(dFulizaBorrowed);

                            //tr.TotalFulizaBorrowed = math.RoundingOf(dFulizaBorrowed);
                            //tr.TotalFulizaDebtBalance = math.RoundingOf(dFulizaAmount);
                            //tr.TotalFulizaCharge = math.RoundingOf(dTransactionCharges);
                        }

                        transactor.TotAlamountTransacted += rp.TotalTransactionWithdrawn;
                        transactor.TotalAmountDeposited += rp.TotalTransactionDeposited;

                        rp.transactions.AddTransaction(tr);
                    }
                }
            }

            transactor.recepients = rps;

            return transactor;
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

                        if (trstat.FulizaCharge.IsNullOrEmpty() == false)
                            trstat.TransactionCost = trstat.FulizaCharge;

                        tr.TransactionID = trstat.Code_ID;
                        tr.TranactionQuota = trstat.Quota;
                        tr.TranactionDate = long.Parse(trstat.Date);

                        tr.TransactionAmount = Convert.ToDouble(trstat.CashAmount);
                        tr.TranactionCharge = Convert.ToDouble(trstat.TransactionCost);
                        tr.LoanBorrowed = Convert.ToDouble(trstat.FulizaBorrowed);

                        rp.transactions.AddTransaction(tr);

                    }
                }

                //if (rpsx.RecepientExists(rp.RecepientName) == false)
                //{
                //    rpsx.AddRecepient(rp);
                //}
                //else
                //{

                //    foreach (var trstat in trsStats)
                //    {
                //        if (trstat.RecepientPhoneNo == rp.RecepientId)
                //        {
                //            var tr = new L_Transaction();

                //            tr.TransactionID = trstat.RecepientPhoneNo;
                //            tr.TranactionQuota = trstat.Quota;
                //            tr.TranactionCost = Convert.ToDouble(trstat.TransactionCost);
                //            tr.TranactionDate = long.Parse(trstat.Date);
                //            tr.TransactionAmount = Convert.ToDouble(trstat.CashAmount);
                //            tr.FulizaBorrowed = Convert.ToDouble(trstat.FulizaBorrowed);
                //            tr.FulizaCharge = Convert.ToDouble(trstat.FulizaCharge);
                //            tr.FulizaDebtBalance = Convert.ToDouble(trstat.FulizaAmount);

                //            rp.transactions.AddTransaction(tr);
                //        }
                //    }
                //}

                //if(rpsx.RecepientExists(rp.RecepientName) == true)
                //{
                //    rpsx.UpdateRecepient()
                //}

                

                rpsx.AddRecepient(rp);

            }

            return rpsx;
        }

        //private L_Transaction 

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
