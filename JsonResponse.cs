using Newtonsoft.Json;

namespace Pract
{
    public class JsonResponse
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("fullAddressRu")]
        public string Adress { get; set; }
        [JsonProperty("placeRu")]
        public string Place { get; set; }
        [JsonProperty("cityRU")]
        public string City { get; set; }
    }
}
