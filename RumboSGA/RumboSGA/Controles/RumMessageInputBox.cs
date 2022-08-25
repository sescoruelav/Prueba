using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Controles
{
    public partial class RumMessageInputBox : Telerik.WinControls.UI.RadForm
    {
        
        public object input;
        public string path;
        public string nombre;
        public string tipoPadre;
        public string idMotivo;
        public RumMessageInputBox()
        {
            InitializeComponent();
        }

        public RumMessageInputBox(string titulo, string pregunta,string tipoPadre)
        {
            InitializeComponent();
            this.Text = titulo;
            this.lblInput.Text = pregunta;
            this.tipoPadre = tipoPadre;

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.OK;
            input = txtInput.Text;
            idMotivo = rmccmbMotivo.SelectedValue.ToString();
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {

            openFileDialog1.InitialDirectory = @"X:\";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path =Path.GetDirectoryName(openFileDialog1.FileName);
                nombre =Path.GetFileName(openFileDialog1.FileName);
                lblPathImagen.Text = openFileDialog1.FileName;
                

            }
        }

        private void RumMessageInputBox_Load(object sender, EventArgs e)
        {
            Utilidades.RellenarMultiColumnComboBox(ref rmccmbMotivo, DataAccess.GetIdMotivosComentario(tipoPadre), "IDMOTIVOCOMENTARIO", "DESCRIPCION", "OTROS", null, false);
        }
    }
}
