using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilador
{
   public class Archivos
    {
        string nombre;//nombre del archivo
        StreamWriter sw;
        StreamReader sr;
        public Archivos()
        {
            this.nombre = "";

        }
        public Archivos(string nombre)
        {
            this.nombre = nombre;

        }
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        public void AbrirEscribir()
        {
            try
            {
                sw = new StreamWriter(this.Nombre, true);

            }
            catch (Exception e1)
            {
               Console.WriteLine("Error al abrir el archivo {0}", e1.ToString());

            }
        }
        public void Escribir(string texto)
        {

            try
            {
                this.sw.WriteLine(texto.ToString());
            }
            catch (Exception e1)
            {
              Console.WriteLine("Error al escribir el archivo {0}", e1.ToString());

            }

        }
        public void CerrarEscribir()
        {
            this.sw.Close();
        }
        public void AbrirLeer()
        {
            try
            {
                sr = new StreamReader(this.Nombre);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public String Leer()
        {
            return sr.ReadLine();
        }
        public void CerrarLeer() { sr.Close(); }
    }
}
