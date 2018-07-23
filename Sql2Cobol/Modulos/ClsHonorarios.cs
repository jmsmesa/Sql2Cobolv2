using System;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using Sql2Cobol.Interfases;

namespace Sql2Cobol.Modulos
{
    public class ClsHonorarios
    {
        protected cDataBase.DataBase db = new cDataBase.DataBase();
        protected cFunciones.Seguridad Seguridad = new cFunciones.Seguridad();

        public IMain vista;

        private string Archivo;
        private string runpath = "";

        private const string Tabla = "t_cilfa.honorarios";
        private const string Proceso = "s2c_honorarios.cob";
        private const string Modulo = "ClsHonorarios";

        public ClsHonorarios(IMain view)
        {
            vista = view;
        }

        public void ProcesarModulo(string RunPath)
        {
            try
            {
                runpath = RunPath;

                MySqlConnection conn = db.Conectar();
                MySqlDataReader myReader = db.ObtenerDataReader(conn, $"SELECT * FROM {Tabla} where pasa_a_cobol = 1 order by idhonorario, fch_alta ASC");

                Int32 FldHonorario, FldIdapm, FldAutorizamkt, FldAutorizaimp, FldDonacion;
                string FldFecha;

                while (myReader.Read())
                {
                    FldHonorario = Convert.ToInt32(myReader["idhonorario"]);
                    FldIdapm = Convert.ToInt32(myReader["idapm"].ToString());
                    FldFecha = myReader["fecha"].ToString().Substring(8, 2) + myReader[2].ToString().Substring(3, 2) + myReader[2].ToString().Substring(0, 2);
                    FldAutorizamkt = Convert.ToInt32(myReader["autorizamkt"].ToString());
                    FldAutorizaimp = Convert.ToInt32(myReader["autorizaimp"].ToString());
                    FldDonacion = Convert.ToInt32(myReader["donacion"].ToString());
                    Archivo = $"honora-a-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

                    GrabarInterfase($"{String.Format("{0:000000}", FldHonorario)}|{String.Format("{0:00}", FldIdapm)}|{String.Format("{0:000000}", FldFecha)}|{String.Format("{0:00}", FldAutorizamkt)}|{String.Format("{0:0}", FldAutorizaimp)}|{String.Format("{0:0}", FldDonacion)}");
                    if (EjecutarModulo())
                    {
                        if (EvaluarResultado())
                        {
                            if (ActualizarTabla(FldHonorario))
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

        private bool GrabarInterfase(string registro)
        {
            try
            {
                StreamWriter Writer = new StreamWriter($@"{vista.DirectorioInterfases}\{Archivo}.request", false);

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

                if (!db.EjecutarSQL(conn, $"UPDATE {Tabla} SET pasa_a_cobol = 0 WHERE idhonorario = {Id}"))
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