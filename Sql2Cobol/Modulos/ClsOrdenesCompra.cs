using System;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using Sql2Cobol.Interfases;

namespace Sql2Cobol.Modulos
{
    public class ClsOrdenesCompra
    {
        protected cDataBase.DataBase db = new cDataBase.DataBase();
        protected cFunciones.Seguridad Seguridad = new cFunciones.Seguridad();

        public IMain vista;

        private string Archivo;

        private const string Tabla = "t_produc.ordenes_compra";
        private const string TablaAsociada = "t_produc.ordenes_compra_items";

        private const string Proceso = "s2c_s2c_Orden_Cmpr.cob";
        private const string Modulo = "ClsOrdenesCompra";

        private string runpath = "";

        public ClsOrdenesCompra(IMain view)
        {
            vista = view;
        }

        public void ProcesarModulo(string RunPath)
        {
            try
            {
                runpath = RunPath;
                MySqlConnection conn = db.Conectar();
                MySqlDataReader myReader = db.ObtenerDataReader(conn, $"SELECT * FROM {Tabla} where paso_a_cobol = 1 order by idorden_compra ASC");

                decimal FldOrdCompPorc, FldOrdCompCotiz, FldOrdCompDolar, FldOrdCompTotal, FldOrdCompIva, FldOrdCompOtros, FldOrdCompDto_1, FldOrdCompDto_2, FldOrdCompDto_3;
                Int32 FldTipoRegistro, FldOrdCompId, FldOrdCompProve, FldOrdCompFecha, FldOrdCompPend, FldOrdCompPago, FldOrdCompDepo, FldOrdCompEmpresa, FldOrdCompMoneda, FldOrdCompFec_ent_1, FldOrdCompFec_ent_2;
                Int32 FldOrdCompFec_ent_3, FldOrdCompFec_ent_4, FldOrdCompFec_ent_5, FldOrdCompPcia_ibb, FldOrdCompTipo;
                string FldOrdCompAutoriza = string.Empty, FldOrdCompAntic = string.Empty, FldOrdCompObs_1 = string.Empty, FldOrdCompSi_impre = string.Empty, FldOrdCompConfir = string.Empty, FldOrdCompResto = string.Empty, FldOrdCompObs_2 = string.Empty, FldOrdCompObs_3 = string.Empty;
                DateTime Fecha;

                while (myReader.Read())
                {
                    FldTipoRegistro = 1;
                    FldOrdCompId = Convert.ToInt32(myReader["idorden_compra"].ToString());
                    FldOrdCompProve = Convert.ToInt32(myReader["idproveedor"].ToString());
                    FldOrdCompFecha = Convert.ToInt32(myReader["fecha"].ToString().Substring(6, 4) + myReader["fecha"].ToString().Substring(3, 2) + myReader["fecha"].ToString().Substring(0, 2));

                    Fecha = (DateTime)myReader["fecha"];
                    FldOrdCompFecha = Convert.ToInt32($"{Fecha.Year.ToString()}{String.Format("{0:00}", Fecha.Month)}{String.Format("{0:00}", Fecha.Day)}");

                    FldOrdCompPend = Convert.ToInt32(myReader["marca_pendiente"].ToString());
                    FldOrdCompPago = Convert.ToInt32(myReader["idformapago"].ToString());
                    FldOrdCompDto_1 = Convert.ToDecimal(myReader["descuento_1"].ToString());
                    FldOrdCompDto_2 = Convert.ToDecimal(myReader["descuento_2"].ToString());
                    FldOrdCompDto_3 = Convert.ToDecimal(myReader["descuento_3"].ToString());
                    FldOrdCompAntic = myReader["anticipo_bu"].ToString();
                    FldOrdCompDepo = Convert.ToInt32(myReader["iddeposito"].ToString());
                    FldOrdCompObs_1 = myReader["observaciones"].ToString().PadRight(60);
                    FldOrdCompObs_2 = string.Empty;
                    FldOrdCompObs_3 = string.Empty;
                    FldOrdCompTotal = Convert.ToDecimal(myReader["total"].ToString());
                    FldOrdCompIva = Convert.ToDecimal(myReader["iva"].ToString());
                    FldOrdCompOtros = Convert.ToDecimal(myReader["otros"].ToString());
                    FldOrdCompSi_impre = myReader["imprime"].ToString();
                    FldOrdCompEmpresa = Convert.ToInt32(myReader["idempresa"].ToString());
                    FldOrdCompMoneda = Convert.ToInt32(myReader["idmoneda"].ToString());
                    FldOrdCompCotiz = Convert.ToDecimal(myReader["cotizacion"].ToString());
                    FldOrdCompDolar = Convert.ToDecimal(myReader["dolar"].ToString());
                    FldOrdCompAutoriza = myReader["autoriza"].ToString();

                    Fecha = (DateTime)myReader["fecha_entrega_1"];
                    FldOrdCompFec_ent_1 = Convert.ToInt32($"{Fecha.Year.ToString().Substring(2, 2)}{String.Format("{0:00}", Fecha.Month)}{String.Format("{0:00}", Fecha.Day)}");
                    Fecha = (DateTime)myReader["fecha_entrega_2"];
                    FldOrdCompFec_ent_2 = Convert.ToInt32($"{Fecha.Year.ToString().Substring(2, 2)}{String.Format("{0:00}", Fecha.Month)}{String.Format("{0:00}", Fecha.Day)}");
                    Fecha = (DateTime)myReader["fecha_entrega_3"];
                    FldOrdCompFec_ent_3 = Convert.ToInt32($"{Fecha.Year.ToString().Substring(2, 2)}{String.Format("{0:00}", Fecha.Month)}{String.Format("{0:00}", Fecha.Day)}");
                    Fecha = (DateTime)myReader["fecha_entrega_4"];
                    FldOrdCompFec_ent_4 = Convert.ToInt32($"{Fecha.Year.ToString().Substring(2, 2)}{String.Format("{0:00}", Fecha.Month)}{String.Format("{0:00}", Fecha.Day)}");
                    Fecha = (DateTime)myReader["fecha_entrega_5"];
                    FldOrdCompFec_ent_5 = Convert.ToInt32($"{Fecha.Year.ToString().Substring(2, 2)}{String.Format("{0:00}", Fecha.Month)}{String.Format("{0:00}", Fecha.Day)}");

                    FldOrdCompPcia_ibb = Convert.ToInt32(myReader["idprovincia_ibb"].ToString());
                    FldOrdCompConfir = myReader["confirmada"].ToString();
                    FldOrdCompTipo = Convert.ToInt32(myReader["tipo"].ToString());
                    FldOrdCompPorc = Convert.ToDecimal(myReader["porcentaje"].ToString());
                    FldOrdCompResto = string.Empty;

                    Archivo = $"ordpgo-a-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

                    GrabarInterfase($"{String.Format("{0:0}", FldTipoRegistro)}|{String.Format("{0:000000}", FldOrdCompId)}|{String.Format("{0:0000}", FldOrdCompProve)}|{String.Format("{0:00000000}", FldOrdCompFecha)}|{String.Format("{0:0}", FldOrdCompPend)}|{String.Format("{0:00}", FldOrdCompPago)}|{String.Format("{0:0000.00}", FldOrdCompDto_1)}|{String.Format("{0:0000.00}", FldOrdCompDto_2)}|{String.Format("{0:0000.00}", FldOrdCompDto_3)}|{FldOrdCompAntic}|{String.Format("{0:0000}", FldOrdCompDepo)}|{FldOrdCompObs_1.PadRight(60)}|{FldOrdCompObs_2.PadRight(60)}|{FldOrdCompObs_3.PadRight(60)}|{String.Format("{0:000000000000.00}", FldOrdCompTotal)}|{String.Format("{0:000000000000.00}", FldOrdCompIva)}|{String.Format("{0:000000000000.00}", FldOrdCompOtros)}|{FldOrdCompSi_impre}|{String.Format("{0:00}", FldOrdCompEmpresa)}|{String.Format("{0:00}", FldOrdCompMoneda)}|{String.Format("{0:000.0000}", FldOrdCompCotiz)}|{String.Format("{0:00.00}", FldOrdCompDolar)}|{FldOrdCompAutoriza}|{String.Format("{0:000000}", FldOrdCompFec_ent_1)}|{String.Format("{0:000000}", FldOrdCompFec_ent_2)}|{String.Format("{0:000000}", FldOrdCompFec_ent_3)}|{String.Format("{0:000000}", FldOrdCompFec_ent_4)}|{String.Format("{0:000000}", FldOrdCompFec_ent_5)}|{String.Format("{0:00}", FldOrdCompPcia_ibb)}|{FldOrdCompConfir}|{String.Format("{0:0}", FldOrdCompTipo)}|{String.Format("{0:000.00}", FldOrdCompPorc)}|{FldOrdCompResto.PadRight(2)}", false);

                    ProcesarSubModulo(FldOrdCompId);
                    if (EjecutarModulo())
                    {
                        if (EvaluarResultado())
                        {
                            if (ActualizarTabla(FldOrdCompId))
                            {
                                vista.MoverInterfase($@"{vista.DirectorioInterfases}\{Archivo}.request", true);
                                vista.MoverInterfase($@"{vista.DirectorioInterfases}\{Archivo}.response", true);
                                return;
                            }
                        }
                    }
                    vista.MoverInterfase($@"{vista.DirectorioInterfases}\{Archivo}.request", false);
                    vista.MoverInterfase($@"{vista.DirectorioInterfases}\{Archivo}.response", false);
                }
                db.Desconectar(conn);
            }
            catch (Exception e)
            {
                vista.InformarError($"Módulo {Modulo} : Excepción en proceso [ProcesarModulo]", e.ToString(), $@"{vista.DirectorioInterfases}\{Archivo}.request");
            }
        }

