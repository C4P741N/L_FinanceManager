using Microsoft.AspNetCore.Http;
using MSota.Models;
using System.Net;

namespace MSota.Responses
{
    public class FactionsResponse : BaseResponse
    {
        public List<FactionListModel> _factionsListModel { get; set; }

        public FactionsResponse(List<FactionListModel> factionsListModel, Error error, HttpStatusCode statusCode) 
            : base(error, statusCode)
        {
            _factionsListModel = factionsListModel;
        }
    }
}
