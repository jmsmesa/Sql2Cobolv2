using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace Sql2Cobol
{
    public partial class Visualizador : Telerik.WinControls.UI.RadForm
    {
        private string File = string.Empty;
        private int pos = 0;

        protected cFunciones.Seguridad Seguridad = new cFunciones.Seguridad();

        public Visualizador(string Archivo)
        {
            InitializeComponent();
            File = Archivo;
            richTextBox1.LoadFile(Archivo, RichTextBoxStreamType.PlainText);
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void RadButton1_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Length != 0)
            {
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = Color.White;
                richTextBox1.Find(txtBuscar.Text, pos + txtBuscar.Text.Length + 1, richTextBox1.TextLength, RichTextBoxFinds.None);

                richTextBox1.SelectionBackColor = Color.Yellow;
                pos = richTextBox1.Text.IndexOf(txtBuscar.Text, pos + txtBuscar.Text.Length + 1) + 1;

                if (pos > 0)
                {
                    richTextBox1.Select(pos, txtBuscar.Text.Length);
                    richTextBox1.ScrollToCaret();
                }
                else
                {
                    richTextBox1.SelectAll();
                    richTextBox1.SelectionBackColor = Color.White;
                }
            }
        }

        private void RadButton3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void RadButton2_Click(object sender, EventArgs e)
        {
            Seguridad.EjecutarProceso("notepad", File, false);
        }
    }
}