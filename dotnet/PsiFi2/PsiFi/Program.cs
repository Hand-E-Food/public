using PsiFi.ConsoleForms;
using PsiFi.Mapping;
using PsiFi.Mapping.Creation;
using Rogue;
using System;

namespace PsiFi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 50);
            Console.SetBufferSize(80, 50);

            var map = new CircularMapCreationStrategy(new Size(41, 41)).CreateMap();
            var engine = new MapEngine
            {
                Map = map,
                MapScreen = new MapScreen {
                    Title = "Psi-Fi",
                },
                Player = new Player {
                    Cell = map[20, 20],
                },
            };
            
            engine.Initialize();
            var result = engine.Play();
            
            Console.ResetColor();
        }
    }
}
