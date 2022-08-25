//Version 0.0 Escrito por Pablo 2021/06/11
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class rAlgoritmo : Telerik.WinControls.UI.RadForm
    {
        private int nAlgoritmo = 0;
        private RadTreeView rTreeAlgoritmo = null;
        private ImageList imageList1 = null;
        public rAlgoritmo(int algoritmo)
        {
            nAlgoritmo = algoritmo;
            InitializeComponent();
            InstanciarVisor();
            this.Controls.Add(rTreeAlgoritmo);
            CargaAlgoritmo(algoritmo);
            this.Show();
        }
        
        private void InstanciarVisor()
        {
            this.rTreeAlgoritmo = new RadTreeView();
            this.rTreeAlgoritmo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            //this.doubleBufferedTableLayoutPanel1.SetColumnSpan(this.rTreeAlgoritmo, 3);
            this.rTreeAlgoritmo.ImageIndex = 0;
            this.rTreeAlgoritmo.ImageList = this.imageList1;
            this.rTreeAlgoritmo.Location = new System.Drawing.Point(3, 75);
            this.rTreeAlgoritmo.Name = "Visor_algoritmos";
            this.rTreeAlgoritmo.Size = new System.Drawing.Size(470, 522);
            this.rTreeAlgoritmo.SpacingBetweenNodes = -1;
            this.rTreeAlgoritmo.TabIndex = 3;
            this.WindowState = FormWindowState.Maximized;

        }
        public void CargaAlgoritmo(int algoritmo)
        {
            CargaAlgoritmo(algoritmo, null);
        }
        public void CargaAlgoritmo(int algoritmo, RadTreeNode nodoPadre)
        {
            bool enc = false;
            RadTreeNode raiz = null;
            DataTable dtRaiz = Business.CargaAlgoritmo(algoritmo);

            foreach(DataRow drAlg in dtRaiz.Rows)
            {
                enc = true;//el algoritmo existe
                string tiraNodo = drAlg["idAlg"] + "-" + drAlg["nombre"];
                String tiraCondicion = Lenguaje.traduce("Si") + " " + drAlg["condicion"] + " " + Lenguaje.traduce("entonces");
                //funcion recursiva, cuando la raiz sea null es el padre de todo y hay que crearlo
                if (nodoPadre is null)
                    if ((drAlg["condicion"] is DBNull) || (drAlg["condicion"].ToString().Length==0))
                    {   //Creacion inicial del primer nodo del arbol
                        raiz = this.rTreeAlgoritmo.Nodes.Add(tiraNodo);
                        raiz.Image = ((System.Drawing.Image)(Resources.alg_alg));
                        raiz.Expand();
                        raiz.TreeViewElement.ShowNodeToolTips = true;
                    }
                    else
                    {
                        raiz = this.rTreeAlgoritmo.Nodes.Add(tiraCondicion);
                        raiz.Image = ((System.Drawing.Image)(Resources.alg_question_mark));
                        raiz.Expand();
                        raiz=raiz.Nodes.Add(tiraNodo);
                        raiz.Image = ((System.Drawing.Image)(Resources.alg_alg));
                        raiz.Expand();
                    }
                else
                    if (drAlg["condicion"] is DBNull)
                    {
                        raiz = nodoPadre.Nodes.Add(tiraNodo);
                        raiz.Image = ((System.Drawing.Image)(Resources.alg_alg));
                        raiz.Expand();
                    }
                    else
                    {
                        raiz = nodoPadre.Nodes.Add(tiraCondicion);
                        raiz.Image = ((System.Drawing.Image)(Resources.alg_question_mark));
                        raiz.Expand();
                        raiz= raiz.Nodes.Add(tiraNodo);
                        raiz.Image = ((System.Drawing.Image)(Resources.alg_alg));
                        raiz.Expand();
                    }
                raiz.CheckType= Telerik.WinControls.UI.CheckType.CheckBox;
                if (drAlg["HABILITADO"].Equals("S"))
                    raiz.CheckState = Telerik.WinControls.Enumerations.ToggleState.On;
                else
                    raiz.CheckState = Telerik.WinControls.Enumerations.ToggleState.Off; 
            }
            if (!enc)
                raiz = this.rTreeAlgoritmo.Nodes.Add(algoritmo + "-" + Lenguaje.traduce("Algoritmo no encontrado"));
            CargaAcciones(algoritmo, raiz);
            raiz.ExpandAll();
        }
        void CargaAcciones(int algoritmo, RadTreeNode nodoPadre)
        {
            RadTreeNode nodoActual = null;
            try
            {
                DataTable dtAcc = Business.CargaAccionesAlgoritmos(algoritmo);
                foreach (DataRow drAcc in dtAcc.Rows)
                {
                    if ((drAcc["condicion"] is DBNull) || (drAcc["condicion"].ToString().Trim().Length <=1))
                        nodoActual = nodoPadre;
                    else
                    {
                        nodoActual = nodoPadre.Nodes.Add(Lenguaje.traduce("Si") + " " + drAcc["condicion"] + " " + Lenguaje.traduce("entonces"));
                        nodoActual.Image = ((System.Drawing.Image)(Resources.alg_question_mark));
                        nodoActual.Expand();
                    }
                    
                    
                    switch (drAcc["tipo"])
                    {
                        case "SNT":
                            MuestraSentencia(Convert.ToInt32(drAcc["sentencia"]), nodoActual, drAcc["HABILITADO"].Equals("1"));
                            //nodoActual.Image = ((System.Drawing.Image)(Resources.alg_Accion));
                            break;
                        case "ALG":
                            CargaAlgoritmo(Convert.ToInt32(drAcc["sentencia"]), nodoActual);
                            nodoActual.Image = ((System.Drawing.Image)(Resources.alg_alg));
                            nodoActual.Expand();
                            break;
                    }
                    nodoActual.Expand();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error cargando algoritmo" + " " + algoritmo + "\n" + ex.Message);
            }

        }
        private void MuestraSentencia(int idSentencia, RadTreeNode nodoPadre, bool habilitado)
        {
            RadTreeNode nodoActual;
            String tSentencia;
            String tSalida;
            try
            {
                DataTable dtSnt = Business.CargaSentenciasAlgoritmos(idSentencia);
                foreach (DataRow drSnt in dtSnt.Rows)
                {
                    //Calculo si tiene salida
                    if (drSnt["salida"] is DBNull || (drSnt["condicion"].ToString().Length <= 1))
                        tSalida = "";
                    else
                        tSalida = " => " + drSnt["salida"];
                    //Calculo si tiene condicion
                    if ((drSnt["condicion"] is DBNull) || (drSnt["condicion"].ToString().Trim().Length <=1))
                        nodoActual = nodoPadre;
                    else
                    {
                        nodoActual = nodoPadre.Nodes.Add(Lenguaje.traduce("Si") + " " + drSnt["condicion"] + " " + Lenguaje.traduce("entonces"));
                        nodoActual.Image = ((System.Drawing.Image)(Resources.alg_question_mark));
                        nodoActual.Expand();
                    }
                  
                    tSentencia = drSnt["idSNT"] + ":" + drSnt["nombre"] +"("+ drSnt["Descripcion"]+")" + tSalida;
                    nodoActual=nodoActual.Nodes.Add(tSentencia);
                    nodoActual.Expand();
                    nodoActual.ToolTipText = "Sentencia:" + "\n"+ drSnt["sentencias"].ToString()+ "\nsinull:\n"+ drSnt["sentencias"].ToString()+ 
                        "\nParametros:\n"+ drSnt["parametros"].ToString()+ "\ninicializacion:\n" + drSnt["inicializacion"].ToString()+
                        "\nIdentificador Clave:\n" + drSnt["identificadorclave"].ToString();
                    nodoActual.TreeViewElement.ShowNodeToolTips = true;
                    if (drSnt["sentencias"].ToString().ToUpper().Contains("SELECT"))
                        nodoActual.Image = ((System.Drawing.Image)(Resources.alg_SQL));
                    else
                        nodoActual.Image = ((System.Drawing.Image)(Resources.alg_Accion));
                    nodoActual.CheckType = Telerik.WinControls.UI.CheckType.CheckBox;
                    //nodoActual.Enabled = false;
                    if (habilitado)
                    {
                        nodoActual.CheckState = Telerik.WinControls.Enumerations.ToggleState.On;
                        nodoActual.ForeColor = Color.Green;
                    }   
                    else
                    {
                        nodoActual.CheckState = Telerik.WinControls.Enumerations.ToggleState.Off;
                        nodoActual.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error cargando sentencia"+ " " + idSentencia + "\n"+ex.Message);
            }
        }
        /*private void rRegla_Load(object sender, EventArgs e)
        {

        }*/
    }
}
