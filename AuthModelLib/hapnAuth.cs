using System.Data;
using System.Text.Json;

namespace AuthModelLib
{
    public class hapnAuth : iGAuth
    {
        const string clientId = "a8l71iuejne2r1knh8l366jq5";
        const string clientSecret = "13657gcmh2j4imbl95mqrd52g71cns5ck76b7uct248gvdivd8ji";

        string tokenEndpoint = "https://auth.usehapn.com/oauth2/token";

        static string accessToken = "";

        static HttpClient _client = new HttpClient();

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> AuthLogin()
        {
            try
            {
                Console.WriteLine("HapnAuth->AuthLogin started: ");
                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret }
                };

                var content = new FormUrlEncodedContent(requestBody);
                var response = await _client.PostAsync(tokenEndpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = "HapnAuth->AuthLogin(): [Status code: " +  response.StatusCode + "Body: " + await response.Content.ReadAsStringAsync();
                    
                    Console.WriteLine(errorMessage);
                    return "FAIL|error";
                }

                var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                accessToken = payload.RootElement.GetProperty("access_token").GetString();

                _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                return "OK";
            
            }
            catch (Exception ex)
            {
                string exceptionMessage = "HapnAuth->AuthLogin(): [" + ex.Message + "]";
                Console.WriteLine(exceptionMessage);
                return "FAIL|error";
            }
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<DataTable> GetProfile()
        {
            try
            {
                Console.WriteLine("HapnAuth->GetProfile started: ");
                DataTable dt = new DataTable("OK");
                string apiUrl = "https://api.iotgps.io/v1/boundaries";
                var response = await _client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(
                        $"Status code: {response.StatusCode}, Body: {await response.Content.ReadAsStringAsync()}");
                }

                var s = response.Content.ReadAsStringAsync();
                Console.WriteLine(await s);

                return dt;

            }
            catch (Exception ex)
            {
                string exceptionMessage = "HapnAuth->GetProfile(): [" + ex.Message + "]";
                Console.WriteLine(exceptionMessage);
                return new DataTable(string.Format("FAIL|{0}", ex.Message));
            }
            
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/
        public async Task<string> SendEmail(string adresantEmail, string adresantName, string subject, string body)
        {
            return "This functionality is not implemented in HapnAuth class";
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> SendSMS(string receiverNumber, string message)
        {
            return "This functionality is not implemented in HapnAuth class";
        }

        /************************************************************************************************************************\
        *                                                                                                                      *
       \************************************************************************************************************************/
    }

    //public async Task<string> GetAccessToken()
    //{
    //    var requestBody = new Dictionary<string, string>
    //    {
    //        { "grant_type", "client_credentials" },
    //        { "client_id", clientId },
    //        { "client_secret", clientSecret }
    //    };

    //    var content = new FormUrlEncodedContent(requestBody);
    //    var response = await _client.PostAsync(tokenEndpoint, content);

    //    if (!response.IsSuccessStatusCode)
    //    {
    //        throw new ApplicationException($"Status code: {response.StatusCode}, Body: {await response.Content.ReadAsStringAsync()}");
    //    }

    //    var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    //    accessToken = payload.RootElement.GetProperty("access_token").GetString();
    //    return accessToken;
    //}

    //public async Task<string> MakeAuthenticatedRequest(string apiUrl, string accessToken)
    //{
    //    _client.DefaultRequestHeaders.Authorization =
    //        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

    //    var response = await _client.GetAsync(apiUrl);
    //    if (!response.IsSuccessStatusCode)
    //    {
    //        throw new ApplicationException(
    //            $"Status code: {response.StatusCode}, Body: {await response.Content.ReadAsStringAsync()}");
    //    }

    //    return await response.Content.ReadAsStringAsync();
    //}

}
