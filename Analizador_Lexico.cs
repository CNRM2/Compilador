using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace Compilador
{
    class Analizador_Lexico
    {
        Archivos file;
        //tabla de token
        ArrayList tokens = new ArrayList();
        Token token;
        Error error;
        ArrayList errores = new ArrayList();
        int i = 0;//indice para el numero de token para analizador de sintaxis
        ArrayList variables = new ArrayList();//almacena variables declaradas
        public Analizador_Lexico()
        {
            file = new Archivos();
        }


        public Archivos File
        {
            get { return this.file; }
            set { this.file = value; }
        }

        //análisis léxico
        public void matriz_transicion(string nombre_archivo)
        {           //matriz de transición
            int[,] matriz = new int[32,31]

    {    /*a..z, A...Z	0…9	     .	   _	menos(-) e, E	 '	    "	  EOL	EOF	   mas(+)  *	   /	 ^	    %	igual(=)  <    	 >	     ,	    ;	   (	  )	     [	    ]	   {	  }	    o	   y	  n   	O.C	  EB        */

                 {1    , 4    , 26   , -9   , 19   , 1    , 11   , 13   , 0    , 0    , 15   , 20   , 16   , 21   , 22   , 23   , 25   , 24   , 128  , 129  , 130  , 131  , 132  , 133  , 134  , 135  , 1    , 1    , 1    , -9 ,  0  },
                 {1    , 1    , 101  , 2    , 101  , 1    , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 1    , 1    , 1    , 101, 101  },
                 {3    , 3    , -1   , 2    , -1   , 3    , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , 3    , 3    , 3    , -1 , -1   },
                 {3    , 3    , 101  , 2    , 101  , 3    , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 3    , 3    , 3    , 101, 101   },
                 {102  , 4    , 5    , 102  , 102  , 7    , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102, 102   },
                 {-2   , 6    , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2 , -2   },
                 {103  , 6    , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103, 103   },
                 {-2   , 9    , -2   , -2   , 8    , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2 , -2   },
                 {-2   , 10   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2 , -2   },
                 {102  , 9    , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102, 102   },
                 {103  , 10   , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103, 103   },
                 {12   , 12   , 12   , 12   , 12   , 12   , 104  , 12   , -4   , -4   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12 , 12   },
                 {-4   , -4   , -4   , -4   , -4   , -4   , 104  , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4 , -4   },
                 {14   , 14   , 14   , 14   , 14   , 14   , 14   , 105  , -5   , -5   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14 , 14   },
                 {14   , 14   , 14   , 14   , 14   , 14   , 14   , 105  , -5   , -5   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14 , 14   },
                 {106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 112  , 106  , 106  , 106  , 106  , 115  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106, 106   },
                 {109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 17   , 31   , 109  , 109  , 118  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109, 109   },
                 {17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , -6   , 17   , 18   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17 , 17   },
                 {17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , -6   , 17   , 17   , 137  , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17 , 17   },
                 {107  , 107  , 107  , 107  , 113  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 116  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107, 107   },
                 {108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 117  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108, 108   },
                 {110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110, 110   },
                 {111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111, 111   },
                 {114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 124  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114, 114   },
                 {119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 121  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119, 119   },
                 {120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 122  , 120  , 123  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120, 120   },
                 {-7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , 27   , 28   , 29   , -7 , -7   },
                 {-8   , -8   , 125  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8 , -8   },
                 {-8   , -8   , 126  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8 , -8   },
                 {-8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , 30   , -8   , -8   , -8 , -8   },
                 {-8   , -8   , 127  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8 , -8   },
                  {31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 136  , 136  , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31, 31   }
              
    };


            //leer archivo
            string line; //almacena la linea leída del archivo
            char[] cadena; // se guardará la linea leida por caracteres 
            char caracter = ' ';
            int cont = 0, end = 0; //guardaremos el número de caracter en el que nos encontramos para añadirlo a la tabla token
            int estado = 0; //almacenaremos el estado en el que nos encontramos

            int c = 0; //representa los caracteres en la matriz de trancision (columnas)
            int contarlinea = 0;

            string simbolo = "";
            bool leer = true; //comprobaremos la lectura para evitar perder tokens generados     
            bool terminar = false;
            //inicio del texto
            file.Nombre = nombre_archivo;
            File.AbrirLeer();

            while ((line = File.Leer()) != null)
            {
                contarlinea++;
                cadena = line.ToCharArray();
                cont = 0;
                end = cadena.Length;
                leer = true;
                terminar = false;

                //while (cont < cadena.Length) {

                while (terminar == false)
                {

                    if (cont < cadena.Length && leer == true) { caracter = cadena[cont]; cont++; terminar = false; }
                    else if (leer == true) { caracter = '\n'; terminar = true; }
                    //if (cont == cadena.Length) { }
                    //leer o no leer

                    if (caracter >= 'A' && caracter <= 'Z') { if (caracter == 'E') { c = 1; } else { c = 0; } } //letras may
                    if (caracter >= 'a' && caracter <= 'z') { if (caracter == 'e') { c = 1; } else { c = 0; } } //letras min
                    if (caracter >= 1 && caracter <= 9) { c = 2; } //numeros
                    switch (caracter)
                    {
                        case '\u005F': { c = 3; break; } //guion bajo _   
                        case '\u002E': { c = 2; break; } //punto .
                        case '\u0022': { c = 7; break; } //comillas "
                        case '\u0027': { c = 6; break; } //apóstrofe '
                        case '\u002C': { c = 18; break; } //coma ,
                        case '\u003B': { c = 19; break; } // punto y coma ;
                        case '\u002B': { c = 10; break; } //suma +
                        case '\u002D': { c = 4; break; } //resta - o guion
                        case '\u002F': { c = 12; break; } // diagonal /
                        case '\u002A': { c = 11; break; } //asterisco *
                        case '\u005E': { c = 13; break; } //potencia ^
                        case '\u0025': { c = 14; break; } //porcentaje %
                        case '\u003E': { c = 17; break; } //mayor que >
                        case '\u003C': { c = 16; break; } //menor que <
                        case '\u003D': { c = 15; break; } //igualdad =
                        case '\u0028': { c = 20; break; } //parentesis (
                        case '\u0029': { c = 21; break; } //parentesis )
                        case '\u007B': { c = 24; break; } // llave {
                        case '\u007D': { c = 25; break; } // llave }
                        case '\u005B': { c = 22; break; } //corchete [
                        case '\u005D': { c = 23; break; } //corchete ]
                        case '\u0026': { c = 27; break; } // &                              
                        case '\u000A': { c = 8; break; } //salto de línea \n u000A
                        case '\u000D': { c = 28; break; } //
                        case '\u0020': { c = 30; break; } // espacio en blanco o u0020
                        case '\u0009': { c = 31; break; } //tab \t
                        case '\u0003': { c = 9; break; } //final del texto o end of file
                        default: { c = 29; break; }


                    }

                    estado = matriz[estado, c];

                    if (estado < 0)
                    {
                        error = new Error();
                        error.No_error = estado;
                        error.Columna = cont;
                        error.Fila = contarlinea;
                        error.Simbolo = Convert.ToString(caracter);
                        errores.Add(error);
                        estado = 0;
                    }


                    if (estado != 101 && estado != 102 && estado != 103 && estado != 104 && estado != 105 && estado != 110 &&
                        estado != 112 && estado != 117 && estado != 119 && estado != 121 && estado != 123)
                    { leer = true; }
                    else { leer = false; }

                    if (leer == true)
                    {
                        if (estado > 0)
                        {
                            if (caracter != '\u0020' && caracter != '\u000A' && caracter != '\u0009' && caracter != '\u000D')
                            { simbolo = simbolo + caracter; }
                        }
                    }
                    if (leer == true)
                        Console.Write(caracter);

                    //conver = new string(simbolo); //convertiremos el simbolo token guardado en el arreglo de chars a string.

                    if (estado > 100)
                    {
                        token = new Token();
                        token.Renglon = contarlinea;
                        token.Columna = cont;
                        if (Reserve(simbolo) != 0)
                        { token.No_token = Reserve(simbolo); }
                        else { token.No_token = estado; }
                        token.Simbolo = simbolo;
                        tokens.Add(token);

                        estado = 0;
                        simbolo = "";//regresamos a la posicion 0 del arreglo simbolo
                        if (cont == cadena.Length && leer == false) { cont = cadena.Length - 1; leer = true; } //else { cont++; }

                    }




                }



            } //fin del texto


            file.CerrarLeer();

            Console.WriteLine("\nR  C Token  Sim");

            foreach (Token token in tokens)
            {
                Console.WriteLine
            (token.Renglon + "  " + token.Columna + "  " + token.No_token + "   " + token.Simbolo);

            }

            Console.WriteLine("\nE  C  F  Sim");

            foreach (Error error in errores)
            {
                Errores(error.No_error);
                Console.WriteLine
            (error.No_error + "  " + error.Columna + "  " + error.Fila + "   " + error.Simbolo);

            }

            estructura();
            System.Console.ReadLine();

        }//dfin edl metodo


        //análisis sintáctico 
        public void estructura()
        {
            int last = tokens.Count - 1;//último token registrado



            if (((Token)(tokens[i])).No_token != 150) { Console.WriteLine("Error de sintaxis. Se esperaba el nombre namespace."); return; }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis.Se esperaba definición del espacio."); return; }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba llave de inicio del bloque."); return; }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token == 134 || ((Token)(tokens[i])).No_token == 135 || ((Token)(tokens[i])).No_token == 136 || ((Token)(tokens[i])).No_token == 137 || ((Token)(tokens[i])).No_token == 151)
            { dec_var(); } //Se identifica la declaración de algún tipo de dato, existe una declaración de variables.
            if (((Token)(tokens[i])).No_token == 133) { principal(((Token)(tokens[i])).No_token); } //si se identifica el token de main, se ignora la declaración de variables.
            if (i < last) { i++; }
            if ((((Token)tokens[last])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba llave de cierre del bloque."); }




            Console.Write(tokens.Count);
        }


        // bloque principal main{}
        public void principal(int token)
        {
            int last = tokens.Count - 1;//último token registrado


            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
            if (i < last) { i++; }
            instrucciones(token);
            if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }


        }
        public bool exp_comp()
        {
            int last = tokens.Count - 1;//último token registrado

            bool ban = false;

            if (expresion() != true) { Console.WriteLine("Error de sintaxis."); return false; }
            ban = true;
            if (i < last) { i++; }

            do
            {
                if (((Token)(tokens[i])).No_token != 131 || ((Token)(tokens[i])).No_token != 132)
                {
                    ban = true;
                }
                else
                {

                    if (expresion() != true)
                    { Console.WriteLine("Error de sintaxis."); return false; }
                    else
                    {
                        if (i < last) { i++; }
                        ban = false;
                    }

                }
            } while (ban == false);

            return ban;
        }
        public bool expresion()
        {
            int last = tokens.Count - 1;//último token registrado

            if (exp_simp() != true) { Console.WriteLine("Error de sintaxis."); return false; }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token == 124 || ((Token)(tokens[i])).No_token == 117 || ((Token)(tokens[i])).No_token == 119 || ((Token)(tokens[i])).No_token == 120 || ((Token)(tokens[i])).No_token == 118 || ((Token)(tokens[i])).No_token == 122)
            {

                if (exp_simp() != true) { Console.WriteLine("Error de sintaxis."); }
            }

            return true;
        }
        public bool exp_simp()
        {
            int last = tokens.Count - 1;//último token registrado

            bool ban = false;

            if (termino() != true) { Console.WriteLine("Error de sintaxis."); return false; }
            if (i < last) { i++; }

            do
            {
                if (((Token)(tokens[i])).No_token != 110 || ((Token)(tokens[i])).No_token != 112)
                { ban = true; }
                else
                {

                    if (termino() != true)
                    { Console.WriteLine("Error de sintaxis."); return false; }
                    else
                    {
                        if (i < last) { i++; }
                        ban = false;
                    }
                }
            } while (ban == false);

            return ban;
        }
        public bool termino()
        {
            int last = tokens.Count - 1;//último token registrado

            bool ban = false;

            if (factor() != true) { Console.WriteLine("Error de sintaxis."); return false; }

            do
            {
                if (((Token)(tokens[i])).No_token != 102 || ((Token)(tokens[i])).No_token != 114)
                {
                    ban = true;
                }
                else
                {
                    factor();
                    if (factor() != true)
                    { Console.WriteLine("Error de sintaxis."); return false; }
                    else
                    {
                        if (i < last) { i++; }
                        ban = false;
                    }
                }
            } while (ban == false);
            return ban;

        }
        public bool factor()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token == 101 || ((Token)(tokens[i])).No_token == 103 || ((Token)(tokens[i])).No_token == 104 || ((Token)(tokens[i])).No_token == 105
                || ((Token)(tokens[i])).No_token == 106 || ((Token)(tokens[i])).No_token == 107 || ((Token)(tokens[i])).No_token == 125 || ((Token)(tokens[i])).No_token == 129)
            {
                if (((Token)(tokens[i])).No_token == 125 || ((Token)(tokens[i])).No_token == 129)
                {
                    if (i < last) { i++; }
                    expresion();
                    if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida."); return false; }

                    if (i < last) { i++; }

                    if (((Token)(tokens[i])).No_token != 126 || ((Token)(tokens[i])).No_token != 130)
                    { Console.WriteLine("Error de sintaxis. Se esperaba ( o ]"); return false; }
                    else
                    {
                        if (i < last) { i++; }
                        return true;
                    }
                }
                return true;
            }
            else { Console.WriteLine("Error de sintaxis."); return false; }

            //return true;
        }
        //instrucciones
        public void dec_var()
        {
            int last = tokens.Count - 1;
            bool ban = false;
            do
            {
                if (((Token)(tokens[i])).No_token == 134 || ((Token)(tokens[i])).No_token == 135 || ((Token)(tokens[i])).No_token == 136
                    || ((Token)(tokens[i])).No_token == 137 || ((Token)(tokens[i])).No_token == 151)
                {
                    ban = true;
                    if (i < last) { i++; }
                    if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba identificador."); return; }
                    variables.Add(((Token)(tokens[i])));
                    if (i < last) { i++; }
                    if (((Token)(tokens[i])).No_token != 123) { Console.WriteLine("Error de sintaxis. Se esperaba símbolo de asignación."); return; }
                    if (i < last) { i++; }
                    if (expresion() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida"); return; }
                    if (i < last) { i++; }
                    if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba punto y coma."); return; }
                    if (i < last) { i++; }

                }
                else { ban = false; }

            } while (ban == true);
        }

        public void instrucciones(int token)
        {
            switch (token)
            {
                case 101: { asignacion(); break; }
                case 141: { cond_if(); break; }
                case 142: { ciclo_for(); break; }
                case 143: { ciclo_while(); break; }
                case 144: { cond_switch(); break; }
                case 146: { ciclo_dowhile(); break; }
                case 148: { escribir(); break; }
                case 149: { leer(); break; }

            }

        }
        public void asignacion()
        {
            int last = tokens.Count - 1;

            //preguntamos si la variable existe 
            if (variables.Contains(((Token)(tokens[i])).Simbolo) != true) { Console.WriteLine("Error de semántica. La variable no ha sido declarada anteriormente."); }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 123) { Console.WriteLine("Error de sintaxis. Se esperaba el símbolo ="); }
            if (i < last) { i++; }
            expresion();
            if (expresion() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida."); }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
            if (i < last) { i++; }


        }
        public void ciclo_for()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba identificador "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 123) { Console.WriteLine("Error de sintaxis. Se esperaba '=' "); }
            if (i < last) { i++; }

            expresion();
            if (((Token)(tokens[i])).No_token != 101 || expresion() != true) { Console.WriteLine("Error de sintaxis. Asignación inválida. "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
            if (i < last) { i++; }

            expresion();
            if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida "); }

            if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
            if (i < last) { i++; }
            expresion();
            if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
            if (i < last) { i++; }

            bloque_instrucciones();
            if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }

            if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
            if (i < last) { i++; }

        }
        public void ciclo_while()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }

            expresion();
            if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida "); }

            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
            if (i < last) { i++; }

            bloque_instrucciones();
            if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }

            if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
            if (i < last) { i++; }


        }
        public void ciclo_dowhile()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
            if (i < last) { i++; }

            bloque_instrucciones();
            if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
            if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 143) { Console.WriteLine("Error de sintaxis. Se esperaba la palabra 'while' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }

            exp_comp();
            if (exp_comp() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida."); }

            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
            if (i < last) { i++; }

        }
        public void cond_if()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }

            exp_comp();
            if (exp_comp() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida."); }

            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
            if (i < last) { i++; }

            bloque_instrucciones();
            if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
            if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
            if (i < last) { i++; }

            //else
            if (((Token)(tokens[i])).No_token != 140)
            {
                if (i < last) { i++; }

                if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
                if (i < last) { i++; }

                bloque_instrucciones();
                if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
                if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
            }
            if (i < last) { i++; }

        }
        public void cond_switch()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba identificador "); }
            //preguntamos si la variable existe 
            if (variables.Contains(((Token)(tokens[i])).Simbolo) != true) { Console.WriteLine("Error de semántica. La variable no ha sido declarada anteriormente."); }
            if (i < last)
            {
                if (i < last) { i++; }
            }
            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
            if (i < last) { i++; }

            bool ban = true;
            do
            {
                if (((Token)(tokens[i])).No_token != 152) { Console.WriteLine("Error de sintaxis. Se esperaba la palabra case. "); ban = true; }
                else
                {
                    if (i < last) { i++; }
                    //se espera un tipo de dato para comparar
                    if (((Token)(tokens[i])).No_token != 103 || ((Token)(tokens[i])).No_token != 104 || ((Token)(tokens[i])).No_token != 105 || ((Token)(tokens[i])).No_token != 106
                    || ((Token)(tokens[i])).No_token != 107 || ((Token)(tokens[i])).No_token != 138 || ((Token)(tokens[i])).No_token != 139 || ((Token)(tokens[i])).No_token != 153)
                    { Console.WriteLine("Error de sintaxis. Se esperaba un tipo de dato. "); }
                    if (i < last) { i++; }
                    if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
                    if (i < last) { i++; }
                    bloque_instrucciones();

                    if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
                    if (((Token)(tokens[i])).No_token != 145) { Console.WriteLine("Error de sintaxis. Se esperaba 'break' "); }
                    if (i < last) { i++; }
                    if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
                    if (i < last) { i++; }
                    if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
                    if (i < last) { i++; }
                    ban = false;
                }
            } while (ban == false);

            if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
            if (i < last) { i++; }
        }
        public void escribir()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }
            bool ban = true;
            do
            {
                if (((Token)(tokens[i])).No_token != 106 || ((Token)(tokens[i])).No_token != 101)
                { Console.WriteLine("Error de sintaxis. Se esperaba string "); }
                //preguntamos si la variable existe 
                if (variables.Contains(((Token)(tokens[i])).Simbolo) != true) { Console.WriteLine("Error de semántica. La variable no ha sido declarada anteriormente."); }
                if (i < last) { i++; }
                if (((Token)(tokens[i])).No_token != 110) { i++; ban = false; } else { ban = true; }

            } while (ban == false);
            if (i < last) { i++; }

            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
            if (i < last) { i++; }
        }
        public void leer()
        {
            int last = tokens.Count - 1;//último token registrado

            if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
            if (i < last) { i++; }
            if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
            if (i < last) { i++; }
        }

        public bool bloque_instrucciones()
        {
            int last = tokens.Count - 1;//último token registrado

            bool ban = true;
            do
            {
                if (((Token)(tokens[i])).No_token != 101 || ((Token)(tokens[i])).No_token != 141 || ((Token)(tokens[i])).No_token != 142 || ((Token)(tokens[i])).No_token != 143
                || ((Token)(tokens[i])).No_token != 144 || ((Token)(tokens[i])).No_token != 146 || ((Token)(tokens[i])).No_token != 148 || ((Token)(tokens[i])).No_token != 149)
                { ban = true; }
                else
                {
                    instrucciones(((Token)(tokens[i])).No_token);
                    if (i < last) { i++; }
                    ban = false;

                }

            } while (ban == false);

            return ban;
        }


        public void Errores(int error)
        {
            switch (error)
            {
                case -1: { Console.WriteLine("Error léxico, debe finalizar con números."); break; }
                case -2: { Console.WriteLine("Error léxico, debe continuar  con números después del punto."); break; }
                case -3: { Console.WriteLine("Error léxico, símbolo indefinido. Para operador AND escribir &&."); break; }
                case -4: { Console.WriteLine("Error léxico, símbolo indefinido. Para operador OR escribir ||."); break; }
                case -5: { Console.WriteLine("Error léxico, símbolo indefinido."); break; }
                case -6: { Console.WriteLine("Error léxico, no se cerraron comillas."); break; }
                case -7: { Console.WriteLine("Error léxico, hay más de un carácter."); break; }
                case -8: { Console.WriteLine("Error léxico, no se cerró con apóstrofe."); break; }
                case -9: { Console.WriteLine("Valio Gorro"); break; }
            }

        }
        public int Reserve(string simb)
        {
            int token = 0;
            switch (simb)
            {
                case "main": { token = 133; break; }
                case "int": { token = 134; break; }
                case "string": { token = 135; break; }
                case "char": { token = 136; break; }
                case "double": { token = 137; break; }
                case "true": { token = 138; break; }
                case "false": { token = 139; break; }
                case "else": { token = 140; break; }
                case "if": { token = 141; break; }
                case "for": { token = 142; break; }
                case "while": { token = 143; break; }
                case "switch": { token = 144; break; }
                case "break": { token = 145; break; }
                case "do": { token = 146; break; }
                case "null": { token = 147; break; }
                case "write": { token = 148; break; }
                case "read": { token = 149; break; }
                case "namespace": { token = 150; break; }
                case "bool": { token = 151; break; }
                case "case": { token = 152; break; }
                case "default": { token = 153; break; }
            }

            return token;
        }

    } //fin de la clase


} //fin del namespace