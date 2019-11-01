using Newtonsoft.Json;
using System;

namespace APIClient.Models
{

    public class DatabaseModel
    {
        /// <summary>
        /// Initializes new database object with random GUID as ID.
        /// </summary>
        public DatabaseModel()
        {
            this.Id = Guid.NewGuid();
        }

        public DatabaseModel(Guid id)
        {
            Id = id;
        }

        //[JsonIgnore]
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}