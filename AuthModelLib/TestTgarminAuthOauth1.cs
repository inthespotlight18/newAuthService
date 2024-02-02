using System.Data;
using System.Security.Cryptography;
using System.Text;


namespace AuthModelLib
{
    public class garminAuth1 : IgOauth1
    {
        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/

        private const string ConsumerKey = "920a113f-b24b-4681-a3c2-fe16d940cb12";
        private const string ConsumerSecret = "VO4vvLXTmq4a6SCghWAMcwGYEwMY9dPvAp7";
        private const string SignatureMethod = "HMAC-SHA1";
        private const string oauth_version = "1.0";

        static string oauth_verifier = "";
        static string oauth_token = "";
        static string oauth_token_secret = "";
        static string oauth_access_token = "";


        private const string accessTokenUrl = "https://connectapi.garmin.com/oauth-service/oauth/access_token";

        //#1 step = https://connectapi.garmin.com/oauth-service/oauth/request_token  GET
        //#2 step = https://connectapi.garmin.com/oauthConfirm  has to be done by user
        //#3 step = https://connectapi.garmin.com/oauth-service/oauth/access_token

        /************************************************************************************************************************\
        *                                     Code below is the implementation of interface                                                                                 *
        \************************************************************************************************************************/

        public async Task<string> getRequestToken()
        {
            string requestUrl = "https://connectapi.garmin.com/oauth-service/oauth/request_token";

            var oauthParameters = new Dictionary<string, string>();

            var signatureBaseString = GenerateSignatureBaseString(requestUrl, "POST", oauthParameters);
            var oauthSignature = GenerateOAuthSignature(signatureBaseString);

            oauthParameters.Add("oauth_signature", Uri.EscapeDataString(oauthSignature));

            var authorizationHeader = $"OAuth {string.Join(", ", oauthParameters.Select(p => $"{p.Key}=\"{p.Value}\""))}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

                // Send the request to acquire the request token
                var response = await httpClient.PostAsync(requestUrl, null);


                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseContent}");

                    var tokenDictionary = ParseOAuthResponse(responseContent);

                    // Retrieve values from the dictionary
                    if (tokenDictionary.TryGetValue("oauth_token", out string oauthToken))
                        oauth_token = oauthToken; //request_token

                    if (tokenDictionary.TryGetValue("oauth_token_secret", out string oauthTokenSecret))
                        oauth_token_secret = oauthTokenSecret;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }

            return "OK";
        }

        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/

        public string setVerifier()
        {
            Console.WriteLine("Please go to the https://connect.garmin.com/oauthConfirm?oauth_token={{oauth_token}}, and get verifier from URL.");
            Console.WriteLine("Copy its value and enter it in the string below.");
            Console.Write("Verifier : ");
            string inputString = Console.ReadLine();
            oauth_verifier = inputString;
            Console.WriteLine("Confirmed Verifier : " + oauth_verifier);
            return "OK";
        }

        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> getAccessToken()
        {
            Console.WriteLine("testAccess::");

            using (var httpClient = new HttpClient())
            {
                // Generate the OAuth signature
                var oauthSignature = GenerateAccessTokenSignature("POST", accessTokenUrl, ConsumerKey, ConsumerSecret, oauth_token, oauth_token_secret, oauth_verifier);

                // Create the Authorization header for the Access Token request
                var authorizationHeader = $"OAuth oauth_consumer_key=\"{ConsumerKey}\", oauth_token=\"{oauth_token}\", oauth_signature_method=\"HMAC-SHA1\", oauth_nonce=\"{GenerateNonce()}\", oauth_timestamp=\"{GenerateTimestamp()}\", oauth_version=\"1.0\", oauth_verifier=\"{oauth_verifier}\", oauth_signature=\"{Uri.EscapeDataString(oauthSignature)}\"";

                // Make the OAuth request to obtain the User Access Token
                using (var request = new HttpRequestMessage(HttpMethod.Post, accessTokenUrl))
                {
                    request.Headers.Add("Authorization", authorizationHeader);

                    // Send the request to obtain the User Access Token
                    var response = await httpClient.SendAsync(request);

                    // Handle the response as needed
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response: {responseContent}");

                        // Parse the response content to extract User Access Token and Token Secret
                        var tokenDictionary = ParseOAuthResponse(responseContent);
                        if (tokenDictionary.TryGetValue("oauth_token", out string userAccessToken) &&
                            tokenDictionary.TryGetValue("oauth_token_secret", out string userTokenSecret))
                        {
                            Console.WriteLine($"User Access Token: {userAccessToken}");
                            Console.WriteLine($"User Token Secret: {userTokenSecret}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: *** access issues *** {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }

            return "Fail";
        }

        /************************************************************************************************************************\
        *                              Code below parses and generates authorization parameters needed for code above                                                                                       *
        \************************************************************************************************************************/

        public static string GenerateAccessTokenSignature(string httpMethod, string url, string consumerKey, string consumerSecret, string requestToken, string tokenSecret, string verifier)
        {
            var parameters = new Dictionary<string, string>
            {
                { "oauth_consumer_key", consumerKey },
                { "oauth_token", requestToken },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_nonce", GenerateNonce() },
                { "oauth_timestamp", GenerateTimestamp() },
                { "oauth_version", "1.0" },
                { "oauth_verifier", verifier }
            };

            // Sort the parameters alphabetically
            var sortedParameters = parameters.OrderBy(p => p.Key)
                                            .ToDictionary(p => p.Key, p => p.Value);

            // Concatenate parameters into normalized string
            var parameterString = string.Join("&", sortedParameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            // Create the Signature Base String
            var signatureBaseString = $"{httpMethod.ToUpper()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(parameterString)}";

            // Generate the key used for signing
            var key = $"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";

            using (var hmacsha1 = new System.Security.Cryptography.HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                var hashBytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(signatureBaseString));
                return Convert.ToBase64String(hashBytes);
            }

        }

        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/

        public static string GenerateSignatureBaseString(string url, string httpMethod, Dictionary<string, string> parameters,  string tokenSecret = "")
        {
            // Add OAuth parameters to the request parameters
            parameters.Add("oauth_consumer_key", ConsumerKey);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_nonce", GenerateNonce());
            parameters.Add("oauth_timestamp", GenerateTimestamp());
            parameters.Add("oauth_version", oauth_version);

            // Sort the parameters alphabetically
            var sortedParameters = parameters.OrderBy(p => p.Key)
                                            .ToDictionary(p => p.Key, p => p.Value);

            // Concatenate parameters into normalized string
            var parameterString = string.Join("&", sortedParameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            // Create the Signature Base String
            var signatureBaseString = $"{httpMethod.ToUpper()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(parameterString)}";

            return signatureBaseString;
        }

        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/


        public static string GenerateOAuthSignature(string signatureBaseString, string tokenSecret = "")
        {
            var key = $"{Uri.EscapeDataString(ConsumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";

            using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                var hashBytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(signatureBaseString));
                return Convert.ToBase64String(hashBytes);
            }
        }

        /************************************************************************************************************************\
        *                                                                                                                      *
        \************************************************************************************************************************/

        private static string GenerateNonce()
        {
            return Guid.NewGuid().ToString("N");
        }

        private static string GenerateTimestamp()
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64((DateTime.UtcNow - epochStart).TotalSeconds).ToString();
            return timestamp;
        }

       /************************************************************************************************************************\
       *                                                                                                                      *
       \************************************************************************************************************************/

        static Dictionary<string, string> ParseOAuthResponse(string responseString)
        {
            var tokenDictionary = new Dictionary<string, string>();

            // Split the response string into key-value pairs
            var keyValuePairs = responseString.Split('&');

            // Populate the dictionary with key-value pairs
            foreach (var pair in keyValuePairs)
            {
                var parts = pair.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0];
                    var value = parts[1];
                    tokenDictionary[key] = value;
                }
            }

            return tokenDictionary;
        }


        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************   

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

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/
    }
}


