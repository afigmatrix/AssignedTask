namespace KaTechInterviewTask.Configs
{
    public class URLConfig
    {
        public string BaseURL { get; set; }
        public string AccessTokenURL { get; set; }
        public string AdvanceSearchResultPageURL { get; set; }
        public string DetailPageBaseURL { get; set; }

        public string GetAccessTokenURL()
        {
            return string.Concat(BaseURL, AccessTokenURL);
        }
        public string GetAdvanceSearchResultPageURL()
        {
            return string.Concat(BaseURL, AdvanceSearchResultPageURL);
        }
    }
}
