using Microsoft.Graph;
using Azure.Identity;
using System.Data;


//https://stackoverflow.com/questions/75604903/delegateauthenticationprovider-not-found-after-updating-microsoft-graph
//https://github.com/microsoftgraph/msgraph-sdk-dotnet/blob/feature/5.0/docs/upgrade-to-v5.md#authentication
namespace AuthModelLib;
public class graphAuth : iGAuth
{

    const string tenantId = "tugit.onmicrosoft.com";
    const string clientId = "d22e80d0-6e1d-4998-84a8-251485588156";
    const string secret = "V6M8Q~7L_9OVzvH~GjzMVb.BbR4FIh-3--Zaoc1A";

    private ClientSecretCredential csc;

    static readonly string[] _scopes = new string[]
    {
        "https://graph.microsoft.com/.default"
    };

    /************************************************************************************************************************\
    *                                                                                                                      *
    \************************************************************************************************************************/   

    public async Task<string> AuthLogin()
    {
        try
        {
            var options = new ClientSecretCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };
            csc = new ClientSecretCredential(tenantId, clientId, secret, options);

            Console.WriteLine("garminAuth->AuthLogin(): OK");
            return "OK";
        } 
        catch (Exception ex)
        {
            string exceptionMessage = "graphAuth->AuthLogin(): [" + ex.Message + "]";
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
            DataTable dt = new DataTable("OK");
            var v = csc.GetToken;
            var gsc = new GraphServiceClient(csc, _scopes);

            var mg = gsc;
            var mo = gsc.Me.GetAsync();
            var uo = gsc.Users.GetAsync();

            //var users = gsc.Users.GetAsync();
            //Console.WriteLine("AuthTokenCSCAsync : user.count[{0}]", users.Value.Count);

            Console.WriteLine("graphAuth->GetProfile() : OK");
            return dt;
        }
        catch (Exception ex)
        {
            string exceptionMessage = "graphAuth->GetProfile(): [" + ex.Message + "]";
            Console.WriteLine(exceptionMessage);
            return new DataTable(string.Format("FAIL|{0}", ex.Message));
        }

    }

    /************************************************************************************************************************\
    *                                                                                                                      *
    \************************************************************************************************************************/

    public string ServiceTest()
    {
        return "graphAuth-> ServiceTest() : OK";
    }

    /************************************************************************************************************************\
    *                                                                                                                      *
    \************************************************************************************************************************/

    public async Task<string> SendSMS(string receiverNumber, string message)
    {
        return "This functionality is not implemented in GraphAuth class";
    }

    /************************************************************************************************************************\
    *                                                                                                                      *
    \************************************************************************************************************************/

    public async Task<string> SendEmail(string adresantEmail, string adresantName, string subject, string body)
    {
        return "This functionality is not implemented in GraphAuth class";
    }

    /************************************************************************************************************************\
    *                                                                                                                      *
    \************************************************************************************************************************/
}