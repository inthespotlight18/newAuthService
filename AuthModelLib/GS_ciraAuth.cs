using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Xml;
using System.Data;
using System.Net.Http;
using System;
//For ote EPP :         95940_epp_ote/SummerFun@1959$
//For ote Web :         95940_web_ote/SummerFun@1959$web
namespace AuthModelLib
{
    public class GS_ciraAuth : iGAuth
    {
        //" xsi:schemaLocation=\"urn:ietf:params:xml:ns:epp-1.0.xsd\">" +
        // v1.3
        // transaction ID -> ABC-12345 => GS231222-1209
        //                  "<pw>SummerFun@1959$</pw>" +
        //                  "<pw>P@ris$2806Epp</pw>" +
        // EPP server details
        const string server_epp_test_OTE = "epp.ca-ote.fury.ca"; // OTE world - created for me by CIRA
        const string server = server_epp_test_OTE;
        const int port = 700;
        const string uote = "95940_epp_ote"; // testing EPP type user
        const string pote = "SummerFun@1959$";
        const string u = uote; // not yet parameterized (check LOGIN command)
        const string p = pote;
        const string _loginXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>" + "<epp xmlns=\"urn:ietf:params:xml:ns:epp-1.0\"" + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + " xsi:schemaLocation=\"urn:ietf:params:xml:ns:epp-1.0 epp-1.0.xsd\">" + "<command>" + "<login>" + "<clID>95940_epp_ote</clID>" + "<pw>SummerFun@1959$</pw>" + "<options>" + "<version>1.0</version>" + "<lang>en</lang>" + "</options>" + "<svcs>" + "<objURI>urn:ietf:params:xml:ns:epp-1.0</objURI>" + "<objURI>urn:ietf:params:xml:ns:domain-1.0</objURI>" + "<objURI>urn:ietf:params:xml:ns:host-1.0</objURI>" + "<objURI>urn:ietf:params:xml:ns:contact-1.0</objURI>" + "<svcExtension>" + "<extURI>urn:ietf:params:xml:ns:fury-2.0</extURI>" + "<extURI>urn:ietf:params:xml:ns:fury-rgp-1.0</extURI>" + "<extURI>urn:ietf:params:xml:ns:idn-1.0</extURI>" + "<extURI>urn:ietf:params:xml:ns:secDNS-1.1</extURI>" + "<extURI>urn:ietf:params:xml:ns:launch-1.0</extURI>" + "<extURI>urn:ietf:params:xml:ns:mark-1.0</extURI>" + "<extURI>urn:ietf:params:xml:ns:signedMark-1.0</extURI>" + "<extURI>http://www.w3.org/2000/09/xmldsig#</extURI>" + "<extURI>urn:ietf:params:xml:ns:rgp-1.0</extURI>" + "<extURI>urn:ietf:params:xml:ns:fee-0.9</extURI>" + "<extURI>urn:ietf:params:xml:ns:fee-0.11</extURI>" + "</svcExtension>" + "</svcs>" + "</login>" + "<clTRID>GS231222-1209k</clTRID>" + "</command>" + "</epp>";
        const string _logoutXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<epp xmlns=\"urn:ietf:params:xml:ns:epp-1.0\"" + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + " xsi:schemaLocation=\"urn:ietf:params:xml:ns:epp-1.0 epp-1.0.xsd\">" + "<command>" + "<logout />" + "</command>" + "</epp>";
        const string _domainXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<epp xmlns=\"urn:ietf:params:xml:ns:epp-1.0\">" + "<command>" + "<check>" + "<domain:check xmlns:domain=\"urn:ietf:params:xml:ns:domain-1.0\">" + "<domain:name>fity.ca</domain:name>" + "<domain:name>mailai.ca</domain:name>" + "</domain:check>" + "</check>" + "</command>" + "</epp>";
        private static SslStream sslStream;
        private static byte[] buffer;
        public static TcpClient tcpClient = new TcpClient(server, port);

        /************************************************************************************************************************\
         *                                 Interface implementation                                                              *
        \************************************************************************************************************************/

        public async Task<string> AuthLogin()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            X509Certificate2Collection certs = new X509Certificate2Collection();
            // certs.Import("_.ca.fury.ca.crt"); // Load your CRT file
            Console.WriteLine("conEPP : Begin");
            // Connect to the server
            TcpClient tcpClient = new TcpClient(server, port);
            //SslStream sslStream = new SslStream(tcpClient.GetStream(), false,
            //                                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
            //                                    null)
            //{
            //    ReadTimeout = -1,
            //    WriteTimeout = -1
            //};
            sslStream = new SslStream(tcpClient.GetStream(), false, ValidateServerCertificate)
            {
                ReadTimeout = -1,
                WriteTimeout = -1
            };
            Console.WriteLine("conEPP : AuthenticateAsClient");
            try
            {
                sslStream.AuthenticateAsClient(server);
                // sslStream.AuthenticateAsClient(server, certs, SslProtocols.Tls12, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return "FAIL||ERROR";
            }
            Console.WriteLine("conEPP : sslStream [{0}]", sslStream.ToString());
            Console.WriteLine("conEPP : Read right away, after AuthenticateAsClient");
            Read();
            string loginXml = _loginXML;
            Console.WriteLine("Login [{0}]", loginXml);
            // Send the Login command
            buffer = Encoding.UTF8.GetBytes(loginXml);
            //sslStream.Write(buffer);
            Write(loginXml);
            //await sslStream.WriteAsync(buffer, 0, buffer.Length);
            //Read();
            sslStream.Flush();
            Read();
            Console.WriteLine("conEPP : Write & Flush for login - waiting for response !");
            // Read the response
            //StreamReader reader = new StreamReader(sslStream);
            //string response = reader.ReadToEnd();
            //Console.WriteLine("Login Response: " + response);
            string domainXml = _domainXML;
            Console.WriteLine("Domain [{0}]", domainXml);           //domain in its own function
            // Send the Domain command
            buffer = Encoding.UTF8.GetBytes(domainXml);
            Write(domainXml);
            //sslStream.Write(buffer);
            sslStream.Flush();
            Read();
            return "OK";
        }

