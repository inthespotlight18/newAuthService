using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace AuthModelLib 
{
    public class googleAuth : iGAuth
    {     
        const string clientId = "532374526064-70e174f85sid96ttojf20k06fjf3kp0s.apps.googleusercontent.com";
        const string secret = "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";

        //our app password for your device = rxmu purd pybx rvus
        private UserCredential _credentials { get; set; }

        private static string[] scopes = { "https://www.googleapis.com/auth/gmail.readonly", "https://www.googleapis.com/auth/youtube" };

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> AuthLogin()
        {
            Console.WriteLine("googleAuth->AuthLogin STARTED");
            try
            {
                var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = secret,
                },
                scopes, "testClientGS", CancellationToken.None).Result;
                //Console.WriteLine("googleAuth->AuthLoginTest : c[{0}]", credentials.Token.IssuedUtc.ToString());

                Console.WriteLine("googleAuth->AuthLoginTest : finished");

                _credentials = credentials;

                return "OK";
            }
            catch (Exception ex)
            {
                string exceptionMessage = "googleAuth->AuthLogin(): [" + ex.Message + "]";
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
                Console.WriteLine("googleAuth->GetProfile started");             
                Console.ReadLine();

                //==============================================================           
                if (_credentials.Token.IsExpired(SystemClock.Default))
                    _credentials.RefreshTokenAsync(CancellationToken.None).Wait();         
                
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credentials
                });

                var profile = service.Users.GetProfile("tugit@fity.ca").Execute();
            
                Console.WriteLine("Total gmails: " + profile.MessagesTotal);          
                Console.ReadLine();

                var jsonResponse = System.Text.Json.JsonSerializer.Serialize<Google.Apis.Gmail.v1.Data.Profile>(profile);

                var dtResult = Helper.Tabulate(jsonResponse);

                if (dtResult != null) return dtResult;

                return new DataTable(string.Format("FAIL|error"));

                //Console.WriteLine("+++++++++++++++++++++++++++");
                //Console.WriteLine(jsonResponse);         //{"EmailAddress":"tugit@fity.ca","HistoryId":2264,"MessagesTotal":8,"ThreadsTotal":8,"ETag":null}
                //Console.WriteLine("+++++++++++++++++++++++++++");
   
            }
            catch (Exception ex)
            {
                string exceptionMessage = "googleAuth->GetProfile(): [" + ex.Message + "]";
                Console.WriteLine(exceptionMessage);
                return new DataTable(string.Format("FAIL|{0}", ex.Message));
            }
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> SendSMS(string receiverNumber, string message)
        {
            return "This functionality is not implemented in GoogleAuth class";

        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public async Task<string> SendEmail(string adresantEmail, string subject, string body)
        {
            try
            {      
                using (var client = new SmtpClient())
                {
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("tugit@fity.ca", "rxmu purd pybx rvus");
                    using (var message = new MailMessage(
                        from: new MailAddress("tugit@fity.ca", "Gordon"),
                        to: new MailAddress(adresantEmail, "Daniil")
                        ))
                    {

                        message.Subject = subject;
                        message.Body = body;

                        client.Send(message);
                    }                  
                }
                return "OK";
            } 

            catch (Exception ex)
            {
                string exceptionMessage = "googleAuth->SendEMAIL(): [" + ex.Message + "]";
                Console.WriteLine(exceptionMessage);
                return "FAIL|" + ex.Message;
            }       

        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public DataTable ProfileToDataTable(Profile profile)
        {
            DataTable? dataTable = new DataTable("ProfileDT");
            dataTable.Columns.Add("ETag");
            dataTable.Columns.Add("EmailAddress");
            dataTable.Columns.Add("HistoryId");
            dataTable.Columns.Add("MessagesTotal");
            dataTable.Columns.Add("ThreadsTotal");
            try
            {
                string etag = profile.ETag;
                string email = profile.EmailAddress;
                string historyId = profile.HistoryId.ToString();
                string messagesTotal = profile.MessagesTotal.ToString();
                string threadsTotal = profile.ThreadsTotal.ToString();

                dataTable.Rows.Add(etag, email, historyId, messagesTotal, threadsTotal);

                return dataTable;
            }
            catch (Exception ex)
            {
                string exceptionMessage = "googleAuth->Gords_convert(): [" + ex.Message + "]";
                Console.WriteLine(exceptionMessage);
                return dataTable;
            }

        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        //public DataTable convertJSONtoDataTable (string strJson)
        //{
        //    DataTable? dataTable = new DataTable("DTDT");
        //    dataTable.Columns.Add("ETag");
        //    dataTable.Columns.Add("EmailAddress");
        //    dataTable.Columns.Add("HistoryId");
        //    dataTable.Columns.Add("MessagesTotal");
        //    dataTable.Columns.Add("ThreadsTotal");

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(strJson))
        //        {
        //            Console.WriteLine("%%%%%%%%%%%%%%%%");
        //            return dataTable;
        //        }

        //        //dataTable = JsonConvert.DeserializeObject<DataTable>(strJson);
                

        //        //DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
                

        //        var dataSet = JsonConvert.DeserializeObject<DataSet>(strJson);
        //        dataTable = dataSet.Tables[0];


        //        return dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        string exceptionMessage = "googleAuth->convertJSONtoDataTable(): [" + ex.Message + "]";
        //        Console.WriteLine(exceptionMessage);
        //        return dataTable;
        //    }            
        //}

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

        public string ServiceTest()
        {
            return "googleAuth-> ServiceTest";
        }

        /************************************************************************************************************************\
         *                                                                                                                      *
        \************************************************************************************************************************/

    }
}

