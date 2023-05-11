using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsimbolos;

namespace AnalizadorSintactico
{
   public class AS
    {
       TS TSimbol = new TS();

       public AS()
       {
            
       }

       public void AnalisisSintactico(){
           string sentencia = "";
           int numerolinea=1;
          
          
           foreach (var token in  TSimbol.llamatabla())
           {
               //if (token.NumLinea == numerolinea)
               //{
                   sentencia = sentencia +" "+ token.simbolo.ToString();
               //}
              

           }
                System.Windows.Forms.MessageBox.Show("" + sentencia);



       }

       



    }
}
