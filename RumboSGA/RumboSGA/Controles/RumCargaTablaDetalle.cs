using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using static RumboSGA.Controles.SFDetalle;

namespace RumboSGA.Controles
{
    internal class RumCargaTablaDetalle
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<NameValue> listaNV = new List<NameValue>();
        public bool ComprobarValores = false;
        public bool editado = false;
        public dynamic newRecord;
        private RumButton rumButtonAceptar = new RumButton();
        private RumButton rumButtonCancelar = new RumButton();
        private RumButton rumButtonEditar = new RumButton();
        private System.Windows.Forms.Panel panelCabezera;
        private Panel panelDatos;
        private RumLabel rumLabelTitulo = new RumLabel();
        private List<TableScheme> lstEsquemaTabla;

        public Panel CargaTablaDetalle(string nombre, List<TableScheme> lstEsquemaTabla, dynamic selectedRow, bool nuevo, bool clonar, string tablaPrincipal, int Width)
        {
            controlesBotones();

            lstEsquemaTabla.Select(tab => tab.Tab);
            List<Tags> listaTags = new List<Tags>();
            JObject lineaSeleccionada = null;
            String valor;
            if (!nuevo)
            {
                try
                {
                    lineaSeleccionada = JsonConvert.DeserializeObject<JObject>(selectedRow);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarErrorNuevo(ex, "Error en cargarVaores en selectedRow:" + selectedRow);
                }
            }
            else
            {
                rumButtonEditar.Visible = false;
            }

            foreach (TableScheme ts in lstEsquemaTabla)
            {
                if (ts.Tipo == "varchar")
                {
                }
                NameValue nv = new NameValue(ts, "");
                nv.Name = ts.Nombre;
                listaNV.Add(nv);
            }
            foreach (NameValue nv in listaNV)
            {
                try
                {
                    if (!nuevo)
                    {
                        valor = lineaSeleccionada.GetValue(nv.Name).ToString();
                        nv.SetValor(valor);
                        log.Info("Valor:" + valor);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarErrorNuevo(ex, "Error en cargarVaores en NameValue: " + nv.Name);
                }
            }

            foreach (var item in lstEsquemaTabla.GroupBy(t => t.Tab).Where(t => t.Count() != 1))
            {
                Tags tag = new Tags();
                tag.nombre = item.Key;
                tag.campo = new List<TableScheme>();
                listaTags.Add(tag);
                log.Info(item.Key);
            }
            int conta = 0;
            foreach (var item in listaTags)
            {
                foreach (var item2 in lstEsquemaTabla)
                {
                    if (item2.Tab == item.nombre)
                    {
                        item.campo.Add(item2);
                        log.Info(item.nombre + " : " + item2.Etiqueta + " " + conta++);
                    }
                }
            }
            listaTags.Reverse();
            editableNameValues(false);

            foreach (var tags in listaTags)
            {
                RadCollapsiblePanel collapsePanel = new RadCollapsiblePanel();

                collapsePanel.CollapsiblePanelElement.Text = tags.nombre;
                collapsePanel.CollapsiblePanelElement.HeaderText = tags.nombre;
                collapsePanel.Dock = DockStyle.Top;
                TableLayoutPanel tabla = new TableLayoutPanel();
                tabla.AutoSize = true;
                tabla.ColumnCount = 4;
                tabla.Dock = DockStyle.Fill;

                for (int i = 0; i < tabla.ColumnCount; i++)
                {
                    tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, (Width) - 200 / tabla.ColumnCount));
                }
                tabla.AutoScroll = true;

                foreach (var campo in tags.campo)
                {
                    NameValue nameValue = null;
                    foreach (NameValue item in listaNV)
                    {
                        if (item.Name == campo.Nombre)
                        {
                            item.Margin = new Padding(0);
                            nameValue = item;
                            if (clonar)
                            {
                                if (item.Name == "A.idarticulo")
                                {
                                    nameValue.SetValor(" ");
                                }
                                rumButtonEditar.Visible = false;
                                editableNameValues(true);
                            }
                        }
                    }
                    tabla.Controls.Add(nameValue);
                }

                if (nuevo)
                {
                    editableNameValues(true);
                    SqlCommandPrepareEx(tablaPrincipal.ToUpper(), ConexionSQL.getConnectionString());
                    rumButtonEditar.Visible = false;
                }
                collapsePanel.Height = tabla.PreferredSize.Height + 40;
                collapsePanel.Width = tabla.PreferredSize.Width + 50;
                collapsePanel.CollapsiblePanelElement.DrawBorder = false;
                collapsePanel.CollapsiblePanelElement.EnableBorderHighlight = false;
                collapsePanel.EnableAnimation = false;
                if (collapsePanel.CollapsiblePanelElement.Text == "General")
                {
                    collapsePanel.CollapsiblePanelElement.HeaderElement.HeaderButtonElement.Enabled = false;
                }
                tabla.Margin = new Padding(0);
                tabla.Padding = new Padding(0);

                collapsePanel.Controls.Add(tabla);
                panelDatos.Width = collapsePanel.Width;
                panelDatos.Controls.Add(collapsePanel);
            }
            panelDatos.AutoScroll = true;

            return panelDatos;
        }

        private void controlesBotones()
        {
            //
            // panelDatos
            //
            this.panelDatos = new System.Windows.Forms.Panel();

            panelDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            panelDatos.Name = "panelDatos";
            panelDatos.TabIndex = 1;
            panelDatos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            panelDatos.AutoScroll = true;
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