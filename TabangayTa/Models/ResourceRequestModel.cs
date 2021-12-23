using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TabangayTa.Models.Request
{
    public class Geolocation
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class ResourceRequestModel
    {
        public List<string> CarrierList { get; set; }
        public Geolocation geolocation { get; set; }
        public string locationName { get; set; }
        public string locationNotes { get; set; }

        [JsonProperty("Rel-Comments")]
        public List<string> RelComments { get; set; }
        public string resourceImage { get; set; }
        public string resourceStatus { get; set; }
        public string ResourceType { get; set; }

        [JsonProperty("text-address")]
        public string TextAddress { get; set; }
        public List<string> car_GasTypes { get; set; }
        public int upvotes { get; set; }
    }


}
