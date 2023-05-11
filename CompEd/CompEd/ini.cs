using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CompEd
{
    public partial class ini : Form
    {

        
        int total = 0;
        public ini()
        {
            InitializeComponent();
           

            
        }

        private void ini_DoubleClick(object sender, EventArgs e)
        {
            Ide id = new Ide();
            this.Hide();

            id.Show();
            
        }

        private void label2_DoubleClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ini_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;

            //minimum indica el valor mínimo de la barra
            progressBar1.Minimum = 0;

            //value indica desde donde se va a comenzar a llenar la barra, la nuestra iniciara desde cero
            progressBar1.Value = 0;

            //Por ejemplo podemos hacer que la barra inicie desde la mitad
            //la siguiente instrucción indica que inicie cargando desde la mitad del tamaño de la barra
            //progressBar1.Value = progressBar1.Maximum / 2;

            //step indica el paso de la barra, entre más pequeño sea la barra tardará más en cargar
            progressBar1.Step = 5;
            //// maximum indica el valor máximo de la barra
            //progressBar1.Maximum = 100;
           
            ////minimum indica el valor mínimo de la barra
            //progressBar1.Minimum = 0;

            ////value indica desde donde se va a comenzar a llenar la barra, la nuestra iniciara desde cero
            //progressBar1.Value = 0;

            ////Por ejemplo podemos hacer que la barra inicie desde la mitad
            ////la siguiente instrucción indica que inicie cargando desde la mitad del tamaño de la barra
            ////progressBar1.Value = progressBar1.Maximum / 2;

            ////step indica el paso de la barra, entre más pequeño sea la barra tardará más en cargar
            //progressBar1.Step = 5;

            //el ciclo for cargará la barra
            //for (int i = progressBar1.Minimum; i < progressBar1.Maximum; i = i + progressBar1.Step)
            //{
            //    //esta instrucción avanza la posición actual de la barra
            //    progressBar1.PerformStep();
            //    label1.Text = "cargando complementos..";
            //}
            //label1.Text = "sistema cargado.";
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = progressBar1.Minimum;
            
            total = total + 1;
            if (total == 2 && i < progressBar1.Maximum)
            {
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                label1.Text = "cargando complementos.";
            }

            if (total == 3 && i < progressBar1.Maximum)
            {
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                label1.Text = "cargando complementos..";
            }

            if (total == 4 && i < progressBar1.Maximum)
            {
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                label1.Text = "cargando complementos...";
            }

            if (total == 5 && i < progressBar1.Maximum)
            {
                label1.Text = "sistema cargado";
                
            for (int isf = i; isf < progressBar1.Maximum; isf = isf + progressBar1.Step)
            {
                //esta instrucción avanza la posición actual de la barra
                progressBar1.PerformStep();
                label1.Text = "sistema cargado";
                
            }
             

            }

            if (total == 6 )
            {
                Ide id = new Ide();
            this.Hide();

            id.Show();
            }
            





        }
    }
}
