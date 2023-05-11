using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
	public class Error
	{
		int numero; //numero de error
		int columna;//dónde se encuentra el error
		int fila; //dónde se encuentra el error
		string simbolo;
		public Error()
		{
			this.numero = 0;
			this.columna = 0;
			this.fila = 0;
			this.simbolo = "";
		}
		public Error(int num, int col, int fil, string sim)
		{
			this.numero = num;
			this.columna = col;
			this.fila = fil;
			this.simbolo = sim;
		}

		public int No_error
		{
			get { return this.numero; }
			set { this.numero = value; }
		}
		public int Columna
		{
			get { return this.columna; }
			set { this.columna = value; }
		}
		public int Fila
		{
			get { return this.fila; }
			set { this.fila = value; }
		}
		public string Simbolo
		{
			get { return this.simbolo; }
			set { this.simbolo = value; }
		}
	}
}
