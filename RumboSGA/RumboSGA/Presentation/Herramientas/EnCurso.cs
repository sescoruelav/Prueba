using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Diagrams;
using Telerik.Windows.Diagrams.Core;


namespace RumboSGA.Presentation.Herramientas
{
    public partial class EnCurso : Telerik.WinControls.UI.RadForm
    {
       
        public struct Esquema
        {
            public string esquema;
            public List<Elemento>  elementos;
        };
        public struct Elemento
        {
            public string nombre;
            public string tipoGrafica;
            public string icono;
            public int x;
            public int y;
            public int lonX;
            public int lonY;
            public string sql;
            public RadDiagramShape shape1;
        };
        Hashtable shapes = new Hashtable();
        private int maxPosX = 190;
        private int maxPosY = 60;
        private int lonX = 170;
        private int lonY = 50;
        string path = Persistencia.DirectorioBase;
        List<Esquema> esquemas;
        public EnCurso()///////////////////////////////////////////////////////////////
        {
            InitializeComponent();
            this.Show();
            esquemas = CargarEsquemas();
            if (esquemas is null)
                return;
        }
        private void radDiagram1_Click(object sender, EventArgs e)
        {

        }
       
        List<Esquema> CargarEsquemas()
        {
            try
            {
                string origen = Persistencia.DirectorioBase + @"\configs\EnCurso.json";
                StreamReader read = new StreamReader(origen);
                string json = @read.ReadToEnd();
                List<Esquema> esquemas = JsonConvert.DeserializeObject<List<Esquema>>(json);
                return esquemas;
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("El fichero de configuración de en curso es incorrecto") + "\n" + e.Message);
            }
            return null;
        }
        public void MostrarEsquema(string esqMostrar)
        {
            bool enc = false;
            try
            {
                for(int nEsq=0; nEsq<esquemas.Count && !enc; nEsq++)
                {
                    Esquema esquema = esquemas[nEsq];
                    if (esquema.esquema.ToLower().Equals(esqMostrar.ToLower()))
                    {
                        enc = true;
                        for (int ele = 0; ele < esquema.elementos.Count; ele++)
                        {
                            Elemento oper = (Elemento)esquema.elementos[ele];
                            switch (oper.tipoGrafica.ToLower())
                            {
                                case "pedidocli":
                                case "pedidopro":
                                case "recepcion":
                                case "tarea":
                                case "rutas":
                                case "ordenrecogida":
                                    InstanciarOperativa(oper);
                                    break;
                                case "relacion":
                                    InstanciarRelacion(oper);
                                    break;
                                case "timer":
                                    //AjustaTimer(ctrl);
                                    break;
                                default:
                                    MessageBox.Show(Lenguaje.traduce("Tipo de gráfica desconocida") + ":" + oper.tipoGrafica + "\n en control: " + oper.nombre);
                                    break;
                            }
                            ResetMaxY();
                        }
                        
                    }
                }
                //panelCT.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Test" + e.Message);
            }
        }

        void ResetMaxY()
        {
            maxPosY = 0;
        }
        private void InstanciarOperativa(Elemento oper)
        {
            DataTable newDt;
            try
            {
                newDt = Business.GetWarnings(oper.sql, 0);//idusuario
                foreach(DataRow row in newDt.Rows)
                {
                    RadDiagramShape shape = new RadDiagramShape()
                    {
                        Text = Convert.ToString(row["ID"]),
                        ElementShape = new RoundRectShape(0),
                        BackColor = System.Drawing.Color.Cyan
                    };
                    shape.Position = new Telerik.Windows.Diagrams.Core.Point((oper.x-1) *(lonX+ 70), maxPosY);
                    shape.ToolTipText = Convert.ToString(row["tooltip"]);
                    shape.DiagramShapeElement.Image =(System.Drawing.Image) Resources.ResourceManager.GetObject(oper.icono);
                    maxPosY += lonY+ 10;
                    shape.Width = lonX;
                    shape.Height = lonY;
                    shape.DiagramShapeElement.TextAlignment = ContentAlignment.MiddleRight;
                    shape.DiagramShapeElement.ImageLayout = ImageLayout.Zoom;
                    shape.DiagramShapeElement.Padding = new System.Windows.Forms.Padding(5, 2, 2, 0);
                    shape.DiagramShapeElement.ImageAlignment = ContentAlignment.MiddleLeft;
                    shape.DiagramShapeElement.TextImageRelation = TextImageRelation.ImageBeforeText;
                    radDiagram1.Items.Add(shape);
                    shapes.Add(Convert.ToString(row["ID"]), shape);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("error\n" + e.Message);
            }
        }
        private void InstanciarRelacion(Elemento oper)
        {
            DataTable newDt;
            try
            {
                newDt = Business.GetWarnings(oper.sql, 0);//idusuario
                foreach (DataRow row in newDt.Rows)
                {
                    RadDiagramConnection connection = new RadDiagramConnection()
                    { Name = "connection" };
                    connection.Source = (RadDiagramShape)shapes[Convert.ToString(row["origen"])];
                    connection.Target = (RadDiagramShape)shapes[Convert.ToString(row["destino"])];
                    connection.Text = Convert.ToString( shapes[Convert.ToString(row["txt"])]);
                    radDiagram1.Items.Add(connection);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("error\n" + e.Message);
            }
        }
    }
}
