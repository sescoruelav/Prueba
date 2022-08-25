using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class PlanificacionDiariaRecepciones : Telerik.WinControls.UI.RadForm
    {
        public PlanificacionDiariaRecepciones()
        {
            InitializeComponent();
            string[] descriptions = { "Fecha Prevista", "Fecha Tope" };
            int count = 1;
            foreach (string description in descriptions)
            {
                this.radScheduler1.Resources.Add(new Resource(count++, description));
            }
            DataTable appointments = ConexionSQL.getDataTable("SELECT RP.IDRUTA, R.DESCRIPCION, RP.DIA AS FECHACARGA" +
            " FROM TBLRUTASPREPARACION RP INNER JOIN TBLRUTAS R ON RP.IDRUTA = R.IDRUTA INNER JOIN TBLPEDIDOSCLICAB PCL ON R.IDRUTA = PCL.IDRUTA");
            SchedulerBindingDataSource data = new SchedulerBindingDataSource();
            AppointmentMappingInfo appointmentMappingInfo = new AppointmentMappingInfo();
            appointmentMappingInfo.UniqueId ="RowNum";
            appointmentMappingInfo.Start = "FECHACARGA";
            appointmentMappingInfo.End = "FECHACARGA";
            appointmentMappingInfo.Description = "DESCRIPCION";
            //data.ResourceProvider.DataSource = descriptions;
            data.EventProvider.DataSource = appointments;
            data.ResourceProvider.Mapping = appointmentMappingInfo;
            radScheduler1.DataSource = data;
            foreach (DataRow item in appointments.Rows)
            {
                Debug.WriteLine(item[0].ToString()+"  "+item[1].ToString()+"   "+item[2].ToString());
            }
        }
    }
}
