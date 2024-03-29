﻿using MSota.ExtensibleMarkupAtLarge;
using MSota.Models;

namespace MSota.DataServer
{
    public interface ISqlDataServer
    {
        AccountLedgerModel LoadAccountLegerSummary(Calendar_II cal);
        List<QuotaSummaryModel> LoadAccountQuotaSummary(Calendar_II cal);
        void PostData(List<IXmlProps> x_vprop);
        void PostData(JsonBodyModel vals);
        List<TransactionModel> LoadTransactionStatistics(Calendar cal);
        List<FactionsModel> LoadFactionsStatistics(Calendar cal);
        List<FactionListModel> LoadFactionsList(string FactionID);
    }
}