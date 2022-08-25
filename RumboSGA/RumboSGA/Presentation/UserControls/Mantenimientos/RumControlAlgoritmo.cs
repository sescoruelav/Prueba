using RumboSGA.GestionAlmacen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public class RumControlAlgoritmo : RumControlGeneral
    {
        public RumControlAlgoritmo(String nombreJson):base(nombreJson)
        {
            nuevaColumna();
        }

        private void nuevaColumna()
        {

            GridViewCommandColumn commandColumn = new GridViewCommandColumn();
            commandColumn.Name = "visualizarColumn";
            commandColumn.UseDefaultText = true;
            commandColumn.DefaultText = "Visualizar";
            commandColumn.FieldName = "Visualizar";
            GridView.MasterTemplate.Columns.Add(commandColumn);
            GridView.CommandCellClick += new CommandCellClickEventHandler(radGridView1_CommandCellClick);
            
        }
        private void radGridView1_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            var idalgorimo= GridView.Rows[e.Row.Index].Cells[GridView.Columns.IndexOf("ID Algoritmo")].Value.ToString();
            new rAlgoritmo(int.Parse(idalgorimo));
        }



    }
}
