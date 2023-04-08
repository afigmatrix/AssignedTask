namespace KaTechInterviewTask.Models
{
    public class GeneralReponseModel
    {
        public int ItemCount { get; set; }
        public List<AdvanceSearchItemResponseModel> Data { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public void Success(List<AdvanceSearchItemResponseModel> Data, int statusCode = 200)
        {
            this.Data = Data;
            this.StatusCode = statusCode;
            this.ErrorMessage = null;
            this.ItemCount = Data.Count;
        }

        public void Error(string exMessage, int statusCode = 400)
        {
            this.ErrorMessage = exMessage;
            this.StatusCode = statusCode;
            this.Data = null;
            this.ItemCount = 0;
        }
    }
}
