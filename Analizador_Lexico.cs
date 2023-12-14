using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using System.IO.Ports;
using System.Security.Cryptography.X509Certificates;

namespace Compilador
{
    class Analizador_Lexico
    {
        Archivos file;
        //tabla de token
        //ArrayList tokens = new ArrayList();
        List<Token> tokens = new List<Token>();
        List<GuardarVariable> listaVariables = new List<GuardarVariable>();
        List<GurdadarDatosExpo> gurdadarDatosExpos = new List<GurdadarDatosExpo>();
        Dictionary<string, bool> variablesDeclaradas = new Dictionary<string, bool>();
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
            int[,] matriz = new int[32, 31]

    {             /*a..z, A...Z	0…9	   .	   _   menos(-) e, E   '	   "     EOL EOF	  mas(+)   *     /	     ^	   %	igual(=)  <    	 >	    ,	   ;	  (	     )	   [	  ]	     {	     }	   o	  y	      n   	O.C	 EB     */
                /*       0      1      2       3     4      5      6      7      8      9      10     11     12     13     14     15     16     17     18     19     20     21     22     23     24     25     26     27     28     29   30      */ 
           /* 0 */      {1    , 4    , 26   , -9   , 19   , 1    , 11   , 13   , 0    , 0    , 15   , 20   , 16   , 21   , 22   , 23   , 25   , 24   , 128  , 129  , 130  , 131  , 132  , 133  , 134  , 135  , 1    , 1    , 1    , -9 ,  0  },
           /* 1 */      {1    , 1    , 101  , 2    , 101  , 1    , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 1    , 1    , 1    , 101, 101  },
           /* 2 */      {3    , 3    , -1   , 2    , -1   , 3    , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , -1   , 3    , 3    , 3    , -1 , -1   },
           /* 3 */      {3    , 3    , 101  , 2    , 101  , 3    , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 101  , 3    , 3    , 3    , 101, 101   },
           /* 4 */      {102  , 4    , 5    , 102  , 102  , 7    , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102, 102   },
           /* 5 */      {-2   , 6    , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2 , -2   },
           /* 6 */      {103  , 6    , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103, 103   },
           /* 7 */      {-2   , 9    , -2   , -2   , 8    , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2 , -2   },
           /* 8 */      {-2   , 10   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2   , -2 , -2   },
           /* 9 */      {102  , 9    , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102  , 102, 102   },
           /*10 */      {103  , 10   , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103  , 103, 103   },
           /*11 */      {12   , 12   , 12   , 12   , 12   , 12   , 104  , 12   , -4   , -4   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12   , 12 , 12   },
           /*12 */      {-4   , -4   , -4   , -4   , -4   , -4   , 104  , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4   , -4 , -4   },
           /*13 */      {14   , 14   , 14   , 14   , 14   , 14   , 14   , 105  , -5   , -5   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14 , 14   },
           /*14 */      {14   , 14   , 14   , 14   , 14   , 14   , 14   , 105  , -5   , -5   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14   , 14 , 14   },
           /*15 */      {106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 112  , 106  , 106  , 106  , 106  , 115  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106  , 106, 106   },
           /*16 */      {109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 17   , 31   , 109  , 109  , 118  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109  , 109, 109   },
           /*17 */      {17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , -6   , 17   , 18   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17 , 17   },
           /*18 */      {17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , -6   , 17   , 17   , 137  , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17   , 17 , 17   },
           /*19 */      {107  , 107  , 107  , 107  , 113  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 116  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107  , 107, 107   },
           /*20 */      {108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 117  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108  , 108, 108   },
           /*21 */      {110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110  , 110, 110   },
           /*22 */      {111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111  , 111, 111   },
           /*23 */      {114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 124  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114  , 114, 114   },
           /*24 */      {119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 121  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119  , 119, 119   },
           /*25 */      {120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 122  , 120  , 123  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120  , 120, 120   },
           /*26 */      {-7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   ,-7    , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , -7   , 27   , 28   , 29   , -7 , -7   },
           /*27 */      {-8   , -8   , 125  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8 , -8   },
           /*28 */      {-8   , -8   , 126  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8 , -8   },
           /*29 */      {-8   , -8   , 127  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , 30   , -8   , -8   , -8 , -8   },
           /*30 */      {-8   , -8   , 127  , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8   , -8 , -8   },
           /*31 */      {31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 136  , 136  , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31   , 31, 31   }

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
                    else if (leer == true) { caracter = '\u000A'; terminar = true; }
                    //if (cont == cadena.Length) { }
                    //leer o no leer

                    if (caracter >= 'A' && caracter <= 'Z') { if (caracter == 'E') { c = 5; } else c = 0; } //letras may
                    else if (caracter >= 'a' && caracter <= 'z') { if (caracter == 'e') { c = 5; } else c = 0; } //letras min
                    else if (caracter >= '0' && caracter <= '9') { c = 1; } //numeros
                    else
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
                            case '\u000A': { c = 8; break; } //salto de línea \n u000A
                            case '\u000D': { c = 28; break; } //
                            case 'y': { c = 27; break; }
                            case '\u0020': { c = 30; break; } // espacio en blanco o u0020
                            case '\u0009': { c = 31; break; } //tab \t
                            case '\u0003': { c = 9; break; } //final del texto o end of file
                            default: { c = 29; break; }


                        }
                    if (caracter == 'n') { c = 28; }
                    if (caracter == 'y') { c = 27; }
                    if (caracter == 'o') { c = 26; }

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


