using KaTechInterviewTask.Configs;
using KaTechInterviewTask.Models;
using KaTechInterviewTask.Models.ResponseModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace KaTechInterviewTask.Logics
{
    public class AdvanceSearchLogic
    {
        public URLConfig _URLconfig { get; }
        public CredentialConfig _credentialConfig { get; }
        public AdvanceSearchLogic(URLConfig URLconfig, CredentialConfig credentialConfig)
        {
            _URLconfig = URLconfig;
            _credentialConfig = credentialConfig;
        }

        public async Task<GeneralReponseModel> GetAdvanceSearchResult(string wordValue)
        {
            GeneralReponseModel generalReponseModel = new GeneralReponseModel();
            try
            {
                List<AdvanceSearchItemResponseModel> responsData = new List<AdvanceSearchItemResponseModel>();
                decimal maxItemValue = 100;
                using var client = new HttpClient();

                var response = await SendRequest(client, wordValue);
                var mappedResult = MapResponseModels(response);
                responsData.AddRange(mappedResult);

                decimal itemsCount = GetItemsCount(response);

                if (itemsCount > 100)
                {
                    int pageCount = (int)Math.Ceiling(itemsCount / maxItemValue);
                    for (int i = 1; i < pageCount; i++)
                    {
                        var perPageResponse = await SendRequest(client, wordValue, i);
                        var perPageMappedResult = MapResponseModels(perPageResponse);
                        responsData.AddRange(perPageMappedResult);
                    }
                }
                generalReponseModel.Success(responsData);
            }
            catch (Exception ex)
            {
                generalReponseModel.Error(ex.Message);
            }
            return generalReponseModel;
        }

        private async Task<string> GetAccessToken(HttpClient httpClient)
        {
            var url = _URLconfig.GetAccessTokenURL();
            var body = new Dictionary<string, string>
            {
                {"grant_type",_credentialConfig.GrantType },
                {"client_id",_credentialConfig.Id },
                {"client_secret",_credentialConfig.Secret }
            };
            var bodyContent = new FormUrlEncodedContent(body);

            var response = await httpClient.PostAsync(url, bodyContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJObjParsedValue = JObject.Parse(responseContent);
            var accessToken = (string)responseJObjParsedValue.GetValue("access_token");
            return accessToken;
        }

        private List<AdvanceSearchItemResponseModel> MapResponseModels(string response)
        {
            var trademarkData = JsonConvert.DeserializeObject<TrademarkData>(response);
            var itemResult = trademarkData.Trademarks?.Select(m => new AdvanceSearchItemResponseModel
            {
                Classes = m.GoodsAndServices?[0].Class,
                Details_Page_Url = string.Concat(_URLconfig.DetailPageBaseURL, m.Number),
                Number = m.Number,
                Logo_Url = m.Images?.ImagesList?[0],
                Name = string.Join(',',m.Words),
                Status1 = m.StatusGroup,
                Status2 = m.StatusCode
            }).ToList();
            return itemResult;
        }

        private decimal GetItemsCount(string responseString)
        {
            var responseJObjParsedValue = JObject.Parse(responseString);
            var count = (decimal)responseJObjParsedValue.GetValue("count");
            return count;
        }

        private StringContent GetRequestBody(string wordValue, int pageNumber, int pageSize)
        {
            var model = new
            {
                changedSinceDate = "",
                pageNumber = pageNumber,
                pageSize = pageSize,
                rows = new[] { new { op = "AND", query = new { addressForService = "", claimant = "", classNumber = new { text = "", type = "SINGLE" },
                date = new { from = "", to = "", type = "LODGEMENT_DATE" },
                flags = new object[] { }, goodsAndServices = "", image = new { text = "", type = "EXACT" },
                irNumber = "",
                kinds = new object[] { }, opponent = "", otherInformation = "", owner = "", removalApplicant = "",
                statuses = new object[] { },
                trademarkNumber = "",
                word = new { text = wordValue, type = "PART" }, wordPhrase = "" } } },
                sort = new { field = "NUMBER", direction = "ASCENDING" }
            };
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }

        private async Task<string> SendRequest(HttpClient client, string wordValue, int pageNumber = 0, int pageSize = 100)
        {
            string advanceSearchURL = _URLconfig.GetAdvanceSearchResultPageURL();
            if (string.IsNullOrEmpty(_credentialConfig.AccessToken))
                _credentialConfig.AccessToken = await GetAccessToken(client);

            StringContent bodyData = GetRequestBody(wordValue, pageNumber, pageSize);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _credentialConfig.AccessToken);
            var response = await client.PostAsync(advanceSearchURL, bodyData);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
