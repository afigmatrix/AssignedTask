namespace KaTechInterviewTask.Models.ResponseModels
{
    public class Trademark
    {
        public string Number { get; set; }
        public List<string> Words { get; set; }
        public Images Images { get; set; }
        public string StatusGroup { get; set; }
        public string StatusCode { get; set; }
        public List<GoodsAndServices> GoodsAndServices { get; set; }
    }
}