        /************************************************************************************************************************\
         *                                                                                                                       *
        \************************************************************************************************************************/

        public async Task<string> AuthLogout()
        {
            Console.WriteLine("conEPP : Write & Flush for domain - waiting for response !");
            // Read the response
            //StreamReader reader = new StreamReader(sslStream);
            //string response = reader.ReadToEnd();
            //Console.WriteLine("Domain Response: " + response);
            string logoutXml = _logoutXML;
            Console.ReadKey();
            Console.WriteLine("Logout [{0}]", logoutXml);
            // Send the Logout command
            buffer = Encoding.UTF8.GetBytes(logoutXml);
            Write(logoutXml);
            //sslStream.Write(buffer);
            sslStream.Flush();
            Read();
            Console.WriteLine("conEPP : Write & Flush for logout - waiting for response !");
            // Read the response
            //StreamReader reader = new StreamReader(sslStream);
            //string response = reader.ReadToEnd();
            //Console.WriteLine("Logout Response: " + response);
            Console.WriteLine("conEPP : Closing");
            // Close the connection
            sslStream.Close();
            tcpClient.Close();
            Console.WriteLine("conEPP : Finish");
            Console.ReadKey();
            return "OK";

        }

        /************************************************************************************************************************\
         *           Certificate validation callback                                                                             *
        \************************************************************************************************************************/

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Implement your certificate validation logic here
            Console.WriteLine("conEPP : ValidateServerCertificate");
            return true;
        }

        /************************************************************************************************************************\
         *                                                                                                                        *
        \************************************************************************************************************************/

        public static byte[] Read()
        {
            var lenghtBytes = new byte[4];
            int read = 0;
            while (read < 4)
            {
                read = read + sslStream.Read(lenghtBytes, read, 4 - read);
            }
            Array.Reverse(lenghtBytes);
            var length = BitConverter.ToInt32(lenghtBytes, 0) - 4;
            //int integerValue = BitConverter.ToInt32(byteArray, 0);
            string len = length.ToString();
            Console.WriteLine("Read  [{0}]", len);
            //if (loggingEnabled)
            //{
            //    Debug.Log("Reading " + length + " bytes.");
            //}
            var bytes = new byte[length];
            var returned = 0;
            while (returned != length)
            {
                returned += sslStream.Read(bytes, returned, length - returned);
            }
            string result = System.Text.Encoding.UTF8.GetString(bytes);
            Console.WriteLine("Read  [{0}]", result);
            //if (loggingEnabled)
            //{
            //    Debug.Log("****************** Received ******************");
            //    Debug.Log(bytes);
            //}
            return bytes;
        }

       /************************************************************************************************************************\
        *                                                                                                                      *
       \************************************************************************************************************************/

        public static void Write(string s)
        {
            //var bytes = GetBytes(s);
            var bytes = Encoding.UTF8.GetBytes(s);
            var lenght = bytes.Length + 4;
            var lenghtBytes = BitConverter.GetBytes(lenght);
            Array.Reverse(lenghtBytes);
            sslStream.Write(lenghtBytes, 0, 4);
            sslStream.Write(bytes, 0, bytes.Length);
            sslStream.Flush();
            //if (loggingEnabled)
            //{
            //    Debug.Log("****************** Sending ******************");
            //    Debug.Log(bytes);
            //}
        }
        private static byte[] GetBytes(XmlDocument s)
        {
            return Encoding.UTF8.GetBytes(s.OuterXml);
        }

        /************************************************************************************************************************\
         *             Unused interface implementation                                                                                                         *
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

        public async Task<string> Refresh()
        {
            Console.WriteLine("---------------------------------------------------------");
            string _helloXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><epp xmlns=\"urn:ietf:params:xml:ns:epp-1.0\"><hello/></epp>";
            buffer = Encoding.UTF8.GetBytes(_helloXML);
            Write(_helloXML);
            sslStream.Flush();
            Read(); //get rid of console lines
            Console.WriteLine("conEPP : _helloXML !");
            return _helloXML;

            //variable that accepts buffer, 
        }

        /************************************************************************************************************************\
        *                                                        <?xml version="1.0" encoding="UTF-8" standalone="no"?>
<epp xmlns="urn:ietf:params:xml:ns:epp-1.0">
<hello/>
</epp>                                                              *
       \************************************************************************************************************************/

    }
}