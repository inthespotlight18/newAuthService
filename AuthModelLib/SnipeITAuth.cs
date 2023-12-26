using System.Data;
using System.Net.Http.Headers;



namespace AuthModelLib
{
    public class SnipeITAuth : iGAuth
    {
        static HttpClient client = new HttpClient();

        //eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiMjQ4NTJmNDc5ODE3NDNiNjBiNWNkMGFkN2FlMWQxNjE5MDU0NjA4YTdjYmRiMGEyZWMwNjcxZGMzYzc2MzA3MmUwMWI1Nzc4ODA5ZDc3YjQiLCJpYXQiOjE3MDA3NjYyNjUuOTI4OTA2LCJuYmYiOjE3MDA3NjYyNjUuOTI4OTEyLCJleHAiOjIxNzQxNTE4NjUuODg2Njc0LCJzdWIiOiIxNyIsInNjb3BlcyI6W119.gDYaaQIZtHoXor8whCRKOzs0C-GfzzZ0cexXa_BPC3jzGcURrf_3zkWMMr1gAg-ftDImUTUqts8ZFmo5nx9lyqnxGZafn7Yg85U9nqFKfxK2XxMS1TvoOripD3OY4UGBH6Dnk3DBFDeM1PiAjs46NduXDmG7O2H_5lEJtys29oY-OMMH_ywmrjg7mdcmWVBE366Q0Emjs8BFws5_5Gm5RPGML68HUBkpg91iTkfvj8KDBIzpbb25bxJrTgm8EBLkwNEYAVkHyu3ueAEwrlnJ4nzO1OXzpVltU0wCkqkibveqWwU4ipL33f1wxUI5HaifGGhXhEDVb3ETK3xe4Lmwp1BSXnLVkl-MwjLO66UY5Z_WNqu1waeyTsoEuWOEzjgcIlntkMvkqI_1g1Iqj0lpI81frj9hU4gkAh9Rqn67-izaO471qQn_kVx_t7QbKX5BmRJ2QDfoyla4mDHS78wEJr_diYmDHKSg5H_j6plB_iwSiAs3NwJSYzOxYLQ3JPHL9hi6nBI2XlgZl37dNuyNbVa4oJOuwzDltocDFAu0-MHOuDsbbC7hqcrZjAXA7eqNmFkDJAoOBX3pQTLc1jbTUzKuJkM3VNMyQRpIOksqEVVWsPBqXUHEX2TaHIJM0kVFYb90UNcKXBqVgqKbTPGDhJetah0KM5bJOaod1X6HNME

        static string BearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiMjQ4NTJmNDc5ODE3NDNiNjBiNWNkMGFkN2FlMWQxNjE5MDU0NjA4YTdjYmRiMGEyZWMwNjcxZGMzYzc2MzA3MmUwMWI1Nzc4ODA5ZDc3YjQiLCJpYXQiOjE3MDA3NjYyNjUuOTI4OTA2LCJuYmYiOjE3MDA3NjYyNjUuOTI4OTEyLCJleHAiOjIxNzQxNTE4NjUuODg2Njc0LCJzdWIiOiIxNyIsInNjb3BlcyI6W119.gDYaaQIZtHoXor8whCRKOzs0C-GfzzZ0cexXa_BPC3jzGcURrf_3zkWMMr1gAg-ftDImUTUqts8ZFmo5nx9lyqnxGZafn7Yg85U9nqFKfxK2XxMS1TvoOripD3OY4UGBH6Dnk3DBFDeM1PiAjs46NduXDmG7O2H_5lEJtys29oY-OMMH_ywmrjg7mdcmWVBE366Q0Emjs8BFws5_5Gm5RPGML68HUBkpg91iTkfvj8KDBIzpbb25bxJrTgm8EBLkwNEYAVkHyu3ueAEwrlnJ4nzO1OXzpVltU0wCkqkibveqWwU4ipL33f1wxUI5HaifGGhXhEDVb3ETK3xe4Lmwp1BSXnLVkl-MwjLO66UY5Z_WNqu1waeyTsoEuWOEzjgcIlntkMvkqI_1g1Iqj0lpI81frj9hU4gkAh9Rqn67-izaO471qQn_kVx_t7QbKX5BmRJ2QDfoyla4mDHS78wEJr_diYmDHKSg5H_j6plB_iwSiAs3NwJSYzOxYLQ3JPHL9hi6nBI2XlgZl37dNuyNbVa4oJOuwzDltocDFAu0-MHOuDsbbC7hqcrZjAXA7eqNmFkDJAoOBX3pQTLc1jbTUzKuJkM3VNMyQRpIOksqEVVWsPBqXUHEX2TaHIJM0kVFYb90UNcKXBqVgqKbTPGDhJetah0KM5bJOaod1X6HNME";


        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> AuthLogin()
        {
            try
            {
                client.BaseAddress = new Uri("https://assets.garnet.ca");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", BearerToken);

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                return "OK";
            }
            catch (Exception ex)
            {
                string exceptionMessage = "SnipeITAuth->AuthLogin(): [" + ex.Message + "]";
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
                using HttpResponseMessage response = await client.GetAsync("api/v1/users");

                var r = response.EnsureSuccessStatusCode();

                Console.WriteLine("SnipeITAuth->GetProfile() " + r.ToString());

                
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{jsonResponse}\n");
                var dt = genericHelper.Tabulate(jsonResponse);

                return dt;
            }
            catch(Exception ex)
            {
                string exceptionMessage = "SnipeITAuth->GetProfile(): [" + ex.Message + "]";
                Console.WriteLine(exceptionMessage);
                return new DataTable(string.Format("FAIL|{0}", ex.Message));
            }
            
        }
       

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> SendEmail(string adresantEmail, string adresantName, string subject, string body)
        {
            return "This functionally is not implemented yet";
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> SendSMS(string receiverNumber, string message)
        {
            return "This functionally is not implemented yet";
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public string ServiceTest()
        {
            return "SnipeITAuth-> ServiceTest";
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/
    }
}
