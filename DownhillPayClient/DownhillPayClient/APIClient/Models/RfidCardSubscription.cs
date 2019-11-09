using APIClient.Models;
using Newtonsoft.Json;
using System;


namespace DownhillPayClient.APIClient.Models

{
    public class RfidCardSubscription : DatabaseModel
    {
        /// <summary>
        /// Creates empty object
        /// </summary>
        public RfidCardSubscription()
        {

        }

        /// <summary>
        /// Initializes object with given properties.
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="idRfidCard"></param>
        public RfidCardSubscription(DateTime dateStart, DateTime dateEnd, Guid idRfidCard) : base()
        {
            DateStart = dateStart;
            DateEnd = dateEnd;
            IdRfidCard = idRfidCard;
        }

        #region Properties
        [JsonProperty("date_start")]
        public DateTime DateStart { get; set; }

        [JsonProperty("date_end")]
        public DateTime DateEnd { get; set; }

        [JsonProperty("id_rfid_card")]
        public Guid IdRfidCard { get; set; }
        #endregion
    }


}