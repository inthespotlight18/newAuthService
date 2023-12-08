using System.Data;

namespace AuthModelLib
{         

        public interface iGAuth
        {
            Task<string> AuthLogin();
            Task<DataTable> GetProfile();

            Task<string> SendSMS(string receiverNumber, string message);

            Task<string> SendEmail(string adresantEmail, string subject, string body);

        }
    

        

    
}