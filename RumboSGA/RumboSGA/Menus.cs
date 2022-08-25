using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGA
{
    public class Menus
    {
        private Dictionary<string, List<string>> menus;
        private Dictionary<string, List<string>> menuHerramientas;
        private Dictionary<string, List<string>> menuInventario;
        private Dictionary<string, List<string>> menuMaestros;
        private Dictionary<string, List<string>> menuClientes;
        private Dictionary<string, List<string>> menuProveedores;
        private Dictionary<string, List<string>> menuHistorico;
        private Dictionary<string, List<string>> menuConfiguracion;

        public Menus()
        {
            menus = new Dictionary<string, List<string>>();
            RellenarMenus();
            menuHerramientas = new Dictionary<string, List<string>>();
            RellenarMenuHerramientas();
            menuHistorico = new Dictionary<string, List<string>>();
            RellenarMenuHistorico();
            menuMaestros = new Dictionary<string, List<string>>();
            RellenarMenuMaestros();
            menuClientes = new Dictionary<string, List<string>>();
            RellenarMenuClientes();
            menuProveedores = new Dictionary<string, List<string>>();
            RellenarMenuProveedores();
            menuInventario = new Dictionary<string, List<string>>();
            RellenarMenuInventario();
            menuConfiguracion = new Dictionary<string, List<string>>();
            RellenarMenuConfiguracion();
        }

        public void RellenarMenus()
        {
            menus.Add("0", new List<string> { "herramientasNode", "false", Lenguaje.traduce("Herramientas"), "(ninguno)", "true" });
            menus.Add("1", new List<string> { "maestrosNode", "false", Lenguaje.traduce("Maestros"), "(ninguno)", "true" });
            menus.Add("2", new List<string> { "historicosNode", "false", Lenguaje.traduce("Históricos"), "(ninguno)", "true" });
            menus.Add("3", new List<string> { "confNode", "false", Lenguaje.traduce("Configuración"), "(ninguno)", "true" });
            menus.Add("4", new List<string> { "acerca", "false", Lenguaje.traduce("Acerca de"), "(ninguno)", "true" });
        }

        public void RellenarMenuHerramientas()
        {
            menuHerramientas.Add("0", new List<string> { "planificacion", "false", Lenguaje.traduce("Planificación"), "(ninguno)", "false", "20074" });
            menuHerramientas.Add("1", new List<string> { "proveedoresPedidosNode", "false", Lenguaje.traduce("Recepciones"), "(ninguno)", "true", "20046" });
            menuHerramientas.Add("2", new List<string> { "devolProveedor", "false", Lenguaje.traduce("Devoluciones Proveedor"), "(ninguno)", "true", "30006" });
            menuHerramientas.Add("3", new List<string> { "devolCliente", "false", Lenguaje.traduce("Devoluciones de Cliente"), "(ninguno)", "true", "20065" });
            menuHerramientas.Add("4", new List<string> { "ordenesRecogidaNode", "false", Lenguaje.traduce("Expediciones"), "(ninguno)", "true", "20023" });
            //menuHerramientas.Add("5", new List<string> { "movimientosNode", "false", Lenguaje.traduce("Movimientos"), "(ninguno)", "true", "20017" });
            menuHerramientas.Add("6", new List<string> { "stockNode", "false", Lenguaje.traduce("Stock"), "(ninguno)", "true", "20060" });
            menuHerramientas.Add("7", new List<string> { "controlTareasNode", "false", Lenguaje.traduce("Control de Tareas"), "(ninguno)", "true", "20091" });
            menuHerramientas.Add("8", new List<string> { "acopiosProduccionNode", "false", Lenguaje.traduce("Produccion"), "(ninguno)", "true", "20068" });
            menuHerramientas.Add("9", new List<string> { "trazabilidadNode", "false", Lenguaje.traduce("Trazabilidad"), "(ninguno)", "true", "40005" });
            menuHerramientas.Add("10", new List<string> { "reposiciones", "false", Lenguaje.traduce("Reposiciones"), "(ninguno)", "true", "20067" });
            //menuHerramientas.Add("11", new List<string> { "compactadorApilamientosNode", "false", Lenguaje.traduce("Compilador Apilamientos"), "(ninguno)", "true", "30017" });
            menuHerramientas.Add("11", new List<string> { "productividadNode", "false", Lenguaje.traduce("Productividad"), "(ninguno)", "true", "20083" });
            menuHerramientas.Add("12", new List<string> { "menuInventario", "false", Lenguaje.traduce("Inventario"), "(ninguno)", "true" });
            menuHerramientas.Add("13", new List<string> { "informesNode", "false", Lenguaje.traduce("Informes"), "(ninguno)", "true", "20076" });
        }

        public void RellenarMenuInventario()
        {
            menuInventario.Add("0", new List<string> { "inventarioInicialNode", "false", Lenguaje.traduce("Inventario Inicial"), "(ninguno)", "true", "500034" });
            menuInventario.Add("1", new List<string> { "inventarioContinuoNode", "false", Lenguaje.traduce("Inventario Continuo"), "(ninguno)", "true" });
            menuInventario.Add("2", new List<string> { "incidenciasInventarioNode", "false", Lenguaje.traduce("Incidencias Inventario"), "(ninguno)", "true", "20081" });
            //Comentado por césar: no hemos encontrado ninguna referencia y no lleva a nada
            //menuInventario.Add("3", new List<string> { "limpiarIncidenciasNode", "false", "Limpiar incidencias", "(ninguno)","true", "20081" });
        }

        public void RellenarMenuMaestros()
        {
            menuMaestros.Add("0", new List<string> { "clientesMainNode", "false", Lenguaje.traduce("Clientes"), "(ninguno)", "true" });
            menuMaestros.Add("1", new List<string> { "proveedoresNodeMain", "false", Lenguaje.traduce("Proveedores"), "(ninguno)", "true" });
            menuMaestros.Add("4", new List<string> { "maquinasNode", "false", Lenguaje.traduce("Maquinas"), "(ninguno)", "true", "20070" });
            menuMaestros.Add("5", new List<string> { "articulosNode", "false", Lenguaje.traduce("Artículos"), "(ninguno)", "true", "20004" });
            menuMaestros.Add("6", new List<string> { "familiasNode", "false", Lenguaje.traduce("Familias"), "(ninguno)", "true", "20052" });
            menuMaestros.Add("7", new List<string> { "agenciasNode", "false", Lenguaje.traduce("Agencias"), "(ninguno)", "true", "20000" });
            menuMaestros.Add("8", new List<string> { "bomNode", "false", Lenguaje.traduce("Bom"), "(ninguno)", "true", "20006" });
            menuMaestros.Add("9", new List<string> { "zonaIntercambioNode", "false", Lenguaje.traduce("Zona Intercambio"), "(ninguno)", "true", "20097" });
            menuMaestros.Add("10", new List<string> { "motivosRegularizacionNode", "false", Lenguaje.traduce("Motivos Regularización"), "(ninguno)", "true", "20016" });
            menuMaestros.Add("11", new List<string> { "rutasNode", "false", Lenguaje.traduce("Rutas"), "(ninguno)", "true", "20075" });
            menuMaestros.Add("12", new List<string> { "rutasPreparacionNode", "false", Lenguaje.traduce("Rutas Preparacion"), "(ninguno)", "true", "20080" });
            menuMaestros.Add("13", new List<string> { "tareasPendientesNode", "false", Lenguaje.traduce("Tareas Pendientes"), "(ninguno)", "true", "20062" });
            menuMaestros.Add("14", new List<string> { "tareasTipoNode", "false", Lenguaje.traduce("Tareas Tipo"), "(ninguno)", "true", "20061" });
            menuMaestros.Add("15", new List<string> { "combiPaletsNode", "false", Lenguaje.traduce("Combi Palet"), "(ninguno)", "true", "20085" });
            menuMaestros.Add("16", new List<string> { "paletsTipoNode", "false", Lenguaje.traduce("Palets Tipo"), "(ninguno)", "true", "20020" });
            menuMaestros.Add("17", new List<string> { "formatosNode", "false", Lenguaje.traduce("Formatos SSCC"), "(ninguno)", "true", "20102" });
            menuMaestros.Add("18", new List<string> { "lotesNode", "false", Lenguaje.traduce("Lotes"), "(ninguno)", "true", "20092" });
            menuMaestros.Add("19", new List<string> { "propietariosNode", "false", Lenguaje.traduce("Propietario"), "(ninguno)", "true", "30041" });
            menuMaestros.Add("20", new List<string> { "estadoFabNode", "false", Lenguaje.traduce("Estado Fabricacion"), "(ninguno)", "true", "40007" });
            menuMaestros.Add("21", new List<string> { "estadoMaqNode", "false", Lenguaje.traduce("Estado Maquina"), "(ninguno)", "true", "40008" });
            menuMaestros.Add("22", new List<string> { "estadoExistNode", "false", Lenguaje.traduce("Estado Existencias"), "(ninguno)", "true", "40009" });
            menuMaestros.Add("23", new List<string> { "ordenFabricacion", "false", Lenguaje.traduce("Orden Fabricacion"), "(ninguno)", "true", "20063" });
            menuMaestros.Add("24", new List<string> { "EANProveedor", "false", Lenguaje.traduce("Ean Proveedor"), "(ninguno)", "true", "20002" });
            menuMaestros.Add("25", new List<string> { "transitoLinNode", "false", Lenguaje.traduce("Transito Lineas"), "(ninguno)", "true", "20099" });
            menuMaestros.Add("26", new List<string> { "presentacionesNode", "false", Lenguaje.traduce("Presentaciones"), "(ninguno)", "true", "30067" });
            menuMaestros.Add("27", new List<string> { "carroMovilCabNode", "false", Lenguaje.traduce("Carro Movil"), "(ninguno)", "true", "30035" });
            menuMaestros.Add("28", new List<string> { "clasificacionEntradaNode", "false", Lenguaje.traduce("Clasificacion Entradas"), "(ninguno)", "true", "30401" });
            menuMaestros.Add("29", new List<string> { "SubsistemasNode", "false", Lenguaje.traduce("Subsistemas"), "(ninguno)", "true", "30402" });
            menuMaestros.Add("30", new List<string> { "TipoSoporteNode", "false", Lenguaje.traduce("Tipos Soporte"), "(ninguno)", "true", "30403" });
        }

        public void RellenarMenuClientes()
        {
            menuClientes.Add("0", new List<string> { "devolucionesClienteMantenimientoNode", "false", Lenguaje.traduce("Devoluciones Cliente"), "(ninguno)", "true", "20065" });
            menuClientes.Add("1", new List<string> { "clientesNode", "false", Lenguaje.traduce("Clientes"), "(ninguno)", "true", "20007" });
            menuClientes.Add("2", new List<string> { "clientesPedidosNuevoNode", "false", Lenguaje.traduce("Pedidos"), "(ninguno)", "true", "20023" });
            menuClientes.Add("3", new List<string> { "recepcionesHistoricoNode", "false", Lenguaje.traduce("Historico Recepciones"), "(ninguno)", "true", "30030" });
            menuClientes.Add("4", new List<string> { "estadoPedidoClienteNode", "false", Lenguaje.traduce("Estado Pedidos Cliente"), "(ninguno)", "true", "20044" });
            menuClientes.Add("5", new List<string> { "ordenesRecogidaMantenimientoNode", "false", Lenguaje.traduce("Mantenimiento Expediciones"), "(ninguno)", "true", "30061" });
        }

        public void RellenarMenuProveedores()
        {
            menuProveedores.Add("0", new List<string> { "proveedoresNode", "false", Lenguaje.traduce("Proveedores"), "(ninguno)", "true", "20025" });
            menuProveedores.Add("1", new List<string> { "estadoPedidoNode", "false", Lenguaje.traduce("Estado Pedidos"), "(ninguno)", "true", "40006" });
            menuProveedores.Add("2", new List<string> { "devolucionesProveedorMantenimientoNode", "false", Lenguaje.traduce("Devoluciones Proveedor"), "(ninguno)", "true", "30006" });
            menuProveedores.Add("3", new List<string> { "pedidosProveedorMantenimientoNode", "false", Lenguaje.traduce("Pedidos Proveedor"), "(ninguno)", "true", "20046" });
            menuProveedores.Add("4", new List<string> { "recepcionesHistoricoNode", "false", Lenguaje.traduce("Historico Recepciones"), "(ninguno)", "true", "30030" });
        }

        public void RellenarMenuHistorico()
        {
            menuHistorico.Add("0", new List<string> { "existenciasHistoricoNode", "false", Lenguaje.traduce("Existencias Histórico"), "(ninguno)", "true", "30019" });
            menuHistorico.Add("1", new List<string> { "reservaHistoricoNode", "false", Lenguaje.traduce("Reservas Histórico"), "(ninguno)", "true", "30018" });
            menuHistorico.Add("2", new List<string> { "stockStatusNode", "false", Lenguaje.traduce("Estado Stock"), "(ninguno)", "true", "20035" });
            menuHistorico.Add("3", new List<string> { "operariosMovNode", "false", Lenguaje.traduce("Histórico Movimientos"), "(ninguno)", "true", "20083" });
            menuHistorico.Add("4", new List<string> { "visorLogNode", "false", Lenguaje.traduce("Visor Log"), "(ninguno)", "true", "30024" });
            menuHistorico.Add("5", new List<string> { "movimientosEmbalajeNode", "false", Lenguaje.traduce("Movimientos Embalaje"), "(ninguno)", "true", "30038" });
            menuHistorico.Add("6", new List<string> { "packingListNode", "false", Lenguaje.traduce("Packing List"), "(ninguno)", "true", "30033" });
        }

        public void RellenarMenuConfiguracion()
        {
            menuConfiguracion.Add("0", new List<string> { "usuariosNode", "false", Lenguaje.traduce("Usuarios"), "(ninguno)", "true", "20041" });
            menuConfiguracion.Add("1", new List<string> { "usuariosGruposNode", "false", Lenguaje.traduce("Grupo Usuario"), "(ninguno)", "true", "20040" });
            menuConfiguracion.Add("2", new List<string> { "permisosNode", "false", Lenguaje.traduce("Permisos"), "(ninguno)", "true", "20026" });
            menuConfiguracion.Add("3", new List<string> { "operarioNode", "false", Lenguaje.traduce("Operario"), "(ninguno)", "true", "20018" });
            menuConfiguracion.Add("4", new List<string> { "recursosTarea", "false", Lenguaje.traduce("Recursos Tarea"), "(ninguno)", "true", "20051" });
            //Tomás recursos tarea viejo ha sido sustituido por el nuevo
            menuConfiguracion.Add("5", new List<string> { "grupo", "false", Lenguaje.traduce("Grupo"), "(ninguno)", "true", "20013" });
            menuConfiguracion.Add("6", new List<string> { "rumMantenimientoNode", "false", Lenguaje.traduce("Mantenimiento"), "(ninguno)", "true", "20089" });
            menuConfiguracion.Add("7", new List<string> { "zonaCabNode", "false", Lenguaje.traduce("Zona"), "(ninguno)", "true", "20042" });
            menuConfiguracion.Add("8", new List<string> { "mapaAlmacenNode", "false", Lenguaje.traduce("Mapa Almacén"), "(ninguno)", "true", "20048" });
            menuConfiguracion.Add("9", new List<string> { "ubicaciones", "false", Lenguaje.traduce("Ubicaciones"), "(ninguno)", "true", "20038" });
            menuConfiguracion.Add("10", new List<string> { "sentenciasNode", "false", Lenguaje.traduce("Sentencias"), "(ninguno)", "true", "30048" });
            menuConfiguracion.Add("11", new List<string> { "algoritmosNode", "false", Lenguaje.traduce("Algoritmos"), "(ninguno)", "true", "30046" });
            menuConfiguracion.Add("12", new List<string> { "etiquetasNode", "false", Lenguaje.traduce("Etiquetas"), "(ninguno)", "true", "20050" });
            menuConfiguracion.Add("13", new List<string> { "recursos", "false", Lenguaje.traduce("Recursos"), "(ninguno)", "false", "20051" });
            menuConfiguracion.Add("14", new List<string> { "parametrosNode", "false", Lenguaje.traduce("Parametros"), "(ninguno)", "true", "20021" });
        }

        public Dictionary<string, List<string>> getMenuHistorico()
        {
            return this.menuHistorico;
        }

        public Dictionary<string, List<string>> getMenus()
        {
            return this.menus;
        }

        public Dictionary<string, List<string>> getMenuHerramientas()
        {
            return this.menuHerramientas;
        }

        public Dictionary<string, List<string>> getMenuMaestros()
        {
            return this.menuMaestros;
        }

        public Dictionary<string, List<string>> getMenuClientes()
        {
            return this.menuClientes;
        }

        public Dictionary<string, List<string>> getMenuProveedores()
        {
            return this.menuProveedores;
        }

        public Dictionary<string, List<string>> getMenuInventario()
        {
            return this.menuInventario;
        }

        public Dictionary<string, List<string>> getMenuConfiguracion()
        {
            return this.menuConfiguracion;
        }
    }
}