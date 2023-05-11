using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Analizador_Lexico pr = new Analizador_Lexico();
         
            pr.matriz_transicion("archivo.txt");
			//pr.estructura();
           
            Console.ReadLine();


        }

		public int func() { return 3; }
    }
}
