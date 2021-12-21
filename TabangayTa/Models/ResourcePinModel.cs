using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TabangayTa.Models
{
    public class Geolocation
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Result
    {
        public Geolocation geolocation { get; set; } = new Geolocation();
        public string locationName { get; set; }
        public string locationNotes { get; set; }

        [JsonProperty("Rel-Comments")]
        public List<string> RelComments { get; set; } = new List<string>();
        public string resourceImage { get; set; }
        public string resourceStatus { get; set; }
        public string ResourceType { get; set; }

        [JsonProperty("text-address")]
        public string TextAddress { get; set; }
        public int upvotes { get; set; }

        [JsonProperty("Created By")]
        public string CreatedBy { get; set; }

        [JsonProperty("Created Date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public string _id { get; set; }
        public string Slug { get; set; }
        [JsonProperty("CarrierList")]
        public List<string> CarrierList { get; set; } = new List<string>();
        public List<string> car_GasTypes { get; set; } = new List<string>();
    }

    public class Response
    {
        public int cursor { get; set; }
        public List<Result> results { get; set; } = new List<Result>();
        public int remaining { get; set; }
        public int count { get; set; }
    }

    public class ResourcePinModel
    {
        public Response response { get; set; } = new Response();
    }
}
