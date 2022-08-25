using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RumboSGA.Presentation.UserControls;
using System.Data.SqlClient;

namespace RumboSGA.Presentation.Pruebas
{
    public partial class pruebaTareasPendientes : BaseGridControl
    {
        string queryInicial = "Select * from tblTareasPendientes";
        string queryProveedores = "Select nombre from TBLPROVEEDORES where IDPROVEEDOR=@ID";
        public pruebaTareasPendientes()
        {
            InitializeComponent();

            
        }
        public void conexion()
        {
            using (SqlConnection con=new SqlConnection())
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(queryInicial);
                //cmd.Parameters["@ID"].Value =;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
            }
        }
    }
}
