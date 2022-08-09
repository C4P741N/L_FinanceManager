﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBConnection;
using Microsoft.Data.SqlClient;

namespace DataAndStatistics
{
    public class VariablesFormater
    {
        private DataAndStatisticsProp oDsp = null;
        private DBRecordGet oDbget = null;
        private SqlDataReader oReader = null;
        private DataMazematics math = null;
        private U_StatisticsProp uisprop = null;
        public void BeginFormatingVariables(ref U_StatisticsProp uisprop)
        {
            oDbget = new DBRecordGet();
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
                                                                                        double dvFulizaCharge = 0;
                                                                                        double dFulizaCharge = 0;
                                                                                        double dvFulizaAmountPaid = 0;
                                                                                        double dFulizaAmountPaid = 0;

            bool bSpent = false;

            List<DataAndStatisticsProp> oStuffs = xx_Stuf();

            foreach (DataAndStatisticsProp stuff in oStuffs)
            {
                math.BeginMazematics(stuff,
                                    ref dvCashBalance,
                                    ref dvFulizaAmount,
                                    ref dvFulizaCharge,
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
                dFulizaCharge       +=  dvFulizaCharge;
                dFulizaAmountPaid   +=  dvFulizaAmountPaid;
            }
            uisprop.CashReceived        = math.RoundingOf(dCashSpent);
            uisprop.CashSpent           = math.RoundingOf(dCashReceived);
            uisprop.FulizaAmount        = math.RoundingOf(dFulizaAmount);
            uisprop.FulizaCharge        = math.RoundingOf(dFulizaCharge);
            uisprop.FulizaAmountPaid    = math.RoundingOf(dFulizaAmountPaid);

        }
        private List<DataAndStatisticsProp> xx_Stuf()
        {
            List<DataAndStatisticsProp> oDataVal = new List<DataAndStatisticsProp>();

            oDbget.BeginGetDataAndStuff(ref oReader);

            if (oReader.HasRows)
            {
                while (oReader.Read())
                {
                    DataAndStatisticsProp prop = new DataAndStatisticsProp();

                    prop.Code               =   oReader["Code"].ToString();
                    prop.Date               =   oReader["M_Date"].ToString();
                    prop.RecepientName      =   oReader["M_RecepientName"].ToString();
                    prop.RecepientPhoneNo   =   oReader["M_RecepientPhoneNo"].ToString();
                    prop.RecepientDate      =   oReader["M_RecepientDate"].ToString();
                    prop.RecepientAccNo     =   oReader["M_RecepientAccNo"].ToString();
                    prop.CashAmount         =   oReader["M_CashAmount"].ToString();
                    prop.Balance            =   oReader["M_Balance"].ToString();
                    prop.szProtocol         =   oReader["M_szProtocol"].ToString();
                    prop.TransactionStatus  =   oReader["M_TransactionStatus"].ToString();
                    prop.TransactionCost    =   oReader["M_TransactionCost"].ToString();
                    prop.Address            =   oReader["M_Address"].ToString();
                    prop.Type               =   oReader["M_Type"].ToString();
                    prop.Subject            =   oReader["M_Subject"].ToString();
                    prop.Body               =   oReader["M_Body"].ToString();
                    prop.Toa                =   oReader["M_Toa"].ToString();
                    prop.Sc_toa             =   oReader["M_Sc_toa"].ToString();
                    prop.Service_center     =   oReader["M_Service_center"].ToString();
                    prop.Read               =   oReader["M_Read"].ToString();
                    prop.Locked             =   oReader["M_Locked"].ToString();
                    prop.Date_sent          =   oReader["M_Date_sent"].ToString();
                    prop.Status             =   oReader["M_Status"].ToString();
                    prop.Sub_id             =   oReader["M_Sub_id"].ToString();
                    prop.Readable_date      =   oReader["M_Readable_date"].ToString();
                    prop.szContact_name     =   oReader["M_szContact_name"].ToString();
                    prop.Quota              =   oReader["M_Quota"].ToString();
                    prop.FulizaLimit        =   oReader["M_FulizaLimit"].ToString();
                    prop.FulizaBorrowed     =   oReader["M_FulizaBorrowed"].ToString();
                    prop.FulizaCharge       =   oReader["M_FulizaCharge"].ToString();
                    prop.FulizaAmount       =   oReader["M_FulizaAmount"].ToString();

                    oDataVal.Add(prop);
                }
                
            }

            return oDataVal;
        }
    }
}
