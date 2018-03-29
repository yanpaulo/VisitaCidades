using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades
{
    public static class Utils
    {
        private readonly static Random rng = new Random();

        public static Random Rand => rng;
    }
}
