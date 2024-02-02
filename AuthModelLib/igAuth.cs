using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Worksheets.Item.PivotTables.RefreshAll;
using System.Data;

namespace AuthModelLib
{         
        public interface iGAuth
        {
            Task<string> AuthLogin();
            Task<string> AuthLogout();
            Task<string> Refresh();
            Task<DataTable> GetProfile();
            Task<string> SendSMS(string receiverNumber, string message);
        
            Task<string> SendEmail(string adresantEmail, string adresantName, string subject, string body);

        }
}