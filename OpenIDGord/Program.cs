using AuthModelLib;

namespace openIDGord
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("AuthService started!");
            iGAuth apiObj = null;

            while (true)
            {
                // Display the menu
                Console.WriteLine("Menu - Please, select one of the following API'S:");
                Console.WriteLine("1. Microsoft Graph");
                Console.WriteLine("2. Google Auth");
                Console.WriteLine("3. RingCentral");
                Console.WriteLine("4. SnipeITAuth");
                Console.WriteLine("5. HapnAuth");
                Console.WriteLine("6. Zoom");
                Console.WriteLine("7. Exit program");



                // Get user input
                Console.Write("Enter your choice (1-7): ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    
                    // Use a switch block to handle different cases
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("You selected Microsoft Graph ");
                            apiObj = new graphAuth();
                            break;
                        case 2:
                            Console.WriteLine("You selected Google Auth");
                            apiObj = new googleAuth();
                            break;
                        case 3:
                            Console.WriteLine("You selected RingCentral ");
                            apiObj = new RingCentralAuth();
                            break;
                        case 4:
                            Console.WriteLine("You selected SnipeITAuth ");
                            apiObj = new SnipeITAuth();
                            break;
                        case 5:
                            Console.WriteLine("You selected HapnAuth ");
                            apiObj = new hapnAuth();
                            break;
                        case 6:
                            Console.WriteLine("You selected Zoom");
                            apiObj = new zoomAuth();
                            break;
                        case 7:
                            Console.WriteLine("Exiting program");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                if (apiObj != null)
                {

                    Console.WriteLine($"apiObj type: {apiObj.GetType()}, value: {apiObj}");
                    Console.WriteLine();

                    //Make API calls here
                    await apiObj.AuthLogin();
                    await apiObj.GetProfile();

                    Console.WriteLine("AuthService finished...");
                    Console.ReadLine();

                }

            }
        }
    }
}
