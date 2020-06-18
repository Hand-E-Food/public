using PsiFi.Engines;
using PsiFi.Views.ColorConsole;

namespace PsiFi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var applicationEngine = new ApplicationEngine(new MainMenu());
            applicationEngine.Begin();
        }
    }
}