        public void ProcesarSubModulo(Int32 ID)
        {
            try
            {
                MySqlConnection conn = db.Conectar();
                MySqlDataReader myReader = db.ObtenerDataReader(conn, $"SELECT * FROM {TablaAsociada} where idorden_compra = {ID}");

                Int32 FldItem_TipoRegistro;
                decimal FldItem_Com_Orden, FldItem_Com_Producto, FldItem_Com_Tipo, FldItem_Com_Secuencia, FldItem_Com_Envase;
                decimal FldItem_Com_Sector, FldItem_Com_Ord_Tra, FldItem_Com_Proy, FldItem_Com_Precio, FldItem_Com_Dto;
                decimal FldItem_Com_Cantidad_1, FldItem_Com_Recibida_1, FldItem_Com_Cantidad_2, FldItem_Com_Recibida_2, FldItem_Com_Cantidad_3, FldItem_Com_Recibida_3, FldItem_Com_Cantidad_4;
                decimal FldItem_Com_Recibida_4, FldItem_Com_Cantidad_5, FldItem_Com_Recibida_5, FldItem_Com_Cantpre, FldItem_Com_Iva;
                string FldItem_Com_Renglon_1, FldItem_Com_Renglon_2, FldItem_Com_Res_1, FldItem_Com_Nom_Prod, FldItem_Com_Unidad, FldItem_Com_Uni_Pre, FldItem_Com_Caract, FldItem_Com_Obs;
                Double FldItem_Com_Cuenta;

                while (myReader.Read())
                {
                    FldItem_TipoRegistro = 2;
                    FldItem_Com_Orden = Convert.ToInt32(myReader["idorden_compra"].ToString());
                    FldItem_Com_Producto = Convert.ToInt32(myReader["idproducto"].ToString());
                    FldItem_Com_Tipo = Convert.ToInt32(myReader["tipo"].ToString());
                    FldItem_Com_Secuencia = Convert.ToInt32(myReader["secuencia"].ToString());
                    FldItem_Com_Nom_Prod = myReader["nombre_producto"].ToString();
                    FldItem_Com_Cantidad_1 = Convert.ToDecimal(myReader["cantidad_1"].ToString());
                    FldItem_Com_Recibida_1 = Convert.ToDecimal(myReader["recibida_1"].ToString());
                    FldItem_Com_Cantidad_2 = Convert.ToDecimal(myReader["cantidad_2"].ToString());
                    FldItem_Com_Recibida_2 = Convert.ToDecimal(myReader["recibida_2"].ToString());
                    FldItem_Com_Cantidad_3 = Convert.ToDecimal(myReader["cantidad_3"].ToString());
                    FldItem_Com_Recibida_3 = Convert.ToDecimal(myReader["recibida_3"].ToString());
                    FldItem_Com_Cantidad_4 = Convert.ToDecimal(myReader["cantidad_4"].ToString());
                    FldItem_Com_Recibida_4 = Convert.ToDecimal(myReader["recibida_4"].ToString());
                    FldItem_Com_Cantidad_5 = Convert.ToDecimal(myReader["cantidad_5"].ToString());
                    FldItem_Com_Recibida_5 = Convert.ToDecimal(myReader["recibida_5"].ToString());
                    FldItem_Com_Unidad = myReader["idunidad"].ToString();
                    FldItem_Com_Precio = Convert.ToDecimal(myReader["precio"].ToString());
                    FldItem_Com_Dto = Convert.ToDecimal(myReader["descuento"].ToString());
                    FldItem_Com_Envase = Convert.ToInt32(myReader["idenvase"].ToString());
                    FldItem_Com_Cantpre = Convert.ToDecimal(myReader["cant_presentacion"].ToString());
                    FldItem_Com_Uni_Pre = myReader["idun_presentacion"].ToString();
                    FldItem_Com_Iva = Convert.ToDecimal(myReader["tasa_iva"].ToString());
                    FldItem_Com_Sector = Convert.ToInt32(myReader["iddivision"].ToString());
                    FldItem_Com_Ord_Tra = Convert.ToInt32(myReader["orden_trabajo"].ToString());
                    FldItem_Com_Cuenta = Convert.ToDouble(myReader["idcuenta"].ToString());
                    FldItem_Com_Caract = myReader["caracteristicas"].ToString();
                    FldItem_Com_Proy = Convert.ToInt32(myReader["idcentro"].ToString());
                    FldItem_Com_Obs = myReader["observaciones"].ToString();
                    FldItem_Com_Renglon_1 = string.Empty;
                    FldItem_Com_Renglon_2 = string.Empty;
                    FldItem_Com_Res_1 = string.Empty;

                    GrabarInterfase($"{String.Format("{0:0}", FldItem_TipoRegistro)}|{String.Format("{0:000000}", FldItem_Com_Orden)}|{String.Format("{0:000000}", FldItem_Com_Producto)}|{String.Format("{0:0}", FldItem_Com_Tipo)}|{String.Format("{0:0000}", FldItem_Com_Secuencia)}|{FldItem_Com_Nom_Prod.PadRight(50)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Cantidad_1)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Recibida_1)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Cantidad_2)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Recibida_2)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Cantidad_3)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Recibida_3)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Cantidad_4)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Recibida_4)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Cantidad_5)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Recibida_5)}|{String.Format("{0:00000}", FldItem_Com_Unidad)}|{String.Format("{0:000000000000.0000}", FldItem_Com_Precio)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Dto)}|{String.Format("{0:000}", FldItem_Com_Envase)}|{String.Format("{0:0000000000.0000}", FldItem_Com_Cantpre)}|{String.Format("{0:0000}", FldItem_Com_Uni_Pre)}|{String.Format("{0:0000.00}", FldItem_Com_Iva)}|{String.Format("{0:0000}", FldItem_Com_Sector)}|{String.Format("{0:000000}", FldItem_Com_Ord_Tra)}|{String.Format("{0:000000000000}", FldItem_Com_Cuenta)}|{FldItem_Com_Caract}|{String.Format("{0:00}", FldItem_Com_Proy)}|{FldItem_Com_Obs.PadRight(30)}|{FldItem_Com_Renglon_1.PadRight(50)}|{FldItem_Com_Renglon_2.PadRight(50)}|{FldItem_Com_Res_1.PadRight(70)}", true);
                }
                db.Desconectar(conn);
            }
            catch (Exception e)
            {
                vista.InformarError($"Módulo {Modulo} : Excepción en proceso [ProcesarSubModulo]", e.ToString(), $@"{vista.DirectorioInterfases}\{Archivo}.request");
            }
        }

