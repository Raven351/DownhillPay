using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownhillPayClient.APIClient.Models;
using DownhillPayClient.Properties;
using RestSharp;

namespace DownhillPayClient.APIClient.Requests
{
    public class PointsRequest : Request
    {
        public PointsRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.Points;
        }
        /// <summary>
        /// Returns list of Points objects where PosNumber is or is not null.
        /// </summary>
        /// <param name="notNull">If true, returns records where PosNumber is not null</param>
        /// <returns>List of Points objects</returns>
        public List<Points> GetByPosNumber(bool notNull)
        {
            RestRequest request = new RestRequest();
            if (notNull == true) request.Resource = EndpointUri + "pos_number=not.is.null";
            else request.Resource = EndpointUri + "pos_number=is.null";
            var response = this.Get<List<Points>>(request);
            var points = response.Data;
            if (points.Count > 0) return points;
            else return null;
        }
    }
}
