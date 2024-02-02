using System.Data;

namespace AuthModelLib
{         

        public interface IgOauth1
        {
            Task<string> getRequestToken();

            string setVerifier();

            Task<string> getAccessToken();

            //Task<string> SendSMS(string receiverNumber, string message);

            //Task<string> SendEmail(string adresantEmail, string adresantName, string subject, string body);

        }
    

        

    
}