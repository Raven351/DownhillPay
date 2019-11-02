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
    public class RfidCardSubscriptionRequest : Request
    {
        public RfidCardSubscriptionRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.RfidCardSubscription;
        }

        public bool Exists(string cardUid, DateTime startDate, DateTime endDate)
        {
            RfidCardRequest rfidCardRequest = new RfidCardRequest();
            var rfidCardRequestResponse = rfidCardRequest.Get(cardUid);
            if (rfidCardRequestResponse == null) throw new NullReferenceException("Invalid card!");
            var request = new RestRequest(
                EndpointUri + "or=(and(date_start.lte." + startDate + "," + "date_end.gte." + startDate + 
                "),and(date_start.lte." + endDate + "," + "date_end.gte." + endDate + "))&id=eq." + rfidCardRequestResponse.Id);
            var response = this.Get<List<RfidCardSubscription>>(request);
            if (response.Data.Count == 0) return false;
            else return true;

            //or=(and(date_start.lte.2019-01-05,date_end.gte.2019-01-05),and(date_start.lte.2019-03-05,date_end.gte.2019-03-05))&id=eq.9a69917a-7fdb-4092-bb5b-6ae6077216ac
        }
    }
}
