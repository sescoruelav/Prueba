using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public static class SentenciasSQL
    {

        public static string queryPedidosProCab="SELECT * FROM TBLPEDIDOSPROCAB";
        public static string queryIncidenciasInventario = "SELECT * FROM TBLINCIDENCIASINVENTARIO";
        public static string queryRecepciones = "Select * from TBLPEDIDOSPROESTADO,TBLRECEPCIONESCAB";
        public static string queryJerarquia = "select IDRECEPCION, ALBARANTRANSPORTISTA, MATRICULACAMION, OBSERVACIONES from TBLRECEPCIONESCAB";
        public static string queryJerarquiaRecepciones = "select IDPEDIDOPRO,IDRECEPCION, ALBARANTRANSPORTISTA, MATRICULACAMION, OBSERVACIONES from TBLRECEPCIONESCAB";
        public static string controlTareas= @"SELECT DISTINCT TP.idtarea, PPC.REFERENCIA as referencia_Pedido_Proveedor,PCC.REFERENCIA as referencia_pedido_proveedor " +
                "FROM TBLTAREASPENDIENTES TP " +
                "LEFT JOIN TBLRESERVAS R ON TP.IDTAREA= R.IDTAREA " +
                "LEFT JOIN TBLPEDIDOSPROCAB PPC ON R.IDPEDIDOPRO = PPC.IDPEDIDOPRO " +
                "LEFT JOIN TBLPEDIDOSCLICAB PCC ON R.IDPEDIDOCLI = PCC.IDPEDIDOCLI";
        public static string queryEstadoPedido = "select * from TBLPEDIDOSPROESTADO";

        public static string queryHerramientaMovimientos = "SELECT R.*,RT.DESCRIPCION AS TIPORESERVA, A.REFERENCIA, A.DESCRIPCION, A.ATRIBUTO,  HO.DESCRIPCION AS ORIGEN, HD.DESCRIPCION AS DESTINO,RE.DESCRIPCION AS RECURSO," +
"OP.NOMBRE AS OPERARIO, PCC.REFERENCIA AS PEDIDOCLIENTE, PPC.REFERENCIA AS PEDIDOPROVEEDOR FROM TBLHUECOS AS HD INNER JOIN TBLRESERVASTIPO AS RT" +
"INNER JOIN  TBLRESERVAS AS R ON RT.IDRESERVATIPO = R.IDRESERVATIPO INNER JOIN TBLARTICULOS AS A ON R.IDARTICULO = A.IDARTICULO INNER JOIN  TBLHUECOS AS HO ON R.IDHUECOORIGEN = HO.IDHUECO ON HD.IDHUECO = R.IDHUECODESTINO" +
"LEFT OUTER JOIN TBLPEDIDOSPROCAB AS PPC ON R.IDPEDIDOPRO = PPC.IDPEDIDOPRO LEFT OUTER JOIN TBLPEDIDOSCLICAB AS PCC ON R.IDPEDIDOCLI = PCC.IDPEDIDOCLI LEFT OUTER JOIN TBLRECURSOS AS RE ON R.IDRECURSO = RE.IDRECURSO" +
"LEFT OUTER JOIN TBLOPERARIOS AS OP ON R.IDOPERARIO = OP.IDOPERARIO  WHERE R.IDENTRADA = 1 OR R.IDENTRADA = 3 OR R.IDENTRADA = 4 OR R.IDENTRADA = 5 OR R.IDENTRADA = 6 OR R.IDENTRADA = 7 OR R.IDENTRADA = 8 OR R.IDENTRADA = 9" +
"OR R.IDENTRADA = 10 OR R.IDENTRADA = 12 OR R.IDENTRADA = 13 OR R.IDENTRADA = 14 OR R.IDENTRADA = 15 OR R.IDENTRADA = 16 OR R.IDENTRADA = 23 OR R.IDENTRADA = 24 OR R.IDENTRADA = 25 OR R.IDENTRADA = 26 OR R.IDENTRADA = 28" +
"OR R.IDENTRADA = 29 OR R.IDENTRADA = 35 OR R.IDENTRADA = 36 OR R.IDENTRADA = 42 OR R.IDENTRADA = 43 OR R.IDENTRADA = 46 OR R.IDENTRADA = 47 OR R.IDENTRADA = 48 OR R.IDENTRADA = 49 OR R.IDENTRADA = 50 OR R.IDENTRADA = 51" +
"OR R.IDENTRADA = 52 OR R.IDENTRADA = 53 OR R.IDENTRADA = 56 OR R.IDENTRADA = 57 OR R.IDENTRADA = 59 OR R.IDENTRADA = 61 OR R.IDENTRADA = 62 OR R.IDENTRADA = 64 OR R.IDENTRADA = 65 OR R.IDENTRADA = 66 OR R.IDENTRADA = 67" +
"OR R.IDENTRADA = 68 OR R.IDENTRADA = 69 OR R.IDENTRADA = 70 OR R.IDENTRADA = 71 OR R.IDENTRADA = 72 OR R.IDENTRADA = 74 OR R.IDENTRADA = 75 OR R.IDENTRADA = 76 OR R.IDENTRADA = 77 OR R.IDENTRADA = 78 OR R.IDENTRADA = 79" +
"OR R.IDENTRADA = 80 OR R.IDENTRADA = 81 OR R.IDENTRADA = 82 OR R.IDENTRADA = 84 OR R.IDENTRADA = 85 OR R.IDENTRADA = 86 OR R.IDENTRADA = 88 OR R.IDENTRADA = 89 OR R.IDENTRADA = 90 OR R.IDENTRADA = 91 OR R.IDENTRADA = 92" +
"OR R.IDENTRADA = 93 OR R.IDENTRADA = 94 OR R.IDENTRADA = 95 OR R.IDENTRADA = 96 OR R.IDENTRADA = 97 OR R.IDENTRADA = 98 OR R.IDENTRADA = 99 OR R.IDENTRADA = 100 OR R.IDENTRADA = 101";

        public static string queryReposicionesActivadoZona = "SELECT a.idarticulo as idarticulo,a.referencia, a.descripcion,h.descripcion as hpicking,a.stockMinReab  as stockMinReab,coalesce(exPI.cantidad,0) as ExistenciasPI, " +
            "coalesce(exUB.cantidad,0) as ExistenciasUB,coalesce(vrPI.cantidad,0) as ReservasPI,coalesce(vrRP.cantidad,0) as ReservasRP,coalesce(exPI.cantidad,0)+coalesce(vrRP.cantidad,0)-coalesce(vrPI.cantidad,0) as Total " +
            "from TBLARTICULOS a left join VEXISTENCIASPIZONAPICKING exPI on a.idarticulo=exPI.idarticulo left join tblhuecos h on a.idhuecopicking=h.idhueco "
                + " left join VEXISTENCIASUBFUERAZONAPICKING exUB on a.idarticulo=exUB.idarticulo left join VRESERVACANTIDADPI vrPI on a.idarticulo=vrPI.idarticulo "
                + " left join VRESERVACANTIDADRP vrRP on a.idarticulo=vrRP.idarticulo where a.idhuecopicking>0 order by a.idarticulo";

        public static string queryReposicionesNoActivado  = "SELECT a.idarticulo as idarticulo,a.referencia, a.descripcion,h.descripcion as hpicking,a.stockMinReab  as stockMinReab,coalesce(exPI.cantidad,0) as ExistenciasPI, " +
           "coalesce(exUB.cantidad,0) as ExistenciasUB,coalesce(vrPI.cantidad,0) as ReservasPI,coalesce(vrRP.cantidad,0) as ReservasRP,coalesce(exPI.cantidad,0)+coalesce(vrRP.cantidad,0)-coalesce(vrPI.cantidad,0) as Total " +
           "from TBLARTICULOS a left join VEXISTENCIASPI exPI on a.idarticulo=exPI.idarticulo left join TBLHUECOS h on a.idhuecopicking= h.idhueco  left join VEXISTENCIASUB exUB on a.idarticulo= exUB.idarticulo " +
           "left join VRESERVACANTIDADPI vrPI on a.idarticulo= vrPI.idarticulo  left join VRESERVACANTIDADRP vrRP on a.idarticulo= vrRP.idarticulo  where a.idhuecopicking>0 order by a.idarticulo";

        public static string querySoloConflictosNoActivadoPorZona = "SELECT a.idarticulo as idarticulo,a.referencia, a.descripcion,h.descripcion as hpicking,a.stockMinReab  as stockMinReab,coalesce(exPI.cantidad,0) as ExistenciasPI, " +
            "coalesce(exUB.cantidad,0) as ExistenciasUB,coalesce(vrPI.cantidad,0) as ReservasPI,coalesce(vrRP.cantidad,0) as ReservasRP,coalesce(exPI.cantidad,0)+coalesce(vrRP.cantidad,0)-coalesce(vrPI.cantidad,0) as Total " +
            "from TBLARTICULOS a left join VEXISTENCIASPI exPI on a.idarticulo=exPI.idarticulo left join TBLHUECOS h on a.idhuecopicking= h.idhueco  left join VEXISTENCIASUB exUB on a.idarticulo= exUB.idarticulo " +
            "left join VRESERVACANTIDADPI vrPI on a.idarticulo= vrPI.idarticulo  left join VRESERVACANTIDADRP vrRP on a.idarticulo= vrRP.idarticulo" +
            " where a.idhuecopicking>0 and coalesce(exPI.cantidad,0)+coalesce(vrRP.cantidad,0)-coalesce(vrPI.cantidad,0)<a.stockMinReab and  coalesce(exUB.cantidad,0)>a.stockminreab ";
        public static string querySoloConflictosActivadoPorZona = "SELECT a.idarticulo as idarticulo,a.referencia, a.descripcion,h.descripcion as hpicking,a.stockMinReab  as stockMinReab,coalesce(exPI.cantidad,0) as ExistenciasPI, " +
            "coalesce(exUB.cantidad,0) as ExistenciasUB,coalesce(vrPI.cantidad,0) as ReservasPI,coalesce(vrRP.cantidad,0) as ReservasRP,coalesce(exPI.cantidad,0)+coalesce(vrRP.cantidad,0)-coalesce(vrPI.cantidad,0) as Total " +
            "from tblarticulos a left join VEXISTENCIASPIZONAPICKING exPI on a.idarticulo=exPI.idarticulo  left join tblhuecos h on a.idhuecopicking=h.idhueco "
                + " left join VEXISTENCIASUBFUERAZONAPICKING exUB on a.idarticulo=exUB.idarticulo  left join VRESERVACANTIDADPI vrPI on a.idarticulo=vrPI.idarticulo "
                + " left join VRESERVACANTIDADRP vrRP on a.idarticulo=vrRP.idarticulo" +" where a.idhuecopicking>0 and coalesce(exPI.cantidad,0)+coalesce(vrRP.cantidad,0)-coalesce(vrPI.cantidad,0)<a.stockMinReab and  coalesce(exUB.cantidad,0)>a.stockminreab ";

    }



}
