using DownhillPayClient.APIClient.Models;
using DownhillPayClient.APIClient.Serializers;
using DownhillPayClient.Properties;
using Newtonsoft.Json;
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
        public string EndpointUriUid { get; set; }
        public RfidCardRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.RfidCard;
            EndpointUriId = EndpointUri + APIEndpoints.Default.RfidCardId;
            EndpointUriUid = EndpointUri + APIEndpoints.Default.RfidCardUid;
        }

        public List<RfidCard> Get()
        {
            var request = new RestRequest(EndpointUri);
            var response = this.Get<List<RfidCard>>(request);
            return response.Data;
        }

        public RfidCard Get(string uid)
        {
            var request = new RestRequest(EndpointUriUid + uid);
            var response = this.Get<List<RfidCard>>(request);
            var client = response.Data;
            if (client.Count == 1) return client[0];
            else return null;
        }

        public string PatchPoints(string uid, int pointsRemaining, int PointToAdd)
        {
            var request = new RestRequest(EndpointUriUid + uid, Method.PATCH)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonNETSerializer();
            request.AddJsonBody(new Points((pointsRemaining + PointToAdd).ToString()));
            var response = this.Patch(request);
            var data = response.Content;
            return Convert.ToString(data);
        }

        private struct Points
        {
            public Points(string pointsBalance)
            {
                PointsBalance = pointsBalance;
            }

            [JsonProperty("points_balance")]
            public string PointsBalance { get; set; }
        }
    }
}
