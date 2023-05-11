using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Token
    {
        int renglon, columna, no_token;
        string simbolo;

        public Token()
        {
            this.renglon = 0;
            this.columna = 0;
            this.no_token = 0;
            this.simbolo = "";
        }
        public Token(int ren, int col, int ntoken, string sim)
        {
            this.renglon = ren;
            this.columna = col;
            this.no_token = ntoken;
            this.simbolo = sim;
        }

        public int Renglon
        {
            get { return this.renglon; }
            set { this.renglon = value;}
        }
        public int Columna
        {
            get { return this.columna; }
            set { this.columna = value; }
        }
        public int No_token
        {
            get { return this.no_token; }
            set { this.no_token = value; }
        }
        public string Simbolo
        {
            get { return this.simbolo; }
            set { this.simbolo = value; }
        }
    }
}
