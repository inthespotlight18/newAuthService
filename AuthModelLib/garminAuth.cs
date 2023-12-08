//using System.Data;
//using System.Diagnostics;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Web;
//using Google.Apis.Auth.OAuth2;
//using RestSharp;
//using RestSharp.Authenticators;



//namespace AuthModelLib
//{
//    public class GarminOAuth1a
//    {
//        private const string ConsumerKey = "920a113f-b24b-4681-a3c2-fe16d940cb12";
//        private const string ConsumerSecret = "VO4vvLXTmq4a6SCghWAMcwGYEwMY9dPvAp7";

       

//        static HttpClient client = new HttpClient();

//        static RestClient _client;

//        //#1 step = https://connectapi.garmin.com/oauth-service/oauth/request_token  GET
//        //#2 step = https://connectapi.garmin.com/oauthConfirm  GET
//        //#3 step = https://connectapi.garmin.com/oauth-service/oauth/access_token




//        //oauth_token=c6057f88-13d3-47fd-a4ee-60d882513f24
//        //oauth_token_secret= BxbA0vDYHTvQvaYbWlaikc21WEW4m4wy5vr

//        // static RestClient? client;

//        //public (string token, string tokenSecret) GetUnauthorizedRequestToken(RestClientOptions client)
//        //{
//        //    Console.WriteLine("Acquiring Unauthorized Request Token and Token Secret...");

//        //    

//        //    var request = new RestRequest("oauth/request_token");
//        //    var response = client.Execute(request);

//        //    Debug.Assert(response != null);
//        //    Debug.Assert(HttpStatusCode.OK == response.StatusCode);

//        //    var qs = System.Web.HttpUtility.ParseQueryString(response.Content);
//        //    var requestToken = qs["oauth_token"];
//        //    var oauthTokenSecret = qs["oauth_token_secret"];

//        //    Debug.Assert(requestToken != null);
//        //    Debug.Assert(oauthTokenSecret != null);

//        //    return (requestToken, oauthTokenSecret);
//        //}

//        //public (string token, string tokenSecret) GetAccessToken(string requestToken, string requestTokenSecret, string verifier)
//        //{
//        //    Console.WriteLine("Acquiring User Access Token and Token Secret...");
//        //    var oauthBaseUri = new Uri("https://connectapi.garmin.com/oauth-service/");
//        //    var client = new RestClient(oauthBaseUri);
//        //    client.Authenticator = OAuth1Authenticator.ForAccessToken(
//        //        ConsumerKey,
//        //        ConsumerSecret,
//        //        requestToken,
//        //        requestTokenSecret,
//        //        verifier
//        //    );

//        //    var request = new RestRequest("oauth/access_token");
//        //    var response = client.Execute(request);

//        //    Console.WriteLine($"Code: {response.StatusCode}, response: {response.Content}");
//        //    Debug.Assert(response != null);
//        //    Debug.Assert(HttpStatusCode.OK == response.StatusCode);

//        //    var qs = HttpUtility.ParseQueryString(response.Content);
//        //    var accessToken = qs["oauth_token"];
//        //    var accessTokenSecret = qs["oauth_token_secret"];

//        //    Debug.Assert(accessToken != null);
//        //    Debug.Assert(accessTokenSecret != null);

//        //    return (accessToken, accessTokenSecret);
//        //}

//        //***********************************************
//        public (string token, string tokenSecret) AccessToken(string requestToken, string requestTokenSecret, string verifier)
//        {
//            Console.WriteLine("Acquiring User Access Token and Token Secret...");
//            var oauthBaseUri = new Uri("https://connectapi.garmin.com/oauth-service/");

//            try
//            {
//                client.BaseAddress = new Uri("https://connectapi.garmin.com/oauth-service/");

//                client.DefaultRequestHeaders.Authorization =
//                    new AuthenticationHeaderValue(ConsumerKey, "Consumer Key");

//                client.DefaultRequestHeaders.Authorization =
//                    new AuthenticationHeaderValue(ConsumerSecret, "Consumer Secret");

//                client.DefaultRequestHeaders.Authorization =
//                    new AuthenticationHeaderValue()


//                client.DefaultRequestHeaders.Accept.Add(
//                    new MediaTypeWithQualityHeaderValue("application/json"));

//                return "OK", "OK";
//            }
//            catch (Exception ex)
//            {
//                string exceptionMessage = "SnipeITAuth->AuthLogin(): [" + ex.Message + "]";
//                Console.WriteLine(exceptionMessage);
//                return "FAIL|error", "f";
//            }

//            //var client = new RestClient(oauthBaseUri);
//            //client.Authenticator = OAuth1Authenticator.ForAccessToken(
//            //    ConsumerKey,
//            //    ConsumerSecret,
//            //    requestToken,
//            //    requestTokenSecret,
//            //    verifier
//            //);

//            //var request = new RestRequest("oauth/access_token");
//            //var response = client.Execute(request);

//            //Console.WriteLine($"Code: {response.StatusCode}, response: {response.Content}");
//            //Debug.Assert(response != null);
//            //Debug.Assert(HttpStatusCode.OK == response.StatusCode);

//            //var qs = HttpUtility.ParseQueryString(response.Content);
//            //var accessToken = qs["oauth_token"];
//            //var accessTokenSecret = qs["oauth_token_secret"];

//            //Debug.Assert(accessToken != null);
//            //Debug.Assert(accessTokenSecret != null);

//            //return (accessToken, accessTokenSecret);
//        }

//        //public string GetUserId(string accessToken, string accessTokenSecret)
//        //{
//        //    Console.WriteLine("Getting User's Garmin Id...");

//        //    var healthApiBaseUri = new Uri("https://healthapi.garmin.com/");
//        //    var client = new RestClient(healthApiBaseUri);
//        //    client.Authenticator = OAuth1Authenticator.ForProtectedResource(
//        //       ConsumerKey,
//        //    ConsumerSecret,
//        //    accessToken,
//        //       accessTokenSecret
//        //   );

//        //    var request = new RestRequest("wellness-api/rest/user/id", DataFormat.Json);
//        //    var response = client.Execute(request);
//        //    Debug.Assert(response != null);
//        //    Debug.Assert(HttpStatusCode.OK == response.StatusCode);

//        //    Console.WriteLine($"Code: {response.StatusCode}, response: {response.Content}");

//        //    // TODO - Deserialize JSON response to a UserId model
//        //    return response.Content;
//        //}
//    }
//}