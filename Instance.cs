using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace Corp.Datos.Clases
{
    public static class InstanceOF
    {
        public enum PROCESO_SP
        { 
            TX = 1,
            TX_D = 2,
            TX_F = 3,            
            FF_MM = 4,
            FF_MM_LIMPIA = 5,
            FF_MM_GENERALINEA = 6,
            TX_LN_EXCESO = 7,
            TX_LN_REBAJE_UP = 8,
            LN_DEFAULT = 9,
            LN_SUB = 10,
            TX_LOGICA = 11,
            TX_ESTRUCTURA = 12,
            TX_DETALLE = 13,
            CF_CLIENTE_UPDATE = 14,
            CF_CRC_UPDATE = 15,
            TX_LM_EXCESO = 16,
            TX_LM_REBAJE_UP = 17,
            PARIDAD = 18,
            LN_CLIENTES = 19,
            MONEDAS_ENTIDAD = 20,
            LN_PRINCIPAL = 21,
            CF_CONTROL_CREAR = 22,    
            CF_RELACIONAR_CONTROL = 23,
            MN_INSERT_ITEM = 24,
            MN_INSERT_PRODUCTO = 25,
            MN_UPDATE_ITEM = 26

        }
        private static Dictionary<PROCESO_SP, string> dSP = new Dictionary<PROCESO_SP, string> 
        {
            { PROCESO_SP.TX, "sp_Transaccion_Insert" },
            { PROCESO_SP.TX_D, "sp_TransaccionDetalle_Insert" },
            { PROCESO_SP.TX_F, "sp_TransaccionFlujo_Insert" },
            { PROCESO_SP.FF_MM, "sp_CargaFFMM" },
            { PROCESO_SP.FF_MM_LIMPIA, "sp_Elimina_CargaFFMM"},
            { PROCESO_SP.FF_MM_GENERALINEA, "sp_Genera_LineaFFMM"},
            { PROCESO_SP.TX_LN_EXCESO, "sp_ExcesoLinea_Insert"},
            { PROCESO_SP.TX_LN_REBAJE_UP, "sp_ClienteLcr_Update"},
            { PROCESO_SP.LN_DEFAULT, "sp_Verifica_Linea_Default"},
            { PROCESO_SP.LN_SUB, "sp_Verifica_SubLinea_Default"},
            { PROCESO_SP.TX_LOGICA, "sp_Logica_Insert"},
            { PROCESO_SP.TX_ESTRUCTURA, "sp_Estructura_Insert"},
            { PROCESO_SP.TX_DETALLE, "sp_EstructuraDetalle_Insert"},
            { PROCESO_SP.CF_CLIENTE_UPDATE, "sp_Cliente_Update"},
            { PROCESO_SP.CF_CRC_UPDATE, "sp_Clasificacionriesgocliente_Insert"},
            { PROCESO_SP.TX_LM_EXCESO, "sp_ExcesoLimite_Insert"},
            { PROCESO_SP.TX_LM_REBAJE_UP, "sp_UsuarioLimite_Update"},
            { PROCESO_SP.PARIDAD, "sp_Paridad"},
            { PROCESO_SP.LN_CLIENTES, "sp_Obtiene_Lineas_Clientes"},
            { PROCESO_SP.MONEDAS_ENTIDAD, "sp_Obtiene_Monedas_Entidad"},
            { PROCESO_SP.LN_PRINCIPAL, "Linea_Principal"},
            { PROCESO_SP.CF_CONTROL_CREAR, "sp_Lcr_Insert"},
            { PROCESO_SP.CF_RELACIONAR_CONTROL, "sp_codigolcr_Insert"},
            { PROCESO_SP.MN_INSERT_ITEM, "sp_Item_Insert"},
            { PROCESO_SP.MN_INSERT_PRODUCTO, "sp_Producto_Insert" },
            { PROCESO_SP.MN_UPDATE_ITEM, "sp_Item_Update"}
        };
        private static Dictionary<Type, System.Data.SqlDbType> d_tipo = new Dictionary<Type, System.Data.SqlDbType> { { typeof(int), System.Data.SqlDbType.Int }, { typeof(DateTime), System.Data.SqlDbType.DateTime }, { typeof(decimal), System.Data.SqlDbType.Decimal }, { typeof(string), System.Data.SqlDbType.NVarChar }, { typeof(char), System.Data.SqlDbType.Char }, { typeof(Guid), System.Data.SqlDbType.UniqueIdentifier }, { typeof(float), System.Data.SqlDbType.Float } };

        /// <summary>
        /// Obtiene lista de campos de un objeto
        /// </summary>
        /// <param name="_tipoClase"> type</param>
        /// <returns> list string</returns>
        public static List<string> Obtener_Fields(Type _tipoClase)
        {
            try
            {
                return _tipoClase.GetFields().Select(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene diccionario field info de un objeto
        /// </summary>
        /// <param name="_tipoClase"> type </param>
        /// <returns>dictionary string fieldinfo</returns>
        public static Dictionary<string, FieldInfo> Obtener_Field_Infos(Type _tipoClase)
        {
            try
            {
                return (from x in _tipoClase.GetFields() select new { x.Name, info = x }).ToDictionary(p => p.Name, p => p.info);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// uso para procedimientos almacenados externos
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="_tipoClase"></param>
        /// <param name="l_no_considerados"></param>
        /// <param name="sqlCon"></param>
        /// <param name="_sp"></param>
        /// <returns></returns>
        public static dynamic Obtener_Objeto_Procedimiento(dynamic obj, Type _tipoClase, List<string> l_no_considerados, SqlConnection sqlCon, string _sp)
        {
            try
            {
                string sp_nombre = _sp;
                var cmd_transaccion = new SqlCommand(sp_nombre, sqlCon) { CommandType = System.Data.CommandType.StoredProcedure, CommandTimeout = 400 };

                dynamic result = null;

                if (obj != null)
                {
                    var l_fields = _tipoClase.GetFields().Select(p => p.Name).ToList();
                    List<MethodInfo> l_methods = _tipoClase.GetMethods().ToList() ?? new List<MethodInfo>();
                    List<string> l_outputs = l_methods.Select(p => p.Name).Contains("Out") ? obj.Out() : new List<string>();

                    l_fields.Where(p => !l_no_considerados.Contains(p)).ToList().ForEach(p =>
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = string.Format("@{0}", p);
                        param.SqlDbType = d_tipo[Obtener_Campo_Type(p, _tipoClase)];
                        param.Direction = l_outputs.Contains(p) ? ParameterDirection.Output : ParameterDirection.Input;
                        param.Value = Obtener_Campo(p, obj, _tipoClase);
                        cmd_transaccion.Parameters.Add(param);
                    });

                    if (l_outputs.Count() > 0)
                    {
                        cmd_transaccion.ExecuteNonQuery();

                        l_outputs.ForEach(p =>
                        {
                            Setear_Campo(p, obj, cmd_transaccion.Parameters[string.Format("@{0}", p)].Value, _tipoClase);
                        });

                        result = obj;
                    }
                    else
                    {
                        result = Convert.ToInt32(cmd_transaccion.ExecuteScalar());
                    }
                }
                else
                {
                    result = Convert.ToInt32(cmd_transaccion.ExecuteScalar());
                }

                
                
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtiene resultado procedimiento almacenado y se le dan parametros manuales, luego convierte a la clase señalada, siempre en relacion a los campos resultado de la query sean iguales a los del objeto
        /// </summary>
        /// <param name="cmd">string</param>
        /// <param name="_tipoClase">type</param>
        /// <param name="dParameters">dictionary</param>
        /// <returns></returns>
        public static dynamic Obtener_Objeto_Procedimiento(PROCESO_SP _sp, Type _tipoClase, Dictionary<string, object> dParameters)
        {
            try
            {
                string sp_nombre = dSP[_sp];
                dynamic lista = Crear_Lista(_tipoClase);
                if (string.IsNullOrEmpty(sp_nombre)) { throw new Exception(); }

                using (SqlConnection cnn = Corp.Datos.Conexion.ObtieneConexion("CF"))
                {
                    SqlCommand command = cnn.CreateCommand();
                    command.CommandText = sp_nombre;
                    command.CommandType = CommandType.StoredProcedure;

                    if (dParameters.Count > 0)
                    {
                        dParameters.ToList().ForEach(p => command.Parameters.Add(string.Format("@{0}", p.Key), d_tipo[p.Value.GetType()]).Value = p.Value);
                    }

                    SqlDataReader rdr = command.ExecuteReader();
                    DataTable dt = new DataTable();

                    dt.Load(rdr);

                    var _type_of_data = Activator.CreateInstance(_tipoClase);
                    List<string> l_fields = Obtener_Fields(_tipoClase);
                    Dictionary<string, FieldInfo> d_field_infos = Obtener_Field_Infos(_tipoClase);
                    dt.AsEnumerable().ToList().ForEach(row =>
                    {
                        dynamic _obj = Crear_Obj(_type_of_data, _tipoClase);
                        l_fields.ForEach(p =>
                        {
                            if (row.Table.Columns.Contains(p))
                            {
                                Type _tipo_campo = d_field_infos[p].FieldType;
                                var _type_of_data_campo = _tipo_campo != typeof(string) ? Activator.CreateInstance(_tipo_campo) : string.Empty;
                                
                                dynamic field_value = Obtener_Valor_Row(row, p, _tipoClase, _type_of_data_campo);
                                if (field_value != null) Setear_Campo(p, _obj, field_value, _tipoClase);
                            }
                        });
                        lista.Add(_obj);
                    });
                }


                return lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene resultado query desde connection modificable luego lo convierte a la clase señalada, siempre en relacion a los campos resultado de query sean iguales a los del objeto
        /// </summary>
        /// <param name="cmd"> string </param>
        /// <param name="_tipoClase"> Type </param>
        /// <returns> objeto requerido </returns>
        public static dynamic Obtener_Objeto(string cmd, Type _tipoClase, SqlConnection cnn)
        {
            try
            {
                if (string.IsNullOrEmpty(cmd)) { throw new Exception(); }
                dynamic lista = Crear_Lista(_tipoClase);

                using (cnn)
                {
                    SqlCommand command = cnn.CreateCommand();
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;

                    SqlDataReader rdr = command.ExecuteReader();
                    DataTable dt = new DataTable();

                    dt.Load(rdr);

                    var _type_of_data = Activator.CreateInstance(_tipoClase);
                    List<string> l_fields = Obtener_Fields(_tipoClase);
                    Dictionary<string, FieldInfo> d_field_infos = Obtener_Field_Infos(_tipoClase);
                    dt.AsEnumerable().ToList().ForEach(row =>
                    {
                        dynamic _obj = Crear_Obj(_type_of_data, _tipoClase);
                        l_fields.ForEach(p =>
                        {
                            if (row.Table.Columns.Contains(p))
                            {
                                Type _tipo_campo = d_field_infos[p].FieldType;
                                var _type_of_data_campo = _tipo_campo != typeof(string) ? Activator.CreateInstance(_tipo_campo) : string.Empty;

                                dynamic field_value = Obtener_Valor_Row(row, p, _tipoClase, _type_of_data_campo);
                                if (field_value != null) Setear_Campo(p, _obj, field_value, _tipoClase);
                            }
                        });
                        lista.Add(_obj);
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene resultado query luego lo convierte a la clase señalada, siempre en relacion a los campos resultado de query sean iguales a los del objeto
        /// </summary>
        /// <param name="cmd"> string </param>
        /// <param name="_tipoClase"> Type </param>
        /// <returns> objeto requerido </returns>
        public static dynamic Obtener_Objeto(string cmd,  Type _tipoClase)
        {
            try
            {
                if (string.IsNullOrEmpty(cmd)) { throw new Exception(); }
                dynamic lista = Crear_Lista(_tipoClase);

                using (SqlConnection cnn = Corp.Datos.Conexion.ObtieneConexion("CF"))
                {
                    SqlCommand command = cnn.CreateCommand();
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;

                    SqlDataReader rdr = command.ExecuteReader();
                    DataTable dt = new DataTable();

                    dt.Load(rdr);
                    
                    var _type_of_data = Activator.CreateInstance(_tipoClase);                    
                    List<string> l_fields = Obtener_Fields(_tipoClase);
                    Dictionary<string, FieldInfo> d_field_infos = Obtener_Field_Infos(_tipoClase);
                    dt.AsEnumerable().ToList().ForEach(row =>
                    {
                        dynamic _obj = Crear_Obj(_type_of_data, _tipoClase);
                        l_fields.ForEach(p =>
                        {
                            if (row.Table.Columns.Contains(p))
                            {
                                Type _tipo_campo = d_field_infos[p].FieldType;
                                var _type_of_data_campo = _tipo_campo != typeof(string) ? Activator.CreateInstance(_tipo_campo) : string.Empty;

                                dynamic field_value = Obtener_Valor_Row(row, p, _tipoClase, _type_of_data_campo);
                                if (field_value != null) Setear_Campo(p, _obj, field_value, _tipoClase);
                            }
                        });
                        lista.Add(_obj);
                    });
                    
                }                                
                
                return lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
               
        /// <summary>
        /// Obtiene resultado de Datable expresado en objeto tipo, siempre en relacion a los campos resultados del datatable sean iguales a los del objeto
        /// </summary>
        /// <param name="dt"> DataTable</param>
        /// <param name="_tipoClase"> Type</param>
        /// <returns></returns>
        public static dynamic Obtener_Objeto(DataTable dt, Type _tipoClase)
        {
            try
            {                

                var _type_of_data = Activator.CreateInstance(_tipoClase);
                dynamic lista = Crear_Lista(_tipoClase);
                List<string> l_fields = Obtener_Fields(_tipoClase);
                Dictionary<string, FieldInfo> d_field_infos = Obtener_Field_Infos(_tipoClase);
                dt.AsEnumerable().ToList().ForEach(row =>
                {
                    dynamic _obj = Crear_Obj(_type_of_data, _tipoClase);
                    l_fields.ForEach(p =>
                    {
                        if (row.Table.Columns.Contains(p))
                        {
                            Type _tipo_campo = d_field_infos[p].FieldType;
                            var _type_of_data_campo = _tipo_campo != typeof(string) ? Activator.CreateInstance(_tipo_campo) : string.Empty;
                            dynamic field_value = Obtener_Valor_Row(row, p, _tipoClase, _type_of_data_campo);
                            if (field_value != null) Setear_Campo(p, _obj, field_value, _tipoClase);
                        }
                    });
                    lista.Add(_obj);
                });
                return lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene resultado query expresado en DataTable
        /// </summary>
        /// <param name="cmd"> string </param>
        /// <returns>DataTable</returns>
        public static DataTable Obtener_Tabla(string cmd)
        {
            try
            {
                if (string.IsNullOrEmpty(cmd)) { throw new Exception(); }

                DataTable dt = new DataTable();

                using (SqlConnection cnn = Corp.Datos.Conexion.ObtieneConexion("CF"))
                {
                    SqlCommand command = cnn.CreateCommand();
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;

                    SqlDataReader rdr = command.ExecuteReader();

                    dt.Load(rdr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static int Eliminar_Registro(string cmd)
        {
            int resp = 0;
            try
            {
                if (string.IsNullOrEmpty(cmd)) { throw new Exception(); }

                DataTable dt = new DataTable();

                using (SqlConnection cnn = Corp.Datos.Conexion.ObtieneConexion("CF"))
                {
                    SqlCommand command = cnn.CreateCommand();
                    command.CommandText = cmd;
                    command.CommandType = CommandType.Text;
                    
                    resp = command.ExecuteNonQuery();

                    return resp;
                }                
            }
            catch (Exception ex)
            {
                return resp;
            }
        }


        /// <summary>
        /// Crea lista dinamica
        /// </summary>
        /// <param name="_tipoClase"></param>
        /// <returns></returns>
        private static dynamic Crear_Lista(Type _tipoClase)
        {
            Type _tipo_lista = typeof(List<>).MakeGenericType(_tipoClase);
            return Activator.CreateInstance(_tipo_lista);
        }

        /// <summary>
        /// crea objeto dinamico
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeofdata"></param>
        /// <param name="_tipoClase"></param>
        /// <returns></returns>
        public static T Crear_Obj<T>(T typeofdata, Type _tipoClase)
        {
            return (T)Activator.CreateInstance(_tipoClase);
        }

        /// <summary>
        /// Obtiene valor celda datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="_campo_busqueda"></param>
        /// <param name="_tipoClase"></param>
        /// <param name="_tipo_data"></param>
        /// <returns></returns>
        private static dynamic Obtener_Valor_Row<T>(DataRow row, string _campo_busqueda, Type _tipoClase, T _tipo_data)
        {
            try
            {
                FieldInfo _campo = _tipoClase.GetField(_campo_busqueda);
                Type _tipo_campo = _campo.FieldType;

                return row.Field<T>(_campo_busqueda);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
        /// <summary>
        /// Establece objeto en la base de datos retornando valor de insert o update
        /// </summary>
        /// <param name="obj">dynamic</param>
        /// <param name="_tipoClase">type</param>
        /// <param name="l_no_considerados">list string</param>
        /// <param name="sqlCon"> sql connection</param>
        /// <param name="sqlTran"> sql transaction </param>
        /// <param name="_sp"> store procedure </param>
        /// <returns>int</returns>       
        public static int Establecer_Objeto(dynamic obj, Type _tipoClase, List<string> l_no_considerados, SqlConnection sqlCon, SqlTransaction sqlTran, PROCESO_SP _sp)
        {
            try
            {
                string sp_nombre = dSP[_sp];
                var l_fields = _tipoClase.GetFields().Select(p=> p.Name).ToList();
                var cmd_transaccion = new SqlCommand(sp_nombre, sqlCon, sqlTran) { CommandType = System.Data.CommandType.StoredProcedure };

                l_fields.Where(p => !l_no_considerados.Contains(p)).ToList().ForEach(p =>
                {
                    cmd_transaccion.Parameters.Add(string.Format("@{0}", p), d_tipo[Obtener_Campo_Type(p, _tipoClase)]).Value = Obtener_Campo(p, obj,_tipoClase);
                });

                return Convert.ToInt32(cmd_transaccion.ExecuteScalar());
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        
        public static int Establecer_Objeto(dynamic obj, Type _tipoClase, List<string> l_no_considerados, InstanceOF.PROCESO_SP _sp)
        {
            using (SqlConnection sqlCon = Corp.Datos.Conexion.ObtieneConexion("CF"))
            {
                using (SqlTransaction sqlTran = sqlCon.BeginTransaction())
                {
                    try
                    {
                        int ret;
                        string sp_nombre = dSP[_sp];
                        var l_fields = _tipoClase.GetFields().Select(p => p.Name).ToList();
                        var cmd_transaccion = new SqlCommand(sp_nombre, sqlCon, sqlTran) { CommandType = System.Data.CommandType.StoredProcedure };

                        l_fields.Where(p => !l_no_considerados.Contains(p)).ToList().ForEach(p =>
                        {
                            cmd_transaccion.Parameters.Add(string.Format("@{0}", p), d_tipo[Obtener_Campo_Type(p, _tipoClase)]).Value = Obtener_Campo(p, obj, _tipoClase);
                        });                       

                        ret = Convert.ToInt32(cmd_transaccion.ExecuteScalar());
                        sqlTran.Commit();
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        sqlTran.Rollback();
                        return -1;
                    }
                }
            }
        }                    

        public static string Establecer_Linea_Objeto(dynamic obj, Type _tipoClase, List<string> l_no_considerados, SqlConnection sqlCon, SqlTransaction sqlTran, PROCESO_SP _sp)
        {

           
            try
            {
                string sp_nombre = dSP[_sp];
                var l_fields = _tipoClase.GetFields().Select(p => p.Name).ToList();
                var cmd_transaccion = new SqlCommand(sp_nombre, sqlCon, sqlTran) { CommandType = System.Data.CommandType.StoredProcedure };

                l_fields.Where(p => !l_no_considerados.Contains(p)).ToList().ForEach(p =>
                {
                    cmd_transaccion.Parameters.Add(string.Format("@{0}", p), d_tipo[Obtener_Campo_Type(p, _tipoClase)]).Value = Obtener_Campo(p, obj, _tipoClase);
                });

                return cmd_transaccion.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        public static int Eliminar_Objeto( SqlConnection sqlCon, SqlTransaction sqlTran, PROCESO_SP _sp,DateTime FechaCarga)
        {
            try
            {
                string sp_nombre = dSP[_sp];
                var cmd_transaccion = new SqlCommand(sp_nombre, sqlCon, sqlTran) { CommandType = System.Data.CommandType.StoredProcedure };

                cmd_transaccion.Parameters.Add("@FechaCarga", SqlDbType.DateTime).Value = FechaCarga;
                
                return Convert.ToInt32(cmd_transaccion.ExecuteScalar());
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        
        private static Type Obtener_Campo_Type(string _campo_busqueda, Type _tipoClase)
        {
            try
            {
                FieldInfo _campo = _tipoClase.GetField(_campo_busqueda);
                Type _tipo_campo;
                _tipo_campo = _campo.FieldType.IsGenericType ? _campo.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(_campo.FieldType) : _campo.FieldType : _campo.FieldType;

                return _tipo_campo;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static bool Comprobar_Campo(string _campo_busqueda, List<string> l_estructura)
        {
            try
            {
                if (l_estructura.Contains(_campo_busqueda)) { return true; }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static dynamic Obtener_Campo(string _campo_busqueda, dynamic _obj, Type _tipoClase)
        {
            try
            {
                FieldInfo _campo = _tipoClase.GetField(_campo_busqueda);

                return _campo.GetValue(Convert.ChangeType(_obj, _tipoClase));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Setear_Campo(string _campo_busqueda, dynamic _obj, object _valor, Type _tipoClase)
        {
            try
            {
                FieldInfo _campo = _tipoClase.GetField(_campo_busqueda);
                Type _tipo_campo;
                _tipo_campo = _campo.FieldType.IsGenericType ? _campo.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(_campo.FieldType) : _campo.FieldType : _campo.FieldType;

                _campo.SetValue(_obj, Convert.ChangeType(_valor, _tipo_campo));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
