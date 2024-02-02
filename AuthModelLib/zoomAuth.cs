using System.Data;
using System.Net.Http.Headers;
using System.Net.Http;

namespace AuthModelLib
{
    public class zoomAuth : iGAuth
    {

        const string clientId = "ABfOVow7RGC22k31c8JwbQ";
        const string secret = "iA5M3YPeoGZY3SrTGknsVM0OPbaj2lwR";
        const string accountId = "wwHIeG8RTie5rD7eFtF_sQ";
        //
        const string appName = "testApp";

        static string accessTokenHolder = "";

        /*
         * Secret Token
            Zoom sends the secret token in each event notification we send to your app.
            Note: This secret token is used to verify event notifications sent by Zoom.

        secretTokenEvent = "PB73G8O_RJ-t4FccsBGaJw";

        Verification Token (Retires in February 2024)
        Replace the Verification Token with Secret Token to verify event notifications from Zoom.

        verificationToken = "0DWOFmyrTzWJfUXdR3ra0w";



        ////////////////////
        ///

        ClientId = "XhUETn1CSHa6rRYB6gbHFQ";

        ClientSecret = "pfqOsYg2F2FiVd2OaT03A3D7Ie9fux3Z";

        Code="MuuldiBVhotcviErJ0PRTaJMTXKYKyJGQ";

        AuthorizationURL = "https://zoom.us/oauth/authorize?client_id=XhUETn1CSHa6rRYB6gbHFQ&response_type=code&redirect_uri=http%3A%2F%2Flocalhost%3A3000%2Fcallback";

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/


        public async Task<string> AuthLogin()
        {
            var apiKey = clientId;
            var apiSecret = secret;

            var accessToken = await GetAccessToken(apiKey, apiSecret);

            //string apiUrl = "https://api.zoom.us/v2/users/me"; // You can change the API endpoint as needed

            
            try
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine($"Access Token: {accessToken}");

                    accessTokenHolder = accessToken;

                    /* Use the access token to make API requests */

                    var apiUrl = "https://api.zoom.us/v2/users/me";
                    var userData = await GetZoomUserData(apiUrl, accessToken);

                    Console.WriteLine($"User Email: {userData.email}");

                    //*/
                }
                else
                {
                    Console.WriteLine("Failed to get access token");
                    return "FAIL|error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("zoomAuth->AuthLogin: " + ex.Message);
                return "FAIL|error";
            }

            

            return "OK";
         
        }

        //using (HttpClient client = new HttpClient())
        //{
        //    // Set the authorization header with the access token
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    try
        //    {
        //        // Make the API request
        //        HttpResponseMessage response = await client.GetAsync(apiUrl);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Parse and handle the response as needed
        //            string responseData = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine("API Response: " + responseData);
        //        }
        //        else
        //        {
        //            Console.WriteLine("Error: " + response.StatusCode);
        //            // Handle error response as needed
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        // Handle exceptions as needed
        //    }

        static async Task<string> GetAccessToken(string apiKey, string apiSecret)
        {
            using (var client = new HttpClient())
            {
                var tokenEndpoint = "https://zoom.us/oauth/token";
                var clientIdAndSecret = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientIdAndSecret);

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

                var response = await client.PostAsync(tokenEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var accessToken = System.Text.Json.JsonDocument.Parse(responseContent).RootElement.GetProperty("access_token").GetString();
                    return accessToken;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
        }

        static async Task<dynamic> GetZoomUserData(string apiUrl, string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetStringAsync(apiUrl);

                return System.Text.Json.JsonDocument.Parse(response).RootElement;
            }
        }

        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/

              

        public Task<DataTable> GetProfile()
        {
            throw new NotImplementedException();
        }

        public Task<string> SendSMS(string receiverNumber, string message)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendEmail(string adresantEmail, string adresantName, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public Task<string> AuthLogout()
        {
            throw new NotImplementedException();
        }

        public Task<string> Refresh()
        {
            throw new NotImplementedException();
        }
    }
}