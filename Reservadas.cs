using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
	public class Reservadas
	{
		string nombre;
		int token; //numero de token que genera

		public Reservadas() { this.nombre = ""; this.token = 0; }
		public Reservadas(string nom, int tok) { this.nombre = nom; this.token = tok; }

	}
}
