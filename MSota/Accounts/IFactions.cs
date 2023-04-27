using MSota.Responses;

namespace MSota.Accounts
{
    public interface IFactions
    {
        FactionsResponse GetFactionList(string factionID);
    }
}