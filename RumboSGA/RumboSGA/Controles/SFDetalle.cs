using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public partial class SFDetalle : Form
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<NameValue> listaNV = new List<NameValue>();
        public bool ComprobarValores = false;
        public bool editado = false;
        public dynamic newRecord;
        private RumButton rumButtonAceptar = new RumButton();
        private RumButton rumButtonCancelar = new RumButton();
        private RumButton rumButtonEditar = new RumButton();
        private Panel panelCabezera;
        private Panel panelDatos;
        private RumLabel rumLabelTitulo = new RumLabel();
        private List<TableScheme> lstEsquemaTabla;
        private string tablaPrincipal;

        public SFDetalle(string nombre, List<TableScheme> lstEsquemaTabla, dynamic selectedRow, bool nuevo, bool clonar, string tablaPrincipal)
        {
            InitializeComponent();

            controlesBotones();
            log.Info(lstEsquemaTabla.ToString());
            RumCargaTablaDetalle cargaTablaDetalle = new RumCargaTablaDetalle();
            panelDatos = cargaTablaDetalle.CargaTablaDetalle(nombre, lstEsquemaTabla, selectedRow, nuevo, clonar, tablaPrincipal, this.Width);
            listaNV = cargaTablaDetalle.listaNV;
            this.Controls.Add(this.panelPrincipal);
            this.panelPrincipal.AutoScroll = true;
            this.panelPrincipal.Controls.Add(this.panelDatos);
            this.panelDatos.BringToFront();
            this.tablaPrincipal = tablaPrincipal;
            if (nuevo || clonar)
            {
                rumButtonEditar.Visible = false;
            }
        }

        private void controlesBotones()
        {
            //
            // panelCabezera
            //
            panelCabezera.AutoScroll = true;
            panelCabezera.Dock = System.Windows.Forms.DockStyle.Top;
            panelCabezera.Name = "panelCabezera";
            panelCabezera.TabIndex = 0;
            panelCabezera.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            //
            // panelDatos
            //
            panelDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            panelDatos.Name = "panelDatos";
            panelDatos.TabIndex = 1;
            panelDatos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            panelDatos.AutoScroll = true;
            //
            // rumButtonAceptar
            //
            rumButtonAceptar.BackColor = System.Drawing.Color.Transparent;
            rumButtonAceptar.Dock = System.Windows.Forms.DockStyle.Right;
            rumButtonAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            rumButtonAceptar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            rumButtonAceptar.Location = new System.Drawing.Point(738, 57);
            rumButtonAceptar.Margin = new System.Windows.Forms.Padding(10);
            rumButtonAceptar.MaximumSize = new System.Drawing.Size(74, 60);
            rumButtonAceptar.Name = "rumButtonAceptar";
            //
            //
            //
            rumButtonAceptar.RootElement.MaxSize = new System.Drawing.Size(74, 60);
            rumButtonAceptar.Size = new System.Drawing.Size(74, 60);
            rumButtonAceptar.TabIndex = 3;
            rumButtonAceptar.Text = "Aceptar";
            rumButtonAceptar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            //
            // rumButtonCancelar
            //
            rumButtonCancelar.BackColor = System.Drawing.Color.Transparent;
            rumButtonCancelar.Dock = System.Windows.Forms.DockStyle.Right;
            rumButtonCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            rumButtonCancelar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            rumButtonCancelar.Location = new System.Drawing.Point(812, 57);
            rumButtonCancelar.Margin = new System.Windows.Forms.Padding(10);
            rumButtonCancelar.MaximumSize = new System.Drawing.Size(74, 60);
            rumButtonCancelar.Name = "rumButtonCancelar";
            //
            //
            //
            rumButtonCancelar.RootElement.MaxSize = new System.Drawing.Size(74, 60);
            rumButtonCancelar.Size = new System.Drawing.Size(74, 60);
            rumButtonCancelar.TabIndex = 2;
            rumButtonCancelar.Text = "Cancelar";
            rumButtonCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            //
            // rumButtonEditar
            //
            rumButtonEditar.BackColor = System.Drawing.Color.Transparent;
            rumButtonEditar.Image = global::RumboSGA.Properties.Resources.edit;
            rumButtonEditar.Dock = System.Windows.Forms.DockStyle.Left;
            rumButtonEditar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            rumButtonEditar.Location = new System.Drawing.Point(12, 60);
            rumButtonEditar.MaximumSize = new System.Drawing.Size(74, 60);
            rumButtonEditar.Name = "rumButtonEditar";
            //
            //
            //
            rumButtonEditar.Size = new System.Drawing.Size(74, 60);
            rumButtonEditar.TabIndex = 1;
            rumButtonEditar.Text = "Editar";
            rumButtonEditar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            rumButtonEditar.Click += new System.EventHandler(this.RumButtonEditar_Click);
            //
            // rumLabelTitulo
            //
            rumLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            rumLabelTitulo.Location = new System.Drawing.Point(75, 60);
            rumLabelTitulo.Name = "rumLabelTitulo";
            rumLabelTitulo.Dock = System.Windows.Forms.DockStyle.Left;
            rumLabelTitulo.Size = new System.Drawing.Size(74, 60);
            rumLabelTitulo.TabIndex = 6;
            rumLabelTitulo.Text = "Detalles";

            panelCabezera.Controls.Add(rumButtonCancelar);
            panelCabezera.Controls.Add(rumButtonAceptar);
            panelCabezera.Controls.Add(rumLabelTitulo);
            panelCabezera.Controls.Add(rumButtonEditar);
        }

        private void RumButtonEditar_Click(object sender, EventArgs e)
        {
            rumButtonEditar.Visible = false;
            rumButtonAceptar.Visible = true;
            rumButtonCancelar.Visible = true;
            editableNameValues(true);
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            if (tablaPrincipal == "TBLHUECOS")
            {
                ComprobarValores = false;
            }

            if (!ComprobarValores)
            {
                string error = "";
                this.newRecord = getLineaValoresValidos(ref error).ToString();
                if (!error.Equals(""))
                {
                    RadMessageBox.Show(error);
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            editado = true;
            Close();
        }

        private void editableNameValues(bool editable)
        {
            foreach (NameValue nv in listaNV)
            {
                if (nv.getTableScheme().EsModificable && editable)
                {
                    nv.Editable(true);
                }
                else
                {
                    nv.Editable(false);
                }
            }
        }

        public JObject getLineaValoresValidos(ref string error)
        {
            try
            {
                JObject resultado = new JObject();
                error = "";
                NameValue.SacarLinea(ref resultado, ref error, listaNV);

                return resultado;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public class Tags
        {
            // Auto-implemented properties.
            public string nombre { get; set; }

            public List<TableScheme> campo { get; set; }

            public Tags()
            {
            }

            public Tags(string nombre, List<TableScheme> campo)
            {
                this.nombre = nombre;
                this.campo = campo;
            }
        }

        private void SqlCommandPrepareEx(string tabla, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String sql = "select db_name() as CONSTRAINT_CATALOG "
                                + ",t_obj.name as TABLE_NAME "
                                + ",user_name(c_obj.uid) as CONSTRAINT_SCHEMA "
                                + ",c_obj.name as CONSTRAINT_NAME "
                                + " ,col.name as COLUMN_NAME, "
                                + "col.colid as ORDINAL_POSITION,"
                                + "com.text as DEFAULT_CLAUSE"
                                + " from sysobjects c_obj "
                                + "join syscomments com on c_obj.id = com.id "
                                + "join sysobjects t_obj on c_obj.parent_obj = t_obj.id "
                                + "join sysconstraints con on c_obj.id = con.constid "
                                + " join syscolumns col on t_obj.id = col.id "
                                + " and con.colid = col.colid " + " where "
                                + " c_obj.xtype = 'D' and t_obj.name='"
                                + tabla.ToUpper() + "'";

                SqlCommand command = new SqlCommand(sql, connection);

                // Create and prepare an SQL statement.
                command.CommandText = sql;
                SqlParameter paramT = new SqlParameter("@id", SqlDbType.Char, ' ');

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                SqlDataReader registros = command.ExecuteReader();

                while (registros.Read())
                {
                    NameValue nameValue = null;
                    foreach (NameValue item in listaNV)
                    {
                        if (item.Name.ToUpper() == "A." + registros["COLUMN_NAME"].ToString())
                        {
                            item.Margin = new Padding(0);
                            nameValue = item;
                            nameValue.SetValor(Regex.Replace(registros["DEFAULT_CLAUSE"].ToString(), "[^0-9A-Za-z]", "", RegexOptions.None));
                        }
                    }
                }
                //DataTable datosdefault = command.
                connection.Close();
            }
        }
    }
}