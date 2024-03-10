using System;
using MarkRichardson.AdminServices;

namespace MarkRichardson.AdminServices.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            AdminServicesClient client = null;
            try
            {
                Console.WindowWidth = Console.LargestWindowWidth;
                Console.Write("Press [Enter] to create the client.");
                Console.ReadLine();
                client = new AdminServicesClient();
                Console.Write("Press [Enter] to send HelloWorld message.");
                Console.ReadLine();
                Console.WriteLine(client.Channel.HelloWorld());
                Console.Write("Press [Enter] to send SetCredential message.");
                Console.ReadLine();
                Console.WriteLine(client.Channel.SetCredential("mark", "letmein", "home"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (client != null)
                    client.Dispose();
            }

            Console.Write("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
