using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitaCidades.Model;

namespace VisitaCidades
{
    public class Bootstrapper
    {
        public static void Run(string[] args)
        {
            if (args.Any(a => a.Contains("?") || a.ToLower().Contains("help")))
            {
                AlgoritmoFabrica.DisplayHelp();
                return;
            }

            try
            {
                using (var game = new Game1(AlgoritmoFabrica.Create(args)))
                    game.Run();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