        private bool GrabarInterfase(string registro, bool sobreescribe)
        {
            try
            {
                StreamWriter Writer = new StreamWriter($@"{vista.DirectorioInterfases}\{Archivo}.request", sobreescribe);

                Writer.WriteLine(registro);
                Writer.Close();
                return true;
            }
            catch (Exception e)
            {
                vista.InformarError($"Módulo {Modulo} : Excepción en proceso [GrabarInterfase]", e.ToString(), $@"{vista.DirectorioInterfases}\{Archivo}.request");
                return false;
            }
        }

        private bool EjecutarModulo()
        {
            try
            {
                if (Seguridad.EjecutarProceso("runcobol", $@"Modulos\{Proceso} a={Archivo}|{runpath}|A") == -1)
                {
                    vista.InformarError($"Módulo {Modulo}.EjecutarModulo : Error al ejecutar el proceso {Proceso}", "", $@"{vista.DirectorioInterfases}\{Archivo}.request");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                vista.InformarError($"Módulo {Modulo} : Excepción en proceso [EjecutarModulo]", e.ToString(), $@"{vista.DirectorioInterfases}\{Archivo}.request");
                return false;
            }
        }

        private bool EvaluarResultado()
        {
            string Registro;
            StringBuilder builder = new StringBuilder();

            try
            {
                using (StreamReader reader = new StreamReader($@"{vista.DirectorioInterfases}\{Archivo}.response"))
                {
                    while ((Registro = reader.ReadLine()) != null)
                    {
                        if (Registro.Trim() != string.Empty)
                        {
                            string[] words = Registro.Split('|');

                            if (words[0] == "00")
                            {
                                return true;
                            }
                            else
                            {
                                builder.Append("Detalle del Error");
                                builder.AppendLine();
                                builder.AppendLine();
                                builder.Append("   Status    : ").Append(words[0]);
                                builder.AppendLine();
                                builder.Append("   Proceso   : ").Append(words[1]);
                                builder.AppendLine();
                                builder.Append("   Archivo   : ").Append(words[2]);
                                builder.AppendLine();
                                builder.Append("   Operacion : ").Append(words[3]);
                                builder.AppendLine();
                                builder.Append("   Request   : ").Append(words[4]);
                                builder.Append("");
                                builder.AppendLine();

                                vista.InformarError($"Módulo {Modulo}.EvaluarResultado : Error al analizar la respuesta del proceso: {Proceso}", builder.ToString(), "");

                                return false;
                            }
                        }
                        else
                        {
                            vista.InformarError($"Módulo {Modulo}.EvaluarResultado : Error al analizar la respuesta del proceso: {Proceso}", "", $@"{vista.DirectorioInterfases}\{Archivo}.request");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                vista.InformarError($"Módulo {Modulo} : Excepción en proceso [ActualizarTabla]", e.ToString(), $@"{vista.DirectorioInterfases}\{Archivo}.request");
                return false;
            }
        }

        private bool ActualizarTabla(Int32 Id)
        {
            try
            {
                MySqlConnection conn = db.Conectar();

                if (!db.EjecutarSQL(conn, $"UPDATE {Tabla} SET paso_a_cobol = 0 WHERE idorden_compra = {Id}"))
                {
                    vista.InformarError($"Módulo {Modulo}.ActualizarTabla : Error al actualizar la tabla {Tabla}", db.DescripcionError, $@"{vista.DirectorioInterfases}\{Archivo}.request");
                    db.Desconectar(conn);
                    return false;
                }
                else
                {
                    db.Desconectar(conn);
                    return true;
                }
            }
            catch (Exception e)
            {
                vista.InformarError($"Módulo {Modulo} : Excepción en proceso [ActualizarTabla]", e.ToString(), $@"{vista.DirectorioInterfases}\{Archivo}.request");
                return false;
            }
        }
    }
}