using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Telerik.WinControls.UI;
using RumboSGAManager.Model.DataContext;
using System.Drawing;
using Telerik.WinControls;
using XML_Edit;

namespace RumboKanban
{
    //Pablo Boix Ene 2022 

    /* esta clase carga un json llamado RumboKanban.json y crea un tablero kanban con esa configuracion .
     * cada Columna puede tener varias fuentes, por ejemplo EN CURSO puede tener la fuente recepciones y expediciones
     * Cada fuente del json pertenece a una columna, pero en una columna se pueden cargar diferentes lineas.
     * 
     * la clase de telerik se puede ver en https://www.telerik.com/blogs/master-workflow-visualizations-new-telerik-taskboard-winforms
     * 
     * Version 1.0 En curso Solo muestra el tablero kanban, no permite cambiar tarjetas de columna
     * Version 2.0 TODO Permite cambiar tarjetas de todo a en curso y desencadena acciones. 
     */
    public class RumboKanban: Telerik.WinControls.UI.RadRibbonForm
    {
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadTaskBoard radTaskBoard1;

        private System.ComponentModel.IContainer components = null;
        static DataTable fuentesTarjetas;
        const int COLUMNAS_CONFIG =3;//número de columnas de configuracion de la select antes de los tags 3: id, referencia y texto, el resto son tags
        struct SColumna
        {
            public string nombreColumna;
            public RadTaskBoardColumnElement rtbColumna;
        }
        struct STarjeta
        {
            public string identificador;
            public bool permanece;
        }
        private Dictionary<string, SColumna> columnas = new Dictionary<string, SColumna>();
        private RadMenuButtonItem rBtnRefresh;
        private RadMenuButtonItem rBtnConfig;
        private Dictionary<string, STarjeta> tarjetas = new Dictionary<string, STarjeta>();

