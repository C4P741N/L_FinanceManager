namespace MSota.EnumsAtLarge
{
    public class EnumContainer
    {
        public enum TransactionQuota
        {
            None                =   0,
            WithdrawnAmount     =   1, 
            MerchantPayment     =   2,
            CustomerTransfer    =   3,
            AccountDeposit      =   4,
            AirtimePurchase     =   5,
            LoanCredit          =   6,
            LoanDebit           =   7,
            PayBillPayment      =   8,
            InvalidTransaction  =   9,
        }

        public enum DoubleEntryAccounting
        {
            Credit = 0,//Outgoing
            Debit = 1,//Incoming
        }
    }
}
