using DownhillPayClient.APIClient;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace APIClient.Models
{
    /// <summary>
    /// Client model for database table. Use this class to serialize to Json.
    /// </summary>
    public class Client : DatabaseModel
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName  { get; set; }
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        public Client()
        {

        }

        /// <summary>
        /// Initializes object with given properties.
        /// </summary>
        public Client(string firstName, string lastName, string phoneNumber, string birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
        }

        /// <summary>
        /// Initializes object with given properties.
        /// </summary>
        public Client(int id, string firstName, string lastName, string phoneNumber, string birthDate) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
        }
    }
}