        public RumboKanban()
        {
            InitializeComponent();
            //ConfiguraEventos(); //TODO terminar implementacion
            this.AllowAero = false;
            if(!CargaConfiguracion())
                return;
            CreaColumnas();
            CargaTarjetas();
            this.Show();
        }
        #region logica interna ///////////////////////////////////////////
        bool CreaColumnas()
        {
            string nombreColumna;
            SColumna vColumna;
            RadTaskBoardColumnElement vrtbCol;
            try
            {
                //recorro la configuracion buscando las diferentes columnas. Cuando encuentro una columna nueva la creo
                foreach (DataRow drw in fuentesTarjetas.Rows)
                {
                    nombreColumna = Convert.ToString(drw["column"]);
                    if (!columnas.TryGetValue(nombreColumna, out vColumna))
                    {   
                        //Si no existe la columna la creo.
                        vColumna = new SColumna();
                        vColumna.nombreColumna = nombreColumna;
                        
                        vrtbCol = new RadTaskBoardColumnElement();
                        vrtbCol.Title = nombreColumna;
                        radTaskBoard1.Columns.Add(vrtbCol);
                        vColumna.rtbColumna = vrtbCol;
                        vColumna.rtbColumna.AddTaskCardButton.Visibility = ElementVisibility.Hidden ;//Evitamos que añadan tarjetas a mano, quitamos + add a card.
                        vColumna.rtbColumna.AllowDrag = false;//En la version 1.X no se permite drag and drop
                        vColumna.rtbColumna.AllowDrop = false;//En la version 1.X no se permite drag and drop 
                        columnas.Add(nombreColumna, vColumna);
                    }
                }
                return true;
            }
            //TODO mejorar gestion y traduccion de errores
            catch(ArgumentException ex)
            {
                MessageBox.Show("no se ha encontrado la columna column en el fichero de configuracion Kanban" );
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("se ha producido un error leyendo el fichero de configuracion Kanban" + "\n" +ex.Message);
                return false; 
            }
        }
        private void CargaTarjetas()
        {
            DataTable dt;
            string nombreColumnaActual;
            SColumna vColumna;
            RadTaskBoardColumnElement columnaActual;
            RadTaskCardElement tarjetaActual;
            string tiraSQL="";
            try
            {
                //recorro la configuracion buscando las diferentes fuentes de tarjetas de las columnas. Cuando encuentro una columna nueva la creo
                foreach (DataRow drw in fuentesTarjetas.Rows)
                {
                    nombreColumnaActual = Convert.ToString(drw["column"]);
                    //busco la columna actual
                    if (columnas.TryGetValue(nombreColumnaActual, out vColumna))
                    {
                        columnaActual = vColumna.rtbColumna;
                        tiraSQL = Convert.ToString(drw["SQL"]);
                        dt = ConexionSQL.getDataTable(tiraSQL);
                        if(!(dt is null))
                        {
                            //Ejecuto la tira sql y creo las tarjetas correspondientes
                            foreach (DataRow dr in dt.Rows)
                            {
                                tarjetaActual = new RadTaskCardElement();
                                tarjetaActual.Name = Convert.ToString(dr["titulo"]);
                                tarjetaActual.TitleText = Convert.ToString(dr["titulo"]);
                                tarjetaActual.DescriptionText = Convert.ToString(dr["texto"]);
                                tarjetaActual.BackColor = BuscaColor(Convert.ToString(drw["backColor"])); //color de las tarjetas;
                                tarjetaActual.AccentSettings.Color = BuscaColor(Convert.ToString(drw["accentColor"])); 
                                tarjetaActual.ForeColor = BuscaColor(Convert.ToString(drw["color"])); //color de las tarjetas;
                                //Cargamos las columna de tags
                                int col = 0;
                                foreach(object dc in dr.ItemArray)
                                {
                                    col++;
                                    if (col > COLUMNAS_CONFIG)//las primeras columnas no son tags
                                    {
                                        RadTaskCardTagElement tag1 = new RadTaskCardTagElement();
                                        tag1.Text =dc.ToString();
                                        tarjetaActual.TagElements.Add(tag1);
                                    }
                                    
                                }
                                columnaActual.TaskCardCollection.Add(tarjetaActual);
                            }
                        }
                        //TODO  guardar en el log que ha habido un error sql 
                        tiraSQL = "";//evito que una excepcion muestre una SQL anterior. 
                        //TODO mejorar la gestion de errores. una SQL mal construida DEBE generar una excepcion y  controlarse en el front, no en el back
                    }
                    else
                    {
                        //TODO poner en el log que hay un funcionam
                    }
                    
                }
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show("La sentencia select es incorrecta" +"\n"+ ex.Message +"\n"+ tiraSQL);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CargaConfiguracion()
        {
            //string origen = Persistencia.DirectorioBase + @"\configs\RumboKanban.json";
            //TODO Incluir persistencia
            string origen =  @"c:\rumbo\configs\RumboKanban.json";
            try
            {
                StreamReader read = new StreamReader(origen);
                string json = @read.ReadToEnd();
                fuentesTarjetas = JsonConvert.DeserializeObject<DataTable>(json);
                return true;
            }
            catch(FileNotFoundException ex)
            {
                //TODO centralizar gestionde errores
                //TODO traducir mensaje
                MessageBox.Show("No se ha encontrado el fichero de configuración Kankan" + origen );
                return false;
            }
            catch(JsonReaderException ex)
            {
                //TODO centralizar gestionde errores
                //TODO traducir mensaje
                MessageBox.Show("El fichero de configuracion Kanban está mal formado" + "\n"+ origen);
                //ir a modificar
                return false;
            }
            catch (Exception ex)
            {
                //TODO centralizar gestionde errores
                //TODO traducir mensaje
                MessageBox.Show("error cargando JSON configuración Kankan" + "\n origen" + origen + ex.Message);
                return false;
            }
        }


        private void ConfiguraEventos()
        {
            //TODO al mover una tarjeta de una lista a otra debe desncadenar reservas o lanzaminentos
            throw new NotImplementedException();
        }
        #endregion logica interna /////////////////////////////////////
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.rBtnRefresh = new Telerik.WinControls.UI.RadMenuButtonItem();
            this.rBtnConfig = new Telerik.WinControls.UI.RadMenuButtonItem();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radTaskBoard1 = new Telerik.WinControls.UI.RadTaskBoard();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTaskBoard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonBar1.Size = new System.Drawing.Size(474, 146);
            this.radRibbonBar1.StartButtonImage = null;
            this.radRibbonBar1.StartMenuItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnRefresh,
            this.rBtnConfig});
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "Rumbo Kanban";
            // 
            // rBtnRefresh
            // 
            // 
            // 
            // 
            this.rBtnRefresh.ButtonElement.ShowBorder = false;
            this.rBtnRefresh.Name = "rBtnRefresh";
            this.rBtnRefresh.Text = "Refresh";
            this.rBtnRefresh.Click += new System.EventHandler(this.rBtnRefresh_Click);
            // 
            // rBtnConfig
            // 
            // 
            // 
            // 
            this.rBtnConfig.ButtonElement.ShowBorder = false;
            this.rBtnConfig.Name = "rBtnConfig";
            this.rBtnConfig.Text = "Config";
            this.rBtnConfig.Click += new System.EventHandler(this.rBtnConfig_Click);
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 401);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(474, 24);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radTaskBoard1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 255);
            this.panel1.TabIndex = 2;
            // 
            // radTaskBoard1
            // 
            this.radTaskBoard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTaskBoard1.Location = new System.Drawing.Point(0, 0);
            this.radTaskBoard1.Name = "radTaskBoard1";
            this.radTaskBoard1.Size = new System.Drawing.Size(474, 255);
            this.radTaskBoard1.TabIndex = 0;
            // 
            // RumboKanban
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 425);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "RumboKanban";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Rumbo Kanban";
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTaskBoard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion  Windows Form Designer generated code

        private void rBtnConfig_Click(object sender, EventArgs e)
        {
            string ficheroaEditar = "";
            try
            {
                //TODO resolver con la clase persistencia
                ficheroaEditar = @"C:\Rumbo\Configs\rumbokanban.json";
                AuxEditor editor2 = new AuxEditor(ficheroaEditar, AuxType.DataTable);
                MessageBox.Show("Debe reiniciar el tablero Kanban para que tenga efecto la nueva configuración"); //TODO traduce
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void rBtnRefresh_Click(object sender, EventArgs e)
        {
            CargaTarjetas();
        }
        System.Drawing.Color BuscaColor(string color)
        {
            //TODO mover a una clase de utilidades general de aqui y de torrecontrol
            switch (color)
            {
                case "rojo":
                case "red":
                    return System.Drawing.Color.Red;
                case "verde":
                case "green":
                    return System.Drawing.Color.Green;
                case "naranja":
                case "orange":
                    return System.Drawing.Color.Orange;
                case "amarillo":
                case "yellow":
                    return System.Drawing.Color.Yellow;
                case "azul":
                case "blue":
                    return System.Drawing.Color.Blue;
                case "blanco":
                case "white":
                    return System.Drawing.Color.White;
                default:
                    return System.Drawing.Color.Black;
            }
        }

    }

}
