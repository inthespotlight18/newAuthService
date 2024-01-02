using AuthModelLib;

namespace openIDGord
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /*
            Console.WriteLine("AuthService started:");
            var SnipeITAuth = new SnipeITAuth();
            var graphAuth = new graphAuth();
            var googleAuth = new googleAuth();
            var garminAuth = new garminAuth();

            var ringCentralAuth = new RingCentralAuth();
            var HapnAuth = new hapnAuth();


            //await googleAuth.AuthLogin();
            //await SnipeITAuth.AuthLogin();
            //await graphAuth.AuthLogin();
            //await ringCentralAuth.AuthLogin();
            //await HapnAuth.AuthLogin();


            //await googleAuth.GetProfile();
            //await graphAuth.GetProfile();
            //await ringCentralAuth.GetProfile();
            //await HapnAuth.GetProfile();
            //await SnipeITAuth.GetProfile();

            await garminAuth.FirstStep();

            */


            var garminAuth = new garminAuth();
            await garminAuth.AuthLogin();






            //await googleAuth.SendEmail("dzlenko0922@gmail.com", "Daniil Zlenko", "C# TEST", "Hello from DZ program");

            //Console.WriteLine(googleAuth.ServiceTest());
            //Console.WriteLine(graphAuth.ServiceTest());

            Console.WriteLine("AuthService finished...");
            Console.ReadLine();

        }
    }
}