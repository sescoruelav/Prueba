using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{

    public partial class RumTreeView : RadTreeView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       
        private int _arbol { get; set; }
        private List<ArbolLin> listArbollin { get; set; }
        public int NivelMaximo { get => _nivelMaximo; set => _nivelMaximo = value; }

        int _nivelInicial = 1;
        int _nivelMaximo;
        private List<RadTreeNode> _nodes = new List<RadTreeNode>();
        private Hashtable valores = new Hashtable();
        private string _prefijoImagen=null;
        private bool _conCheckButton = false;
       

        public RumTreeView(int idarbol) : base()
        {
            
            this._arbol = idarbol;

          
            cargarModeloArbol();
           
        }
        public override string ThemeClassName
        {
            get
            {
                return typeof(RadTreeView).FullName;
            }
        }


        public RumTreeView(int idarbol,String prefijoImagen,  bool conCheckButton=false)
        {
            this._arbol = idarbol;
            this._prefijoImagen = prefijoImagen;
            this._conCheckButton = conCheckButton;
           
            cargarModeloArbol();

        }


       
        
        private void cargarModeloArbol()
        {
            try
            {
                DataTable arbolLin = ConexionSQL.getDataTable("SELECT * FROM RUMARBOLLIN WHERE ARBOL=" + _arbol);
                listArbollin = DataAccess.ConvertDataTable<ArbolLin>(arbolLin);
                NivelMaximo = listArbollin.Count();
                cargaNodosArbol(null, _nivelInicial);


            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void cargaNodosArbol(RumTreeNode nodoPadre, int nivel)
        {
            string query = listArbollin[nivel - 1].TSQL;
            if (nodoPadre != null)
            {
                query = sustituirValores(nodoPadre.valores, query);
            }
            DataTable dt = ConexionSQL.getDataTable(query);
            foreach (DataRow row in dt.Rows)
            {
                RumTreeNode node = new RumTreeNode();

                if (_prefijoImagen != null)
                {
                    node.Image = (Image)Resources.ResourceManager.GetObject(_prefijoImagen + nivel);
                }
                string cadenaTexto = "";
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    cadenaTexto = cadenaTexto + " " + row[i].ToString();
                }
                node.Text = cadenaTexto;
                node.Tag = "[" + _arbol + "." + nivel + "." + dt.Columns[0].ColumnName + "]";
                node.Value = nivel;
                if (_conCheckButton) { 
                     node.CheckType = CheckType.CheckBox;
                    
                }
                if (node.valores == null)
                {
                    node.valores = new Hashtable();
                    if (nodoPadre != null)
                    {
                        Hashtable h = Utilidades.Merger(node.valores, nodoPadre.valores);
                        node.valores = h;
                    }

                }
                node.valores.Add(node.Tag, row[0]);
                if (nodoPadre != null)
                {
                    nodoPadre.Nodes.Add(node);
                }
                else
                {
                    Nodes.Add(node);
                }
                if (nivel < NivelMaximo)
                {
                    cargaNodosArbol(node, nivel + 1);
                }
            }
        }

        private string sustituirValores(Hashtable valores, string tSQL)
        {
            try
            {
                foreach (string clave in valores.Keys)
                {
                    tSQL = tSQL.Replace(clave, valores[clave].ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return tSQL;
        }
        public void updateRumTreeView()
        {
            this.Nodes.Clear();
            cargaNodosArbol(null, _nivelInicial);
        }

        public List<int> getIdentificadoresSelected()
        {
            List<int> lIds = new List<int>();
            foreach (RumTreeNode nodo in Nodes)
            {
               
                    RadTreeNodeCollection hijos = nodo.Nodes;
                    foreach(RumTreeNode nodoHijo in hijos) { 
                        List<int> lista = getIdentificadores(nodoHijo);
                        lIds.AddRange(lista);
                    }
               
            }

            return lIds;
        }

       

        private List<int> getIdentificadores(RumTreeNode nodo)
        {
            List<int> lIds = new List<int>();
            if (Convert.ToInt32(nodo.Value) < NivelMaximo)
            {
                RadTreeNodeCollection hijos = nodo.Nodes;
                foreach (RumTreeNode nodoHijo in hijos)
                {
                  
                        List<int> lista = getIdentificadores(nodoHijo);
                        lIds.AddRange(lista);
                   

                }
            }
            else
            {
                if (nodo.Checked)
                {
                    Hashtable v = nodo.valores;
                    foreach (string clave in v.Keys)
                    {
                        if (clave.Equals(nodo.Tag))
                        {
                            String valor = v[clave].ToString();
                            lIds.Add(Convert.ToInt32(valor));
                        }
                    }
                }
            }
            return lIds;

        }
    }


}
