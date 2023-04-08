using Newtonsoft.Json;

namespace KaTechInterviewTask.Models.ResponseModels
{
    public class Images
    {
        [JsonProperty("Images")]
        public List<string> ImagesList { get; set; }
    }
}
