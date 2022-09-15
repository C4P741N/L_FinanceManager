using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAndStatistics
{
    public class DataMazematics
    {
        public void BeginMazematics(DataAndStatisticsProp vstuff,
                                    ref double dvCashBalance,
                                    ref double dvFulizaAmount,
                                    ref double dvCharges,
                                    ref double dvFulizaAmountPaid)
        {
            xx_StartDataMazematics(vstuff,
                                    ref dvCashBalance,
                                    ref dvFulizaAmount,
                                    ref dvCharges,
                                    ref dvFulizaAmountPaid);
        }
        private void xx_StartDataMazematics(DataAndStatisticsProp vstuff,
                                            ref double dvCashBalance,
                                            ref double dvFulizaAmount,
                                            ref double dvCharges,
                                            ref double dvFulizaAmountPaid)
        {
            switch (vstuff.TransactionStatus)
            {
                case "sent":
                case "paid":
                case "Paybill":
                case "airtime":
                case "Withdraw":
                case "received":
                    xx_AmountSpentAndReceived(vstuff,
                                    ref dvCashBalance);
                    break;
                case "borrowed":
                    xx_AmountBorrowed(vstuff,
                                      ref dvFulizaAmount,
                                      ref dvCharges,
                                      ref dvFulizaAmountPaid);
                    break;
            }
        }

        private DataAndStatisticsProp xx_AmountSpentAndReceived(DataAndStatisticsProp val,
                                                    ref double dvCashBalance)
        {
            string StrCashAmount = val.CashAmount;
            double dCashBalance = Convert.ToDouble(StrCashAmount);
            dvCashBalance = dCashBalance;

            return val;
        }

        private DataAndStatisticsProp xx_AmountBorrowed(DataAndStatisticsProp val,
                                                        ref double dvFulizaAmount,
                                                        ref double dvFulizaCharge,
                                                        ref double dvFulizaAmountPaid)
        {
                                                                                            string StrFulizaAmountPaid = string.Empty;
                                                                                            string StrFulizaAmount  = string.Empty;
                                                                                            string StrFulizaCharge = string.Empty;
                                                                                            double dFulizaAmountPaid = 0;
                                                                                            double dFulizaAmount = 0;
                                                                                            double dFulizaCharge = 0;

            if (val.FulizaCharge != "0")
            {
                StrFulizaAmount = val.FulizaAmount;
                StrFulizaCharge = val.FulizaCharge;

                dFulizaAmount = Convert.ToDouble(StrFulizaAmount);
                dFulizaCharge = Convert.ToDouble(StrFulizaCharge);

                dvFulizaAmount = dFulizaAmount;
                dvFulizaCharge = dFulizaCharge;
            }
            else
            {
                StrFulizaAmountPaid = val.FulizaAmount;

                dFulizaAmountPaid = Convert.ToDouble(StrFulizaAmountPaid);

                dvFulizaAmountPaid = dFulizaAmountPaid;
            }
            //Debug.WriteLine(dvFulizaAmount + " = " + dvFulizaCharge +" = "+ dvFulizaAmountPaid);

            return val;
        }

        public double RoundingOf(double val)
        {
            return Math.Round(val,2);
        }
    }
}
