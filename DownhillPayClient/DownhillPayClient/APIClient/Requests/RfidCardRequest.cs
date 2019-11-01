using DownhillPayClient.APIClient.Models;
using DownhillPayClient.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Requests
{
    public class RfidCardRequest : Request
    {
        public string EndpointUid { get; set; }
        public RfidCardRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.RfidCard;
            EndpointUriId = APIEndpoints.Default.RfidCardId;
            EndpointUid = APIEndpoints.Default.RfidCardUid;
        }

        public List<RfidCard> Get()
        {
            var request = new RestRequest(EndpointUri);
            var response = this.Get<List<RfidCard>>(request);
            return response.Data;
        }

        public RfidCard Get(int id)
        {
            var request = new RestRequest(EndpointUri + EndpointUriId + id);
            var response = this.Get<List<RfidCard>>(request);
            var client = response.Data;
            if (client.Count == 1) return client[0];
            else return null;
        }

        public void Post()
        {

        }

    }
}
