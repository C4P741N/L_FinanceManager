using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DBConnection;
using Microsoft.Data.SqlClient;
using static DataLibrary.BusinessLogic.StatisticsProcessor;

namespace DataAndStatistics
{
    public class VariablesFormater
    {
        private DataAndStatisticsProp oDsp = null;
        //private DBRecordGet oDbget = null;
        //private SqlDataReader oReader = null;
        private DataMazematics math = null;
        private U_StatisticsProp uisprop = null;
        public void BeginFormatingVariables(ref U_StatisticsProp uisprop)
        {
            //oDbget = new DBRecordGet();
            oDsp = new DataAndStatisticsProp();
            math = new DataMazematics();
            uisprop = new U_StatisticsProp();

            xx_StartGettingStufToFormater(uisprop);
            
        }

        private void xx_StartGettingStufToFormater(U_StatisticsProp uisprop)
        {
                                                                                        double dCashSpent = 0;
                                                                                        double dCashReceived = 0;
                                                                                        double dvCashBalance = 0;
                                                                                        double dCashBalance = 0;
                                                                                        double dvFulizaAmount = 0;
                                                                                        double dFulizaAmount = 0;
                                                                                        double dvCharges = 0;
                                                                                        double dCharges = 0;
                                                                                        double dvFulizaAmountPaid = 0;
                                                                                        double dFulizaAmountPaid = 0;

            bool bSpent = false;

            List<DataAndStatisticsProp> oStuffs = xx_Stuf();

            foreach (DataAndStatisticsProp stuff in oStuffs)
            {
                math.BeginMazematics(stuff,
                                    ref dvCashBalance,
                                    ref dvFulizaAmount,
                                    ref dvCharges,
                                    ref dvFulizaAmountPaid);

                bSpent = stuff.TransactionStatus != "received";
              
                if (bSpent) 
                {
                    dCashSpent += dvCashBalance;
                }
                if (!bSpent)
                {
                    dCashReceived += dvCashBalance;
                }
                dFulizaAmount       +=  dvFulizaAmount;
                dCharges            += dvCharges;
                dFulizaAmountPaid   +=  dvFulizaAmountPaid;
            }
            uisprop.CashSpent           = math.RoundingOf(dCashSpent);
            uisprop.CashReceived        = math.RoundingOf(dCashReceived);
            uisprop.FulizaAmount        = math.RoundingOf(dFulizaAmount);
            uisprop.FulizaCharge        = math.RoundingOf(dCharges);
            uisprop.FulizaAmountPaid    = math.RoundingOf(dFulizaAmountPaid);

        }
        private List<DataAndStatisticsProp> xx_Stuf()
        {
            var oDataVal = new List<DataAndStatisticsProp>();

            var Stats = LoadStatistics();

            if (Stats != null)
            {
                //var prop = new DataAndStatisticsProp();

                foreach (var stat in Stats)
                {
                    var prop = new DataAndStatisticsProp();

                    prop.Code = stat.Code;
                    prop.Date = stat.Date;
                    prop.RecepientName = stat.RecepientName;
                    prop.RecepientPhoneNo = stat.RecepientPhoneNo;
                    prop.RecepientDate = stat.RecepientDate;
                    prop.RecepientAccNo = stat.RecepientAccNo;
                    prop.CashAmount = stat.CashAmount;
                    prop.Balance = stat.Balance;
                    prop.szProtocol = stat.szProtocol;
                    prop.TransactionStatus = stat.TransactionStatus;
                    prop.TransactionCost = stat.TransactionCost;
                    prop.Address = stat.Address;
                    prop.Type = stat.Type;
                    prop.Subject = stat.Subject;
                    prop.Body = stat.Body;
                    prop.Toa = stat.Toa;
                    prop.Sc_toa = stat.Sc_toa;
                    prop.Service_center = stat.Service_center;
                    prop.Read = stat.M_Read;
                    prop.Locked = stat.Locked;
                    prop.Date_sent = stat.Date_sent;
                    prop.Status = stat.Status;
                    prop.Sub_id = stat.Sub_id;
                    prop.Readable_date = stat.Readable_date;
                    prop.szContact_name = stat.szContact_name;
                    prop.Quota = stat.Quota;
                    prop.FulizaLimit = stat.FulizaLimit;
                    prop.FulizaBorrowed = stat.FulizaBorrowed;
                    prop.FulizaCharge = stat.FulizaCharge;
                    prop.FulizaAmount = stat.FulizaAmount;

                    oDataVal.Add(prop);
                }
                
            }

            return oDataVal;
        }
    }
}
