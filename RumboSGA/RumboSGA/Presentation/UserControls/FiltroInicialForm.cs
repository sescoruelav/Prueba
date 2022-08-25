using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation
{
    public enum Modos
    {
        VisorSql,
        BaseGridControl
    }

    public partial class FiltroInicialForm : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<TableScheme> esquema;
        Dictionary<string, string> nomEtqDict;
        Dictionary<string, string> tipos = new Dictionary<string, string>();
        string path = Persistencia.ConfigPath;
        string nombreJson;
        Modos modo;

        public FiltroInicialForm(string nombreJson, Modos modo)
        {
            InitializeComponent();
            btnGuardar.Text = Lenguaje.traduce("Guardar");
            btnCancelar.Text = Lenguaje.traduce("Cancelar");
            this.Text = Lenguaje.traduce("Filtro Inicial");
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);            

            this.nombreJson = nombreJson;
            this.modo = modo;
            CargaJSON(nombreJson);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var q = radDataFilter1.Expression;
            foreach (var pair in nomEtqDict)
            {
                q = q.Replace("[" + pair.Value + "]", pair.Key);
            }
            q = ConvertirFormatoFecha(q);
            switch (modo)
            {
                case Modos.BaseGridControl:
                    BaseGridControl.filtroInicialVolatil = q;
                    break;
                case Modos.VisorSql:
                    //TODO: Esto no funciona correctamente con VisorSQLRibbon, uno remplaza otro.
                    //VisorSQLRibbon.FiltroInicialVolatil = q;
                    break;
            }            
            this.Close();
        }

        private string ConvertirFormatoFecha(string q)
        {
            Regex rx = new Regex(@"\#([^\#]*)\#"); //captura todo lo que haya entre #
            MatchCollection mc = rx.Matches(q);
            Dictionary<string, string> fechas = new Dictionary<string, string>();
            foreach (Match match in mc)
            {
                string f = match.ToString().Trim('#').Split(' ')[0]; //coge solo la fecha, ignora la hora
                string d = f.Split('/')[1];
                string m = f.Split('/')[0];
                string y = f.Split('/')[2];
                DateTime fecha = DateTime.Parse(d + "/" + m + "/" + y);
                fechas.Add(match.ToString(), "\'" + fecha.ToString("s") + "\'");
            }
            foreach (var fecha in fechas)
            {
                q = q.Replace(fecha.Key, fecha.Value);
            }
            return q;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CargaJSON(string nombreJson)
        {
            try
            {
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);
                string jsonEsquema = js.First()["Scheme"].ToString();
                string where = js.First()["WHERE"].ToString();
                esquema = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                nomEtqDict = GenerarDiccionarioNomEtiqueta(esquema);
                GenerarListaDescriptores(esquema);
                foreach (var etq in nomEtqDict)
                {
                    where = where.Replace(etq.Key, etq.Value);
                }
                radDataFilter1.DataFilterElement.Expression = ConvertirFormatoWhereJSON(where);
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("Fallo al leer el JSON " + nombreJson));
                log.Debug("FiltroInicialForm falló al leer el JSON " + nombreJson);
            }
        }

        private static string ConvertirFormatoWhereJSON(string v)
        {
            string[] q = v.Split(new string[] { "AND", "<", ">", "=" }, StringSplitOptions.None);
            var result = string.Empty;

            foreach (var query in q)
            {
                var newQuery = string.Empty;
                var dict = new Dictionary<string, string>()
                {
                    {"@hoy", "GETDATE()"},
                };

                foreach (var item in dict)
                {
                    if (query.Contains(item.Key))
                        newQuery = query.Replace(item.Key, item.Value);
                }


                if (query.Contains("@hoy"))
                {
                    string connectionString = ConexionSQL.getConnectionString();
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "SELECT " + newQuery;
                        DateTime exec = (DateTime)cmd.ExecuteScalar();
                        newQuery = exec.Date.ToString("yyyyMMdd");
                    }
                }

                if (!string.IsNullOrEmpty(newQuery))
                    v = v.Replace(query, newQuery);
            }
            
            return v;
        }

        private void GenerarListaDescriptores(List<TableScheme> schemes)
        {
            foreach (var sch in schemes)
            {
                radDataFilter1.Descriptors.Add(new DataFilterDescriptorItem()
                {
                    DescriptorName = sch.Etiqueta,
                    DescriptorType = ObtenerTipo(sch),
                    IsAutoGenerated = true,
                });
                tipos.Add(sch.Etiqueta, ObtenerTipo(sch).Name);
            }
        }

        private Type ObtenerTipo(TableScheme tbsc)
        {
            if (tbsc.CmbObject.CampoMostrado != null) return typeof(String);

            String tipo = tbsc.Tipo;
            if (tipo.StartsWith("CHAR")) return typeof(String);
            if (tipo.StartsWith("VARCHAR")) return typeof(String);
            if (tipo.StartsWith("string")) return typeof(String);
            if (tipo.StartsWith("INT")) return typeof(Int32);
            if (tipo.StartsWith("DATE") || tipo.StartsWith("date")) return typeof(DateTime);
            else return typeof(Nullable);
        }

        private Dictionary<string, string> GenerarDiccionarioNomEtiqueta(List<TableScheme> schemes)
        {
            Dictionary<string, string> nomEtqDict = new Dictionary<string, string>();
            int contadorJoins = 0;

            foreach (var sch in schemes)
            {
                String itemNombre = Utilidades.ponerCorchetesCampo(sch.Nombre);

                if (sch.CmbObject != null && sch.CmbObject.CampoMostrado != null)
                {
                    ComboScheme c = sch.CmbObject;
                    String aliasCombo = "J" + contadorJoins;

                    nomEtqDict.Add(aliasCombo + ".[" + c.CampoMostrado + "]", sch.Etiqueta);
                    contadorJoins++;
                }
                else
                    nomEtqDict.Add(itemNombre, sch.Etiqueta);
            }

            return nomEtqDict;
        }
    }
}
