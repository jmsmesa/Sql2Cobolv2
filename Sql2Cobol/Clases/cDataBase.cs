using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace cDataBase
{
    public class DataBase
    {
        public int CodigoError { get; set; }
        public int NumeroError { get; set; }
        public string DescripcionError { get; set; }
        public bool StatusError { get; set; }
        public string Version { get; set; }

        protected cFunciones.AppSetting AppSetting = new cFunciones.AppSetting();
        protected cFunciones.Errores Error = new cFunciones.Errores();
        protected cFunciones.Seguridad Seguridad = new cFunciones.Seguridad();

        private MySqlTransaction trans;

        public bool EsTransaccion()
        {
            return trans != null;
        }

        public bool TestConection(string database = "DbConnection")
        {
            string coneccion = ConfigurationManager.ConnectionStrings[database].ConnectionString;

            try
            {
                MySqlConnection cn = new MySqlConnection(coneccion);
                cn.Open();
                cn.Close();
                cn.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public MySqlConnection Conectar(string database = "DbConnection")
        {
            LimpiarDatos();

            string coneccion = GetConnectionString(database);

            MySqlConnection cn = new MySqlConnection(coneccion);

            try
            {
                cn.Open();
                StatusError = true;
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
            }
            return cn;
        }

        public string GetConnectionString(string database)

        {
            string coneccion = ConfigurationManager.ConnectionStrings[database].ConnectionString;

            Configuration appconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringSettings connStringSettings = appconfig.ConnectionStrings.ConnectionStrings[database];

            return connStringSettings.ConnectionString;
        }

        public void SetConnectionString(string connectionStringName, string database)

        {
            Configuration appconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appconfig.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = database;
            appconfig.Save();
        }

        public bool Desconectar(MySqlConnection cn)
        {
            LimpiarDatos();
            try
            {
                cn.Close();
                StatusError = true;
                return true;
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return false;
            }
        }

        public MySqlDataReader ObtenerDataSet(MySqlConnection cn, string sqlCmd)
        {
            LimpiarDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sqlCmd, cn)
                {
                    CommandType = CommandType.Text
                };
                MySqlDataReader rdr = cmd.ExecuteReader();
                StatusError = true;
                return rdr;
            }
            catch (MySqlException ex)
            {
                MySqlDataReader dt = null;
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return dt;
            }
        }

        public DataTable ObtenerDataTable(MySqlConnection cn, string sqlCmd)
        {
            LimpiarDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sqlCmd, cn)
                {
                    CommandType = CommandType.Text
                };
                MySqlDataAdapter DaRec2 = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                DaRec2.Fill(dt);
                StatusError = true;
                return dt;
            }
            catch (MySqlException ex)
            {
                DataTable dt = new DataTable();
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return dt;
            }
        }

        public bool EjecutarSQL(MySqlConnection cn, string sqlCmd)
        {
            LimpiarDatos();
            try
            {
                MySqlCommand command = cn.CreateCommand();
                command.CommandText = sqlCmd;
                command.Transaction = trans;
                command.ExecuteNonQuery();
                StatusError = true;
                return true;
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return false;
            }
        }

        public DataTable ObternerDataAdapter(MySqlConnection cn, string sqlCmd)
        {
            LimpiarDatos();
            DataTable dt = new DataTable();
            try
            {
                using (MySqlDataAdapter sda = new MySqlDataAdapter(sqlCmd, cn))
                {
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return dt;
            }
        }

        public void LimpiarDatos()
        {
            CodigoError = 0;
            NumeroError = 0;
            StatusError = true;
            DescripcionError = "";
        }

        private void SetearError(Int32 ErrorCode, String Message, Int32 Number)
        {
            CodigoError = ErrorCode;
            DescripcionError = Message;
            NumeroError = Number;
            StatusError = false;
        }

        public bool BeginTransaction(MySqlConnection cn)
        {
            LimpiarDatos();
            try
            {
                trans = cn.BeginTransaction();
                return true;
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return false;
            }
        }

        public bool Commit()
        {
            LimpiarDatos();
            try
            {
                trans.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return false;
            }
        }

        public bool Rollback()
        {
            LimpiarDatos();
            try
            {
                trans.Rollback();
                return true;
            }
            catch (MySqlException ex)
            {
                SetearError(ex.ErrorCode, ex.Message, ex.Number);
                return false;
            }
        }

        public MySqlDataReader ObtenerDataReader(MySqlConnection cn, string sqlCmd, CommandType tipo = CommandType.Text)
        {
            LimpiarDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sqlCmd, cn)
                {
                    CommandType = tipo
                };

                MySqlDataReader rdr = cmd.ExecuteReader();
                StatusError = true;
                return rdr;
            }
            catch (MySqlException ex)
            {
                MySqlDataReader rdr = null;
                SetearError(ex.ErrorCode, ex.Message, ex.Number);

                return rdr;
            }
        }

        public bool LoadData(string archivo, string tabla, char terminadorCampo = ';', char terminadorLinea = '\n')
        {
            int error = 0;
            string sqlcmd = $"LOAD DATA LOCAL INFILE '{archivo}' IGNORE INTO TABLE {tabla} CHARACTER SET latin5 FIELDS TERMINATED BY '{terminadorCampo}' LINES TERMINATED BY '{terminadorLinea}'";

            MySqlConnection cn = Conectar();
            EjecutarSQL(cn, sqlcmd);

            error = CodigoError;

            Desconectar(cn);

            cn.Close();
            cn.Dispose();
            return error == 0;
        }

        public MySqlDataReader ObtenerRegistro(string Sql)
        {
            MySqlDataReader rdr = null;
            MySqlConnection cn = Conectar();
            try
            {
                rdr = ObtenerDataSet(cn, Sql);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    return rdr;
                }
            }
            catch (Exception)
            {
                return rdr;
            }
            Desconectar(cn);
            cn.Dispose();

            return rdr;
        }
    }
}