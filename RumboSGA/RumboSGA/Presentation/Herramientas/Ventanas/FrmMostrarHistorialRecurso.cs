using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FrmMostrarHistorialRecurso : Telerik.WinControls.UI.RadForm
    {
        private int idRecurso;
        public static string SentenciaSelectMovimientosRecursos = "SELECT IDMOVIMIENTO, IDOPERARIO, IDRECURSO, " +
            "IDHUECOORIGEN, IDHUECODESTINO, IDENTRADA, IDSALIDA, TIPOMOVIMIENTO, MSGERROR, PLIST, SEGUNDOS," +
            " FECHAHORA, UNIDADES, IDPEDIDO, IDPEDIDOLIN, MOVPAREJA, IDRESERVA, IDPLISTORIGEN, IDMAQUINA " +
            "FROM dbo.TBLOPERARIOSMOV";
        public FrmMostrarHistorialRecurso(int idRecurso)
        {
            InitializeComponent();
            this.idRecurso = idRecurso;
            this.Name = Lenguaje.traduce(this.Name);
        }


        private void getDatosFechaActual()
        {
            DateTime fechaInicio = this.radDateTimePickerFechaInicio.Value;
            DateTime fechaFinal = this.radDateTimePickerFechaInicio.Value;

        }
    }
}