                    if (estado != 101 && estado != 102 && estado != 104 && estado != 106
                        && estado != 109 &&
                        estado != 114 && estado != 119 && estado != 120 && estado != 107 && estado != 111)
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
                    //conver = new string(simbolo); //convertiremos el simbolo token guardado en el arreglo de chars a string
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
            Console.WriteLine("\nLista de Tokens");
            Console.WriteLine("\nR  C Token  Sim");

            foreach (Token token in tokens)
            {
                Console.WriteLine
            (token.Renglon + "  " + token.Columna + "  " + token.No_token + "   " + token.Simbolo);

            }
            Console.WriteLine("\nLista de Errores");
            Console.WriteLine("\nE  C  F  Sim");

            foreach (Error error in errores)
            {
                Errores(error.No_error);
                Console.WriteLine
            (error.No_error + "  " + error.Columna + "  " + error.Fila + "   " + error.Simbolo);
            }
            if (error != null && error.No_error <= 0)
            {
                Console.WriteLine("Errores Lexicos encontrados");
            }
            else
            {
                Bloque_Principal();
            }
            System.Console.ReadLine();


        }//dfin edl metodo


        public void Bloque_Principal()
        {
            if (((Token)(tokens[i])).No_token != 233)
            { Console.WriteLine("Error de Sintaxis. Se esperaba el nombre proyecto"); return; }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 101)
            { Console.WriteLine("Error de Sintaxis. Se esperaba el ID"); return; }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 129)
            { Console.WriteLine("Error de Sintaxis. Se esperaba ;"); return; }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 245)
            { Funcion(); }
            if (((Token)(tokens[i])).No_token == 246)
            { procedimiento(); }
            if (((Token)(tokens[i])).No_token == 134)
            { bloque_procedimiento(); }
            if (((Token)(tokens[i])).No_token == 234 || ((Token)(tokens[i])).No_token == 235 ||
                ((Token)(tokens[i])).No_token == 236 || ((Token)(tokens[i])).No_token == 237)
            { Declaracion_Variables(); } //Se identifica la declaración de algún tipo de dato, existe una declaración de variables.
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 134)
            { bloque(); }
        }

        private bool EsCompatible(string tipoDato, string valor)
        {
            if (tipoDato == "entero")
            {
                int intResult;
                return int.TryParse(valor, out intResult);
            }
            else if (tipoDato == "real")
            {
                float floatResult;
                return float.TryParse(valor, out floatResult);
            }
            // Agrega lógica para otros tipos de datos si es necesario
            else
            {
                // Tipo de dato desconocido o no soportado
                throw new ArgumentException("Tipo de dato no soportado: " + tipoDato);
            }
        }

        public void Declaracion_Variables()
        {
            string tipoDato = null;
            string idVariable = null;
            string valor = null;
            do
            {

                if (((Token)(tokens[i])).No_token == 234 || ((Token)(tokens[i])).No_token == 235 ||
                    ((Token)(tokens[i])).No_token == 236 || ((Token)(tokens[i])).No_token == 237)
                {
                    tipoDato = ((Token)(tokens[i])).Simbolo;
                }
                else
                {
                    if (((Token)(tokens[i])).No_token == 245)
                    {
                        Funcion();
                    }
                    else {
                        string lexema = ((Token)(tokens[i])).Simbolo;
                        int tipoDatoToken = Reserve(lexema);
                        if (tipoDatoToken != 0)
                        {
                            tipoDato = lexema;
                        }
                        else
                        {
                            Console.WriteLine("Error de Sintaxis, Se esperaba el tipo de dato");
                        }
                    }

                }

                do
                {
                    Siguiente_token();
                    if (((Token)(tokens[i])).No_token != 101)
                    {
                        Console.WriteLine("Error de sintaxis. Se esperaba el ID");
                    }
                    idVariable = ((Token)(tokens[i])).Simbolo;

                    Siguiente_token();
                    if (((Token)(tokens[i])).No_token == 114)
                    {
                        Siguiente_token();
                        valor = ((Token)(tokens[i])).Simbolo;
                    }
                    if (((Token)(tokens[i])).No_token == 102)
                    {
                        Siguiente_token();
                    }

                    if (variablesDeclaradas.ContainsKey(idVariable))
                    {
                        Console.WriteLine("Error: La variable con el ID '" + idVariable + "' ya ha sido declarada.");
                    }
                    else
                    {
                        // Verificación de compatibilidad entre tipo de dato y valor asignado
                        if (!EsCompatible(tipoDato, valor))
                        {
                            Console.WriteLine("Error: El valor asignado a '" + idVariable + "' no es compatible con el tipo de dato '" + tipoDato + "'.");
                            // Puedes manejar el error como desees, por ejemplo, detener la compilación o seguir con el siguiente token
                            Siguiente_token();
                        }
                        else
                        {
                            variablesDeclaradas[idVariable] = true;
                            listaVariables.Add(new GuardarVariable
                            {
                                TipoDato = tipoDato,
                                Id = idVariable,
                                Valor = valor
                            });
                        }
                    }
                }
                while (((Token)(tokens[i])).No_token == 128);
                Siguiente_token();
            }
            while (((Token)(tokens[i])).No_token != 134);


            if (((Token)(tokens[i])).No_token == 134)
            {
                bloque();
            }
        }

        public class GuardarVariable
        {
            public string TipoDato { get; set; }
            public string Id { get; set; }
            public string Valor { get; set; }
        }
        public class GurdadarDatosExpo
        {
            public string Id { get; set; }
            public string expo { get; set; }
            public string Valor { get; set; }
        }
        public class GuardarFuncion
        {
            public string IdFuncion { get; set; }
        }

        private List<GuardarFuncion> listaFunciones = new List<GuardarFuncion>();

        public void Funcion()
        {
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 245)
                Siguiente_token();
            if (((Token)(tokens[i])).No_token == 234 || ((Token)(tokens[i])).No_token == 235 ||
                ((Token)(tokens[i])).No_token == 236 || ((Token)(tokens[i])).No_token == 237)
                Siguiente_token();
            if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba el ID"); }
            string idFuncion = ((Token)(tokens[i])).Simbolo;
            if (FuncionYaDeclarada(idFuncion))
            {
                Console.WriteLine("Error: La función con el ID '" + idFuncion + "' ya ha sido declarada.");
                // Puedes manejar el error según sea necesario, como detener la compilación o seguir con el siguiente token
            }
            else
            {
                listaFunciones.Add(new GuardarFuncion { IdFuncion = idFuncion });
            }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 130) { parametro(); } else { Console.WriteLine("Error de Sintaxis, Se esperaba '('"); }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 134) { bloque_funcion(); } else { Console.WriteLine("Error de Sintaxis, Se esperaba '{'"); }
            Siguiente_token();
        }

        private bool FuncionYaDeclarada(string idFuncion)
        {
            return listaFunciones.Any(funcion => funcion.IdFuncion == idFuncion);
        }

        public void parametro()
        {
            if (tokens[i].No_token != 130) { Console.WriteLine("Error de Sintaxis, Se esperaba '('"); }
            string tipoDato = null;
            string idVariable = null;
            string valor = null;
            do
            {
                Siguiente_token();
                if (tokens[i].No_token == 234 || tokens[i].No_token == 235 ||
                    tokens[i].No_token == 236 || tokens[i].No_token == 237)
                {
                    tipoDato = ((Token)(tokens[i])).Simbolo;
                    string lexema = ((Token)(tokens[i])).Simbolo;
                    int tipoDatoToken = Reserve(lexema);
                    if (tipoDatoToken != 0)
                    {
                        tipoDato = lexema;
                    }
                    else
                    {
                        Console.WriteLine("Error de Sintaxis, Se esperaba el tipo de dato");
                    }
                }
                else Console.WriteLine("Error de Sintaxis, Falta el tipo de dato");
                Siguiente_token();
                if (tokens[i].No_token == 101) { idVariable = ((Token)(tokens[i])).Simbolo; }
                else {Console.WriteLine("Error de sintaxis. Se esperaba el ID");  }
                if (variablesDeclaradas.ContainsKey(idVariable))
                {
                    Console.WriteLine("Error: La variable con el ID '" + idVariable + "' ya ha sido declarada.");
                }
                else
                {
                    variablesDeclaradas[idVariable] = true;
                    listaVariables.Add(new GuardarVariable
                    {
                        TipoDato = tipoDato,
                        Id = idVariable,
                        Valor = valor
                    });
                }
                Siguiente_token();
                
            }
            while (tokens[i].No_token == 128);
            if (tokens[i].No_token != 131)
            {
                Console.WriteLine("Error de Sintaxis, Se esperaba )");
                return;
            }
        }

        public void bloque()
        {
            if (((Token)(tokens[i])).No_token == 134) { instrucciones(); } else { Console.WriteLine("Error de Sintaxis, Se esperaba '{'"); }
            
            if (tokens[i].No_token != 135)
            {
                Console.WriteLine("Error de Sintaxis, Falta }");
                return;
            }
            Siguiente_token();

        }

        public void instrucciones()
        {
            do
            {
                Siguiente_token();
                if (tokens[i].No_token == 101) { asignacion(); }
                if (tokens[i].No_token == 245) { Declaracion_Variables(); }
                if (tokens[i].No_token == 238) { si(); }
                if (tokens[i].No_token == 247) { leer(); }
                if (tokens[i].No_token == 243) { hacer(); }
                if (tokens[i].No_token == 244) { mientras(); }
                if (tokens[i].No_token == 241) { desde(); }
                if (tokens[i].No_token == 248) { escribir(); }
                if (tokens[i].No_token == 240) { sino(); }
            }
            while (tokens[i].No_token != 135);
            Siguiente_token();
        }
        public void asignacion()
        {
            string tipoDato = null;
            string idVariable = null;
            if (((Token)(tokens[i])).No_token == 101) {
                idVariable = ((Token)(tokens[i])).Simbolo;
                if (variablesDeclaradas.ContainsKey(idVariable))
                {
                    
                }
                else { Console.WriteLine("Error: La variable con el ID '" + idVariable + "' no ha sido declarada."); }
            }
            else
            {
                Console.WriteLine("Error de Sintaxis, Se esperaba ID");
            }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 114) { expresion(); }
            else { Console.WriteLine("Error de Sintaxis, Se esperaba ="); }
            if (((Token)(tokens[i])).No_token != 129) { Console.WriteLine("Error de Sintaxis, Se esperaba ; "); }
            Siguiente_token();
        }

        public void procedimiento()
        {
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 101)
            { Console.WriteLine("Error de Sintaxis, se esperaba ID"); }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 130)
            { parametro(); } else Console.WriteLine("Error de Sintaxis, se esperaba ( ");
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 134)
            { bloque_procedimiento(); }
            else Console.WriteLine("Error de Sintaxis, se esperaba { ");
            Siguiente_token();
        }

        public void bloque_procedimiento()
        {
            int ultimo = tokens.Count - 1;
            if (((Token)(tokens[i])).No_token != 134)
            { Console.WriteLine("Error de Sintaxis, Se esperaba {"); }
            instrucciones();
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 135)
            { Console.WriteLine("Error de Sintaxis, se esperaba }"); }

        }

        public void bloque_funcion()
        {
            bloque();
            if (((Token)(tokens[i])).No_token != 249)
            { Console.WriteLine("Error de Sintaxis, se esperaba la palabra regresa"); }
            expresion();
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 129)
            { Console.WriteLine("Error de Sintaxis, se esperaba ;"); }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 135)
            { Console.WriteLine("Error de Sintaxis, se esperaba }"); }
            Siguiente_token();


        }

        public void si()
        {
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 130)
            {
                Console.WriteLine("Error de Sintaxis, Se esperaba C");
            }
            do
            {        
                expresion();
                Siguiente_token();
                if (((Token)(tokens[i])).No_token == 239)
                {
                    entonces();
                }
                if (((Token)(tokens[i])).No_token == 240)
                {
                    sino();
                }
                if (((Token)(tokens[i])).No_token == 134)
                {
                    bloque();
                    Siguiente_token();
                }
            }
            while ((((Token)(tokens[i])).No_token != 131));
        }

        public void entonces()
        {
            
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 240)
            {
                sino();
            }
            bloque();
            return;   
        }
        public void sino()
        {
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 134)
            { instrucciones(); }
            Siguiente_token();
        }

        public void desde()
        {
            int ultimo = tokens.Count - 1;
            if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de Sintaxis, Se esperaba ("); };
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 114) { expresion(); } else Console.WriteLine("Error de Sintaxis, Se esperaba = ");
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 243) { hasta(); } else Console.WriteLine("Error de Sintaxis, Se esperaba hacer ");
            Siguiente_token();
            expresion();
            if (((Token)(tokens[i])).No_token == 135) { bloque(); } else Console.WriteLine("Error de Sintaxis, Se esperaba { ");
            Siguiente_token();

        }

        public void hacer()
        {
            int ultimo = tokens.Count - 1;
            if (((Token)(tokens[i])).No_token == 134) { bloque(); } else Console.WriteLine("Error de Sintaxis, Se esperaba { ");
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 244) { mientras(); } else Console.WriteLine("Error de Sintaxis, Se esperaba mientras ");
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 130) { expresion(); } else Console.WriteLine("Error de Sintaxis, Se esperaba ( ");
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 131)
            {
                Console.WriteLine("Error de Sintaxis, Se esperaba )");
            }

        }

        public void mientras()
        {
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 130)
            {
                Siguiente_token();
                if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Se esperaba ID"); }
                expresion();
            }
            bloque();
        }

        public void hasta()
        {
            if (((Token)(tokens[i])).No_token == 243) { expresion(); };
            Siguiente_token();
        }

        public void leer()
        {
            Siguiente_token();
            if (((Token)(tokens[i])).No_token == 130) { cadena(); } 
            else if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de Sintaxis, Se esperaba ID"); }
            if (((Token)(tokens[i])).No_token != 131) Console.WriteLine("Error de Sintaxis, Se esperaba )");
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 129) Console.WriteLine("Error de Sintaxis, Se esperaba ;");
            Siguiente_token();

        }

        public void escribir()
        {
            if (((Token)(tokens[i])).No_token != 130) Console.WriteLine("Error de Sintaxis, Se esperaba (");
            if (((Token)(tokens[i])).No_token == 101) { Console.WriteLine("Error de Sintaxis, Se esperaba ID"); }
            else if ((((Token)(tokens[i])).No_token != 101)) { cadena(); }
            Siguiente_token();
            if (((Token)(tokens[i])).No_token != 131) Console.WriteLine("Error de Sintaxis, Se esperaba )");
            if (((Token)(tokens[i])).No_token != 129) Console.WriteLine("Error de Sintaxis, Se esperaba ;");

        }
        public void cadena()
        {
            do
            {
                Siguiente_token();
                if (((Token)(tokens[i])).No_token != 105) Console.WriteLine("Error de Sintaxis, Se esperaba ID ");
                Siguiente_token();
            }
            while ((((Token)(tokens[i])).No_token != 131));
            
        }
        public void expresion()
        {
            expresion_simple();
            if (tokens[i].No_token == 119 || tokens[i].No_token == 120 ||
                    tokens[i].No_token == 121 || tokens[i].No_token == 122
                    || tokens[i].No_token == 123 || tokens[i].No_token == 124)
            {
                Siguiente_token();
                if (tokens[i].No_token == 102 || tokens[i].No_token == 102 || tokens[i].No_token == 250 || tokens[i].No_token == 251)
                {
                    Siguiente_token();
                }
                else
                {
                    Console.WriteLine("Error de Semantica, Dato no Compatible con la Expresion Dada");
                    Siguiente_token();
                }
            }

        }


        public void expresion_simple()
        {
            Siguiente_token();
            termino();
            if (tokens[i].No_token == 106 || tokens[i].No_token == 107)
            {
                Siguiente_token();
               
            }
        }

        public void termino()
        {
            factor();
            int ultimo = tokens.Count - 1;
            if (tokens[i].No_token == 106 || tokens[i].No_token == 107
                || tokens[i].No_token == 10)
            {
                factor();
                Siguiente_token();
            }
            if (tokens[i].No_token == 119 || tokens[i].No_token == 120 ||
                    tokens[i].No_token == 121 || tokens[i].No_token == 122
                    || tokens[i].No_token == 123 || tokens[i].No_token == 124)
            {
                expresion();
            }
            Siguiente_token();
        }

        public void factor()
        {
            if (((Token)(tokens[i])).No_token == 127) { Siguiente_token(); expresion(); }
            if (((Token)(tokens[i])).No_token == 101) { return; }
            if (((Token)(tokens[i])).No_token == 236) { return; }
            if (((Token)(tokens[i])).No_token == 234) { return; }
            if (((Token)(tokens[i])).No_token == 101) { return; }
            if (((Token)(tokens[i])).No_token == 235) { return; }
            if (((Token)(tokens[i])).No_token == 251) { return; }
            if (((Token)(tokens[i])).No_token == 250) { return; }
            if (((Token)(tokens[i])).No_token == 101) { lista_valores(); }
            if (((Token)(tokens[i])).No_token == 130) { expresion(); }
        }

        public void lista_valores()
        {
            do
            {
                if (tokens[i].No_token != 101) { Siguiente_token(); }
                if (tokens[i].No_token != 236) { Siguiente_token(); }
                if (tokens[i].No_token != 234) { Siguiente_token(); }
                if (tokens[i].No_token != 101) { Siguiente_token(); }
                if (tokens[i].No_token != 235) { Siguiente_token(); }
                if (tokens[i].No_token != 250) { Siguiente_token(); }
                if (tokens[i].No_token != 251) { Siguiente_token(); }
            }
            while (tokens[i].No_token == 128);
            if(tokens[i].No_token != 128) { Console.WriteLine("No se genero lista de valores"); }
            return;
        }

        public void Errores(int error)
        {
            switch (error)
            {
                case -1: { Console.WriteLine("Error léxico, se esperaba una letra."); break; }
                case -2: { Console.WriteLine("Error léxico, se esperaba un numero."); break; }
                case -4: { Console.WriteLine("Error léxico, se esperaba comilla."); break; }
                case -5: { Console.WriteLine("Error léxico, se esperaba comillas."); break; }
                case -6: { Console.WriteLine("Error léxico, se esperaba cierre de comentario."); break; }
                case -7: { Console.WriteLine("Error léxico, se esperaba un operador logico."); break; }
                case -8: { Console.WriteLine("Error léxico, se esperaba cierre del operador logico."); break; }
                case -9: { Console.WriteLine("Error léxico, se esperaba otra cosa."); break; }
            }

        }

       
        public int Reserve(string simb)
        {
            int token = 0;
            switch (simb)
            {
                case "proyecto": { token = 233; break; }
                case "real": { token = 234; break; }
                case "cadena": { token = 235; break; }
                case "entero": { token = 236; break; }
                case "logico": { token = 237; break; }
                case "si": { token = 238; break; }
                case "entonces": { token = 239; break; }
                case "sino": { token = 240; break; }
                case "desde": { token = 241; break; }
                case "hasta": { token = 242; break; }
                case "hacer": { token = 243; break; }
                case "mientras": { token = 244; break; }
                case "funcion": { token = 245; break; }
                case "procedimiento": { token = 246; break; }
                case "leer": { token = 247; break; }
                case "escribir": { token = 248; break; }
                case "regresa": { token = 249; break; }
                case "verdadero": { token = 250; break; }
                case "falso": { token = 251; break; }
            }

            return token;
         }
     
        public void Siguiente_token()
        {
            int ultimo = tokens.Count - 1;
            if (i < ultimo) { i++; }
        }

    } //fin de la clase


}