using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Corp.Entidades.Clases;
using Corp.Entidades.Clases.Transaccion;
using Corp.Datos.Clases;
using System.Data.SqlClient;

namespace Corp.Negocio
{
    public class GestorMantenedores
    {
        #region Parametros

        private eProducto Producto;
        private eConcepto Concepto;
        private eItem Item;

        #endregion

        #region Constructor

        public GestorMantenedores() {}

        #endregion

        #region Concepto


        public DataTable Lista_Concepto(int Entidad)
        {
            DataTable conceptos = new DataTable();
            conceptos = InstanceOF.Obtener_Tabla(string.Format(@"select id, EntidadId, Descripcion, FlagNivel, FlagTipoControl 
                                                                 from Concepto 
                                                                 where EntidadId = "+ Entidad +@"
                                                                 order by FlagTipoControl ASC, Descripcion"));
            return conceptos;
        }

        public void Concepto_Almacenar(string Descripcion, int Control, int Plazo)
        {
            Concepto = new eConcepto();                        
            Concepto._descripcion = Descripcion;
            Concepto._flagnivel = Plazo;
            Concepto._flagtipocontrol = Control;
            Concepto.Concepto_Grabar();
        }

        public void Concepto_Modificar(int id, string Descripcion, int Control, int Plazo)
        {
            Concepto = new eConcepto();
            Concepto._id = id;            
            Concepto._descripcion = Descripcion;
            Concepto._flagnivel = Plazo;
            Concepto._flagtipocontrol = Control;
            Concepto.Concepto_Modificar();
        }

        public void Concepto_Eliminar(int id)
        {
            Concepto = new eConcepto();
            Concepto._id = id;
            Concepto.Concepto_Eliminar();
        }
        #endregion

        #region Item

        public DataTable ListarItem(int Entidad)
        {
            DataTable items = new DataTable();
            items = InstanceOF.Obtener_Tabla(string.Format(@"select item.id, item.Descripcion as Item, ConceptoId, concepto.Descripcion as Concepto
                                                             from item inner join concepto on item.ConceptoId = Concepto.Id
                                                             where EntidadId = "+ Entidad));
            return items;
        }

        public int GuardarItem(string Descripcion, int ConceptoId, int EntidadId)
        {
            cf_Item item = new cf_Item();
            item.Descripcion = Descripcion;
            item.ConceptoId  = ConceptoId;
            item.PlazoMinimo = 0;
            item.PlazoMaximo = 0;
            return InstanceOF.Establecer_Objeto(item, typeof(cf_Item), new List<string>(new string[] { "Id" }), InstanceOF.PROCESO_SP.MN_INSERT_ITEM);            
        }

        public int ModificarItem(int id, string Descripcion)
        {
            cf_Item item = new cf_Item();
            item.Descripcion = Descripcion;
            return InstanceOF.Establecer_Objeto(item, typeof(cf_Item), new List<string>(new string[] { "" }), InstanceOF.PROCESO_SP.MN_UPDATE_ITEM);            
        }

        public int EliminarItem(int id)
        {
            return InstanceOF.Eliminar_Registro("delete from Item where id =" + id);
        }
        
        public int Item_Modificar(int id, string descripcion)
        {
            Item = new eItem();
            Item._id = id;
            Item._descripcion = descripcion;
            Item.Item_Modificar();
            return 1;
        }

        public DataTable Item_Concepto_Obtener(int ConceptoId)
        {
            Item = new eItem();
            Item._conceptoid = ConceptoId;
            return Item.Item_Conceptos_Obtener();            
        }

        #endregion

        #region New Concepto
                
        public Dictionary<int, string> ObtenerControles()
        {
            Dictionary<int, string> Controles = new Dictionary<int, string>();
            List<cf_Lcr> lista = new List<cf_Lcr>();

            lista = ListarControles();

            foreach (cf_Lcr control in lista)
            {
                Controles.Add(control.Id, control.NombreLcr);
            }
            return Controles;
        }

        public List<cf_Lcr> ListarControles()
        {
            List<cf_Lcr> lista = new List<cf_Lcr>();
            lista = InstanceOF.Obtener_Objeto(string.Format("select * from Lcr"), typeof(cf_Lcr)) ?? new List<cf_Lcr>();
            return lista;
        }

        public DataTable ObtenerItemConcepto(int tipo)
        {
            DataTable lista = new DataTable();
            lista = InstanceOF.Obtener_Tabla(string.Format(@"   SELECT
		                                                            ITEM.Id					AS [ItemId],
		                                                            ITEM.Descripcion		AS [Item],
		                                                            Concepto.Id				AS [ConceptoId],
		                                                            Concepto.Descripcion	AS [Concepto],
		                                                            Concepto.TipoConcepto	AS [Tipo]
	                                                            FROM
		                                                            Concepto		AS CONCEPTO
		                                                            inner join Item AS ITEM			ON CONCEPTO.Id = ITEM.ConceptoId
                                                                WHERE
                                                                    TipoConcepto = "+ tipo));
            return lista;
        }

        public void GuardarControl(string nombre, List<int> items)
        {
            int id = GuardarLcr(nombre);                // 1.- Intervenir lcr para el nombre del Control            
            bool cambios = GuardarCodigoLcr(id, items); // 2.- Intervenir CodigoLcr para la asociacion con los items
        }

        public bool GuardarCodigoLcr(int id, List<int> items)
        {
            bool resp = false;
            foreach (int item in items)
            {
                int result_control;
                cf_CodigoLcr Codigo = new cf_CodigoLcr();
                Codigo.LcrId = id;
                Codigo.Posicion = 2;
                Codigo.ItemId = item;
                result_control = InstanceOF.Establecer_Objeto(Codigo, typeof(cf_CodigoLcr), new List<string>(new string[] { "Id" }), InstanceOF.PROCESO_SP.CF_RELACIONAR_CONTROL);
                resp = result_control == -1 ? false : true;
                if (!resp)
                    return resp;
            }
            return resp;
        }

        public int GuardarLcr(string nombre)
        {
            int result_Control;
            cf_Lcr Control = new cf_Lcr();
            Control.EntidadId = 1;
            Control.NombreLcr = nombre;
            Control.EstadoId = 1;            
            result_Control = InstanceOF.Establecer_Objeto(Control, typeof(cf_Lcr), new List<string>(new string[] { "Id" }), InstanceOF.PROCESO_SP.CF_CONTROL_CREAR);                                
            return result_Control;
        }

        #endregion

        #region new CONTROLES

        public Dictionary<int, string> ListaControles()
        {
            Dictionary<int, string> lista = new Dictionary<int, string>();
            
            foreach(cf_Lcr Control in ObtenerListaControles())
            {
                lista.Add(Control.Id, Control.NombreLcr);
            }
            return lista;
        }

        public List<cf_Lcr> ObtenerListaControles()
        {
            List<cf_Lcr> ListaControles = new List<cf_Lcr>();
            ListaControles = InstanceOF.Obtener_Objeto(string.Format("select * from Lcr"), typeof(cf_Lcr)) ?? new List<cf_Lcr>();
            return ListaControles;
        }

        public List<int> ListaControlesAsociados(int ClienteId)
        {
            List<int> lista = new List<int>();
            foreach (cf_Lcr Control in ObtenerListaControles(ClienteId))
            {
                lista.Add(Control.Id);
            }
            return lista;
        }

        public List<cf_Lcr> ObtenerListaControles(int ClienteId)
        {
            List<cf_Lcr> ListaControles = new List<cf_Lcr>();
            ListaControles = InstanceOF.Obtener_Objeto(string.Format(@" select
	                                                                        Lcr.*	                                                                        
                                                                        from 
	                                                                        ClienteLcr 
	                                                                        inner join Lcr 
	                                                                        on ClienteLcr.LcrId = Lcr.Id
	                                                                        inner join CodigoLcr
	                                                                        on Lcr.Id = CodigoLcr.LcrId
                                                                        where ClienteId = " + ClienteId), typeof(cf_Lcr)) ?? new List<cf_Lcr>();
            return ListaControles;
        }

        public List<cf_ClienteLcr> ObtenerListaClientes(int ControlId)
        {
            List<cf_ClienteLcr> ListaControles = new List<cf_ClienteLcr>();
            ListaControles = InstanceOF.Obtener_Objeto(string.Format(@" select
		                                                                    clienteLcr.ClienteId
	                                                                    from 
		                                                                    ClienteLcr 
		                                                                    inner join Lcr 
		                                                                    on ClienteLcr.LcrId = Lcr.Id
		                                                                    inner join CodigoLcr
		                                                                    on Lcr.Id = CodigoLcr.LcrId
	                                                                    where
		                                                                    Lcr.Id = " + ControlId), typeof(cf_ClienteLcr)) ?? new List<cf_ClienteLcr>();
            return ListaControles;
        }

        public List<KeyValuePair<int, int>> ListaClientesAsociados(List<int> Controles)
        {            
            List<KeyValuePair<int, int>> resultado = new List<KeyValuePair<int, int>>();
            List<cf_ClienteLcr> listaClientes = new List<cf_ClienteLcr>();

            foreach (int id in Controles)
            {
                List<cf_ClienteLcr> clientes = ObtenerListaClientes(id);
                foreach (cf_ClienteLcr cl in clientes)
                {
                    resultado.Add(new KeyValuePair<int, int>(id, cl.ClienteId));
                }
            }
            return resultado;
        }

        #endregion
        
        #region Producto

        public Dictionary<int, string> GrupoInstrumento()
        {
            Dictionary<int, string> Grupo = new Dictionary<int, string>();
            List<cf_GrupoInstrumento> result = new List<cf_GrupoInstrumento>();
            result = InstanceOF.Obtener_Objeto(string.Format("select * from GrupoInstrumento"), typeof(cf_GrupoInstrumento))?? new List<cf_GrupoInstrumento>();
            foreach (cf_GrupoInstrumento grupo in result)
            {
                Grupo.Add(grupo.Id, grupo.Descripcion);
            }
            return Grupo;
        }

        public DataTable Instrumento()
        {
            DataTable result = InstanceOF.Obtener_Tabla(string.Format("select id, GrupoInstrumentoId, Descripcion from instrumento"));
            return result;
        }

        //public 

        public DataTable Producto_Obtiene(int id)
        {
            DataTable productos = new DataTable();
            Producto = new eProducto();
            Producto.ItemId = id;
            productos = Producto.Producto_Obtener();
            return productos;
        }

        public DataSet Producto_Obtiene_Tablas()
        {
            DataSet ds = new DataSet();
            eGrupoInstrumento grupo = new eGrupoInstrumento();
            eInstrumento instrumento = new eInstrumento();            
            Item = new eItem(3);

            ds.Tables.Add(grupo.GrupoInstrumento_Obtiene()); // validar
            ds.Tables.Add(instrumento.Instrumento_Obtiene());
            ds.Tables.Add(Item.Item_Obtener_Productos());
            
            return ds;
        }

        public void Producto_Almacenar(string nombre, List<int> instrumentos)
        {
            // crear Item
            Item = new eItem();
            Item._conceptoid = 56;
            Item._descripcion = nombre;
            int indice = Item.Item_Almacenar();

            // crear productos (asociacion item <---> Instrumentos)
            if (indice != -1)
            {
                Producto = new eProducto();
                Producto.ItemId = indice;
                Producto.Instrumentos = instrumentos;
                Producto.Nombre = nombre;
                Producto.Producto_Almacenar();
            }
            else 
            { 
                // eliminar item 
            }
        }

        public void Producto_Actualizar(int id, List<int> instrumentos)
        {
            Producto = new eProducto();
            Producto.EstadoId = 2; // verificar en tabla estado
        }

        #endregion

        #region Riesgo Cliente
        
        // metodo replicado para clientes globales
        public List<KeyValuePair<string, string>> CargarClientes()
        {
            List<cf_Cliente> ListaClientes = new List<cf_Cliente>();
            ListaClientes = InstanceOF.Obtener_Objeto(string.Format("select Id, NombreCliente, Codigo, Subcodigo from Cliente"), typeof(cf_Cliente)) ?? new List<cf_Cliente>();

            List<KeyValuePair<string, string>> nombres = new List<KeyValuePair<string, string>>();

            foreach (cf_Cliente cliente in ListaClientes)
            {
                nombres.Add(new KeyValuePair<string, string>(cliente.NombreCliente.TrimEnd() + ", " + cliente.Codigo + "-" + cliente.SubCodigo, cliente.Id.ToString()));
            }
            return nombres;
        }

        public Dictionary<int, string> ObtenerSegmentos()
        {
            List<cf_Segmento> ListaSegmentos = new List<cf_Segmento>();
            ListaSegmentos = InstanceOF.Obtener_Objeto(string.Format("select Id, NombreSegmento from segmento"), typeof(cf_Segmento)) ?? new List<cf_Segmento>();

            Dictionary<int, string> datos = new Dictionary<int, string>();

            foreach (cf_Segmento seg in ListaSegmentos)
            {
                datos.Add(Convert.ToInt32(seg.Id), seg.NombreSegmento.ToString());
            }            
            return datos;
        }

        public Dictionary<int, string> ObtenerClasificacion()
        {
            List<cf_ClasificacionCredito> ListaClasificacion = new List<cf_ClasificacionCredito>();
            ListaClasificacion = InstanceOF.Obtener_Objeto(string.Format("select Id, Valor from ClasificacionCredito"), typeof(cf_ClasificacionCredito)) ?? new List<cf_ClasificacionCredito>();
            
            Dictionary<int, string> datos = new Dictionary<int, string>();

            foreach (cf_ClasificacionCredito clas in ListaClasificacion)
            {
                datos.Add(Convert.ToInt32(clas.Id), clas.Valor.ToString());
            }
            return datos;
        }

        public List<KeyValuePair<string, int>> ObtenerCliente(int id)
        {
            List<cf_Cliente> cliente = new List<cf_Cliente>();
            cliente = InstanceOF.Obtener_Objeto(string.Format("select * from ClienteLcr where ClienteId = {0}", id), typeof(cf_Cliente)) ?? new List<cf_Cliente>();

            List<KeyValuePair<string, int>> valores = new List<KeyValuePair<string, int>>();
            
            valores.Add(new KeyValuePair<string, int>("Segmento", Convert.ToInt16(cliente[0].SegmentoId)));
            valores.Add(new KeyValuePair<string, int>("Clasificacion", Convert.ToInt16(cliente[0].ClasificacionCreditoId)));

            return valores;
        }

        public void ModificarRiesgoCliente(int id, int Clasificacion, int Segmento)
        {
            List<cf_Cliente> cliente = new List<cf_Cliente>();
            cf_ClasificaciconRiesgoCliente RiesgoCliente = new cf_ClasificaciconRiesgoCliente();
            
            cliente = InstanceOF.Obtener_Objeto(string.Format("select * from Cliente where Id = {0}", id), typeof(cf_Cliente)) ?? new List<cf_Cliente>();

            RiesgoCliente.ClasificacionCreditoId = cliente[0].ClasificacionCreditoId;
            RiesgoCliente.ClienteId = cliente[0].Id;
            RiesgoCliente.Fecha = DateTime.Today;

            cliente[0].ClasificacionCreditoId = Clasificacion;
            cliente[0].SegmentoId = Segmento;

            bool estadoHistorial = AlmacenaHistorial(RiesgoCliente);
            bool estadoUpdate       = UpdateCliente(cliente[0]);
        }

        public bool UpdateCliente(cf_Cliente cliente)
        {
            // (dynamic obj, Type _tipoClase, List<string> l_no_considerados, SqlConnection sqlCon, SqlTransaction sqlTran, PROCESO_SP _sp)
            //bool resultado = true;
            //using (SqlConnection sqlCon = Corp.Datos.Conexion.ObtieneConexion("CF"))
            //{
            //    using (SqlTransaction sqlTran = sqlCon.BeginTransaction())
            //    {
            //        try
            //        {
            //            int result_tran = InstanceOF.Establecer_Objeto(cliente, typeof(cf_Cliente), new List<string>(), sqlCon, sqlTran, InstanceOF.PROCESO_SP.CF_CLIENTE_UPDATE);
            //            if (result_tran == -1) throw new Exception();
                        
            //            sqlTran.Commit();                        
            //        }
            //        catch (Exception)
            //        {
            //            sqlTran.Rollback();
            //            resultado = false;
            //        }
            //    }
            //}
            //return resultado;
            int control_resultado = InstanceOF.Establecer_Objeto(cliente, typeof(cf_Cliente), new List<string>(), InstanceOF.PROCESO_SP.CF_CLIENTE_UPDATE);
            return control_resultado == -1 ? false : true;
        }       

        public bool AlmacenaHistorial(cf_ClasificaciconRiesgoCliente RiesgoCliente)
        {
            bool resultado = true;

            using (SqlConnection sqlCon = Corp.Datos.Conexion.ObtieneConexion("CF"))
            {
                using (SqlTransaction sqlTran = sqlCon.BeginTransaction())
                {
                    try
                    {
                        int result_tran = InstanceOF.Establecer_Objeto(RiesgoCliente, typeof(cf_ClasificaciconRiesgoCliente), new List<string>(new string[] { "Id" }), sqlCon, sqlTran, InstanceOF.PROCESO_SP.CF_CRC_UPDATE);
                        if (result_tran == -1) throw new Exception();

                        sqlTran.Commit();
                    }
                    catch (Exception)
                    {
                        sqlTran.Rollback();
                        resultado = false;
                    }
                }
            }
            return resultado;
        }

        #endregion

        #region Reduccuón Threshold
                
        public DataTable ObtenerListaThreshold()
        {   

            DataTable Threshold = InstanceOF.Obtener_Tabla(string.Format(@" select 
	                                                                            transaccion.id,
	                                                                            Valor as Clasificacion,
	                                                                            MontoREC as REC,
	                                                                            transaccion.SegmentoId,	
	                                                                            NombreSegmento as Segmento,
	                                                                            Transaccion.MontoThreshold as Monto
                                                                            from 
	                                                                            Transaccion
	                                                                            inner join Cliente on transaccion.ClienteId = Cliente.Id
	                                                                            inner join ClasificacionCredito on Cliente.ClasificacionCreditoId = ClasificacionCredito.id
	                                                                            left join Segmento on Cliente.SegmentoId = Segmento.Id	"));
            return Threshold;
        }
        
        #endregion

        #region Mantención Threshold

        public DataTable ObtenerThreshold()
        {
            DataTable Threshold = InstanceOF.Obtener_Tabla(string.Format(@" select 
                                                                                TRANSACCION.Id		as Id, 
                                                                                GRUPO.Descripcion	as Modulo, 
	                                                                            Producto = '',
                                                                                --TIPO.NombreTipoTransaccion as Nombre, 
                                                                                CLIENTE.NombreCliente as Nombre, 
                                                                                NumeroTransaccion as Operacion, 
                                                                                MontoREC as REC, 
                                                                                TRANSACCION.MontoThreshold as Threshold	
                                                                            from 
	                                                                            Transaccion as TRANSACCION
	                                                                            inner join Cliente as CLIENTE on TRANSACCION.ClienteId = CLIENTE.Id
	                                                                            inner join GrupoInstrumento as GRUPO on TRANSACCION.GrupoInstrumentoId = GRUPO.Id
	                                                                            inner join TransaccionTipo as TIPO on TRANSACCION.TransaccionTipoId = TIPO.Id"));
            return Threshold;
        }
        
        #endregion
    }
}
