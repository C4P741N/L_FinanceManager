using MSota.DataServer;
using MSota.Models;
using MSota.Responses;

namespace MSota.Accounts
{
    public class Factions : IFactions
    {
        List<FactionListModel> factionList = null;
        ISqlDataServer _sqlDataServer;

        public Factions(ISqlDataServer sqlDataServer)
        {
            _sqlDataServer = sqlDataServer;
        }
        public Responses.FactionsResponse GetFactionList(string factionID)
        {
            factionList = new List<FactionListModel>();

            try
            {
                factionList = _sqlDataServer.LoadFactionsList(factionID);

                return new FactionsResponse(factionList, new MSota.Responses.Error(), System.Net.HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return new FactionsResponse(
                    factionList, 
                    new MSota.Responses.Error
                    {
                        szErrorMessage = ex.Message,
                        szStackTrace = ex.StackTrace,
                        bErrorFound = true
                    }, 
                    System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
