using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsimbolos;
using ManejoDeErrores;
using System.Text.RegularExpressions;

namespace AnalizadorLexico
{
   public class Alexico
    {
      //public  int contador_Ambitoi = 0;
      //public  int contador_Ambitf = 0;
      //public  int ambito = 0;
      // TS tabla_simbolos = new TS();
      //  TE tabla_errorres = new TE();

       //public void analisisLexico (string[] Palabras_Separadas, int numero_de_lineas){

       //     foreach (var palabra in Palabras_Separadas)
       //                 {

       //                     #region Medicion del ambito

       //                     if (palabra == "{")
       //                     {
       //                         contador_Ambitoi = contador_Ambitoi + 1;
       //                     }
       //                     if (palabra == "}")
       //                     {
       //                         contador_Ambitf = contador_Ambitf + 1;
       //                     }
       //                     ambito = contador_Ambitoi;


       //                     #endregion

       //                     if (tabla_simbolos.compararAL(palabra.ToString()))// se manda a comparar la palabra con la tabla de simbolos
       //                     {
                               
       //                         // MessageBox.Show("Simbolo encontrado : "+palabra);
       //                         //                                                    simb  ,val, nunlin           ,tam,ambit,                 id_,           tipo,       descrip
       //                         tabla_de_simbolos objnuevo = new tabla_de_simbolos(palabra, "", numero_de_lineas, -0, ambito, tabla_simbolos.compararALRef(palabra.ToString()), "palabra nuevo", "palabra que coincide con la Tabla de simbolos", "");
       //                         tabla_simbolos.añadir_obj(objnuevo);

       //                         //PagCodigo.SelectionStart = PagCodigo.Find(palabra);
       //                         //PagCodigo.SelectionColor = Color.DodgerBlue;
                                
       //                     }
       //                     else//de no estar en la tabla de simbolos se agrega a un campo nuevo
       //                     {
       //                         if (Regex.IsMatch(palabra, @"[a-zA-Z]"))//sentencia que revisa los dos texbox 
       //                         {
       //                             // System.Windows.Forms.MessageBox.Show("esto es una palabra");
       //                             tabla_de_simbolos objnuevo = new tabla_de_simbolos(palabra, "", numero_de_lineas, -0, ambito, tabla_simbolos.contlineas() + 1, "palabra nuevo", "palabra que no coincide con la Tabla de simbolos,pero no se considera error", "");
       //                             tabla_simbolos.añadir_obj(objnuevo);
       //                         }
       //                         else if (Regex.IsMatch(palabra, @"\d{1}|\d{2}|\d{3}|\d{4}|\d{5}"))
       //                         {
       //                             //System.Windows.Forms.MessageBox.Show("esto es un numero");
       //                             tabla_de_simbolos objnuevo = new tabla_de_simbolos(palabra, palabra, numero_de_lineas, -0, ambito, tabla_simbolos.contlineas() + 1, "numero nuevo", "numero", "");
       //                             tabla_simbolos.añadir_obj(objnuevo);

       //                             //PagCodigo.SelectionStart = PagCodigo.Find(palabra);
       //                             //PagCodigo.SelectionColor = Color.Aquamarine;
       //                         }
       //                         else
       //                         {
       //                             // System.Windows.Forms.MessageBox.Show("Error en la expresion \n no cumple con un formato correcto ");

       //                         }
       //                       }
       //                 }//fin del analisis lexico
       //             //---------------------------------------------------------------
       //                 //#region  Expresiones regulares

       //                 //if (Regex.IsMatch(read, @"^<#int|<#integer\s+[a-z](1,15)(\s+:\s+\d(0,32000))*;$"))
       //                 //{

       //                 //    System.Windows.Forms.MessageBox.Show("esto es una sentencia int");

       //                 //}
       //                 //else if (Regex.IsMatch(read, @"^<#double|<#real\s+[a-z](1,15)(\s+:\s+\d(0,32000))*;$"))
       //                 //{
       //                 //    System.Windows.Forms.MessageBox.Show("esto es una sentencia double");

       //                 //}
       //                 //else if (Regex.IsMatch(read, @"^<#string|<#texto\s+[a-z](1,15)(\s+:\s+\'[a-z](1,15)')*;$"))
       //                 //{
       //                 //    System.Windows.Forms.MessageBox.Show("esto es una sentencia string");

       //                 //}
       //                 //else if (Regex.IsMatch(read, @"^<#bool|<#boolean\s+[a-z](1,15)(\s+:\s+(true|false))*;$"))
       //                 //{
       //                 //    System.Windows.Forms.MessageBox.Show("esto es una sentencia bool");

       //                 //}

       //                 //#endregion      
                    
       //             if (contador_Ambitf != contador_Ambitoi)
       //         {
       //             //MessageBox.Show("error de ambito");
       //             tabla_errorres.addliste(8);
       //         }
       //             //--------------------------------------------------------------
       //             }
       //             //Palabras_Separadas = null;
                }

                


       }





  
