using System;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using Sql2Cobol.Interfases;
using System.Drawing;
using System.Data;
using Telerik.WinControls;
using System.Configuration;

namespace Sql2Cobol
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm, IMain
    {
        #region Interfases

        public Telerik.WinControls.UI.RadListView GlobalEdit { get; set; }
        public string DirectorioInterfases { get; set; }
        public string[] Parametros { get; set; }

        #endregion Interfases

        private bool Enviado;
        private MySqlDataReader rdr = null;
        protected string ArchivoDebug = "Sql2Cobol.log";
        protected string titulo = "Módulo de sinconización de datos ente MySQL y RM/COBOL";

        #region Clases

        protected cDataBase.DataBase BaseDatos = new cDataBase.DataBase();
        protected cFunciones.Varios Varios = new cFunciones.Varios();
        protected cFunciones.Seguridad Seguridad = new cFunciones.Seguridad();
        protected cFunciones.Registry reg = new cFunciones.Registry();
        protected cFunciones.AppSetting AppSetting = new cFunciones.AppSetting();
        protected cFunciones.Dialogo Dialogo = new cFunciones.Dialogo();
        protected CTelerik.Mensajes mensaje = new CTelerik.Mensajes();
        protected cMail.Mail sm = new cMail.Mail();

        #endregion Clases

        #region Módulos

        public Sql2Cobol.Modulos.ClsHonorarios LogicaHonorarios;
        public Sql2Cobol.Modulos.ClsOrdenesCompra LogicaOrdenesCompra;

        #endregion Módulos

        private char Opcion = ' ';
        private Int32 ID = 0;

        private void RadButton5_Click(object sender, EventArgs e)
        {
            if (mensaje.Pregunta("Confirma los cambios?"))
            {
                ActualizarAppSetting();
                if (mensaje.Pregunta($"Los cambios no seran actualizados hasta reiniciar el proceso.{Environment.NewLine}Desea reiniciar la aplicación?"))
                    Application.Restart();
            }
        }

        private void ActualizarAppSetting()
        {
            AppSetting.SetearAppSettings("ambiente", cbAmbiente.Text);
            AppSetting.SetearAppSettings("lapso", lapso.Text);
            AppSetting.SetearAppSettings("runtime", txtRuntime.Text);

            AppSetting.SetearAppSettings("destinatario", txtDestinatario.Text);

            if (AutoStart.Value)
                AppSetting.SetearAppSettings("AutoStart", "S");
            else
                AppSetting.SetearAppSettings("AutoStart", "N");

            if (tAutoRun.Value)
            {
                reg.AgregarAutoRun("Sql2Cobol", Application.ExecutablePath);
                AppSetting.SetearAppSettings("AutoRun", "S");
            }
            else
            {
                reg.BorrarAutoRun("Sql2Cobol");
                AppSetting.SetearAppSettings("AutoRun", "N");
            }

            AppSetting.SetearAppSettings("host", txtHost.Text);
            AppSetting.SetearAppSettings("puerto", txtPuerto.Text);
            AppSetting.SetearAppSettings("password", txtPasswrd.Text);

            if (chkSSL.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
                AppSetting.SetearAppSettings("ssl", "S");
            else
                AppSetting.SetearAppSettings("ssl", "N");

            AppSetting.SetearAppSettings("sender", txtRemitente.Text);

            BaseDatos.SetConnectionString("DbConnection", TxtStringConexion.Text);
        }

        private void ObtenerAppSetting()
        {
            cbAmbiente.Text = AppSetting.ObtenerAppSettings("ambiente");
            lapso.Text = AppSetting.ObtenerAppSettings("lapso");
            txtRuntime.Text = AppSetting.ObtenerAppSettings("runtime");

            txtDestinatario.Text = AppSetting.ObtenerAppSettings("destinatario");

            AutoStart.Value = AppSetting.ObtenerAppSettings("AutoStart") == "S";
            tAutoRun.Value = AppSetting.ObtenerAppSettings("AutoRun") == "S";

            txtHost.Text = AppSetting.ObtenerAppSettings("host");
            txtPuerto.Text = AppSetting.ObtenerAppSettings("puerto");
            txtPasswrd.Text = AppSetting.ObtenerAppSettings("password");
            chkSSL.ToggleState = (AppSetting.ObtenerAppSettings("ssl") == "S") ? Telerik.WinControls.Enumerations.ToggleState.On : Telerik.WinControls.Enumerations.ToggleState.Off;

            txtRemitente.Text = AppSetting.ObtenerAppSettings("sender");

            TxtStringConexion.Text = BaseDatos.GetConnectionString("DbConnection");
        }

        private void Salir_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RadButton2_Click(object sender, EventArgs e)
        {
            if (!Varios.ValidarEmail(txtDestinatario.Text))
                mensaje.Error("La dirección de correo no es válida");
            else
                mensaje.Informacion("La dirección de correo es válida");
        }

        private void RadButton1_Click(object sender, EventArgs e)
        {
        }

        private void VisorInterfases_Click(object sender, EventArgs e)
        {
            Seguridad.EjecutarProceso("VisorLogsWS.exe", "", false);
        }

        private void VerLog_Click(object sender, EventArgs e)
        {
            Visualizador frm = new Visualizador(ArchivoDebug);
            frm.ShowDialog();
            // Seguridad.EjecutarProceso("notepad", ArchivoDebug, false);
        }

        private void IrCarpeta_Click(object sender, EventArgs e)
        {
            Seguridad.EjecutarProceso("explorer", DirectorioInterfases, false);
        }

        private void TAutoRun_ValueChanged(object sender, EventArgs e)
        {
            if (tAutoRun.Value)
            {
                reg.BorrarAutoRun("Sql2Cobol");
                reg.AgregarAutoRun("Sql2Cobol", Application.ExecutablePath);
            }
            else
            {
                reg.BorrarAutoRun("Sql2Cobol");
            }
        }

        private void RadForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        public MainForm(string[] args)
        {
            InitializeComponent();
            Parametros = args;
        }

        private void LimpiarEditor_Click(object sender, EventArgs e)
        {
            Editor.Items.Clear();
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Estado_ValueChanged_1(object sender, EventArgs e)
        {
            if (AutoStart.Value)
            {
                timer1.Interval = 60000 * Convert.ToInt32(lapso.Text);
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            //if (Parametros.Length > 0)
            //{
            //    if (Parametros[0] == "-p")
            //    {
            //        mensaje.Informacion("Ingrese parametro [p]");
            //    }
            //}
            documentWindow2.Select();
            for (int x = 1; x < 61; x++)
                lapso.Items.Add(x.ToString());

            cbAmbiente.Items.Add("Desarrollo");
            cbAmbiente.Items.Add("Producción");

            try
            {
                ObtenerAppSetting();

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                DateTime fechaultima = File.GetLastWriteTime(Assembly.GetEntryAssembly().Location);

                this.Text = titulo;

                radLabelElement2.Text = "| Versión " + version + " - " + fechaultima.ToString(); // fechaultima.ToString("d");
                GlobalEdit = Editor;
                DirectorioInterfases = "interfases";
                ChequearExistenciaDirectorioInterfases();

                ChequearConexionDB();

                StartWait();

                if (AutoStart.Value)
                {
                    timer1.Enabled = true;
                    timer1.Interval = 60000 * Convert.ToInt32(lapso.Text);
                    // TODO Dejar 60000
                    timer1.Interval = 10000 * Convert.ToInt32(lapso.Text);

                    timer1.Start();
                }

                CargarGrilla();

                richTextBox1.LoadFile("Sql2Cobol.hlp", RichTextBoxStreamType.RichText);

                Editor.Items.Add("Eliminar en Producción");

                StopWait();
            }
            catch
            {
                mensaje.Error("Revise la configuracion del Módulo");
                StopWait();
                timer1.Enabled = false;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            StartWait();
            timer1.Stop();
            Display("Buscando registros para actualizar...");

            rdr = BaseDatos.ObtenerRegistro("SELECT * FROM t_cobol.modulos where nombre = 's2c_Honorarios'");

            if (rdr.HasRows && rdr["estado"].ToString() == "Habilitado")
            {
                LogicaHonorarios = new Sql2Cobol.Modulos.ClsHonorarios(this);
                LogicaHonorarios.ProcesarModulo(rdr["runpath"].ToString());
            }

            rdr.Close();
            rdr.Dispose();

            rdr = BaseDatos.ObtenerRegistro("SELECT * FROM t_cobol.modulos where nombre = 's2c_Ordenes_Pago'");

            if (rdr.HasRows && rdr["estado"].ToString() == "Habilitado")
            {
                LogicaOrdenesCompra = new Sql2Cobol.Modulos.ClsOrdenesCompra(this);
                LogicaOrdenesCompra.ProcesarModulo(rdr["runpath"].ToString());
            }

            rdr.Close();
            rdr.Dispose();

            timer1.Start();
            StopWait();
            // TODO Eliminar al instalar en produccion
            timer1.Enabled = false;
            //! Eliminar !!!!!!
        }

        public void GrabarDebug(string linea, string registro, bool pantalla)
        {
            try
            {
                StreamWriter Writer = new StreamWriter(ArchivoDebug, true);
                DateTime fecha = DateTime.Now;

                if (pantalla)
                {
                    Editor.Items.Add($"{String.Format("{0:d/M/yyyy HH:mm:ss}", fecha)} -> {linea}");
                }
                Writer.WriteLine($"{String.Format("{0:d/M/yyyy HH:mm:ss}", fecha)} -> {linea} ({registro})");

                Writer.Close();
            }
            catch
            {
            }
        }

        public bool InformarError(string Mensaje, string Detalle, string Archivo)
        {
            DateTime fecha = DateTime.Now;
            Editor.Items.Add($"{String.Format("{0:d/M/yyyy HH:mm:ss}", fecha)} >>> {Mensaje} - {Detalle} - {Archivo}");

            SendMail(Mensaje, Detalle, Archivo);
            return true;
        }

        public bool Display(string Mensaje)
        {
            DateTime fecha = DateTime.Now;
            Editor.Items.Add($"{String.Format("{0:d/M/yyyy HH:mm:ss}", fecha)} >>> {Mensaje}");
            return true;
        }

        private void SqlInsertLog(string interfase, string detalle, string notas, string registro)
        {
            string sqlText = string.Empty;
            DateTime fecha = DateTime.Now;

            try
            {
                MySqlConnection cn = BaseDatos.Conectar();
                if (BaseDatos.CodigoError == 0)
                {
                    sqlText = $"INSERT INTO cma.logs (fecha, interfase, codigo, detalle, notas) VALUES (STR_TO_DATE('{fecha}', '%d/%m/%Y %H:%i:%s'), '{interfase}', codigo, '{detalle}', '{notas}', '{registro}')";

                    BaseDatos.EjecutarSQL(cn, sqlText);
                    if (BaseDatos.CodigoError == 0)
                        BaseDatos.Desconectar(cn);
                }
            }
            catch
            {
            }
        }

        private void ChequearExistenciaDirectorioInterfases()
        {
            if (!Directory.Exists(DirectorioInterfases))
                Directory.CreateDirectory(DirectorioInterfases);

            if (!Directory.Exists($@"{DirectorioInterfases}\procesadas"))
                Directory.CreateDirectory($@"{DirectorioInterfases}\procesadas");

            if (!Directory.Exists($@"{DirectorioInterfases}\erroneas"))
                Directory.CreateDirectory($@"{DirectorioInterfases}\erroneas");
        }

        public bool MoverInterfase(string Archivo, bool Estado)
        {
            try
            {
                string sDir = string.Empty;
                if (Estado)
                    sDir = $@"{DirectorioInterfases}\Procesadas";
                else
                    sDir = $@"{DirectorioInterfases}\Erroneas";

                string filename = Path.GetFileName(Archivo);

                string Destino = $@"{sDir}\{filename}";

                if (File.Exists(Destino))
                    File.Delete(Destino);

                File.Move(Archivo, Destino);
                return true;
            }
            catch (Exception ex)
            {
                GrabarDebug($"Error MoverInterfase: {ex}", "", true);
                return false;
            }
        }

        private bool ChequearConexionDB()
        {
            try
            {
                MySqlConnection cn = BaseDatos.Conectar();
                if (BaseDatos.CodigoError == 0)
                {
                    BaseDatos.Desconectar(cn);
                }
                else
                {
                    if (!Enviado)
                    {
                        SendMail("Sql2Cobol - Error", "No se puede conectar al Servidor de Base de Datos");
                        Enviado = true;
                    }
                    GrabarDebug("No se pudo conectar a la Base de Datos", "", true);
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                GrabarDebug($"Error SQL: Nro: {ex.Number} - Descripción: {ex.Message}", "", true);
                return false;
            }
            return true;
        }

        public bool SendMail(string Asunto, string Cuerpo, string Archivo = "")
        {
            try
            {
                string Sender = AppSetting.ObtenerAppSettings("sender");

                sm.Remitente = Sender;
                sm.Destinatario = new List<string>();
                sm.ConCopia = new List<string>();
                sm.ConCopiaOculta = new List<string>();
                sm.Archivo = new List<string>();

                sm.Destinatario.Add(AppSetting.ObtenerAppSettings("destinatario"));

                sm.Cuerpo = Cuerpo;
                sm.Asunto = Asunto;
                sm.Archivo.Add(Archivo);

                if (!sm.EnivarCorreo())
                {
                    GrabarDebug($"No se puede enivar el email: {sm.MensajeError}", "", true);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                GrabarDebug("No se puede enivar el email", "", true);
                return false;
            }
        }

        private void StartWait()
        {
            Progreso.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            Progreso.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.Dash;
            Progreso.StartWaiting();
        }

        private void LimpiarEditor_Click_1(object sender, EventArgs e)
        {
            Editor.Items.Clear();
        }

        private void StopWait()
        {
            Progreso.StopWaiting();
            Progreso.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
        }

        private void RadButton3_Click(object sender, EventArgs e)
        {
            SendMail("Mensaje de prueba", "Detalle del mensaje");
        }

        private void RadButton1_Click_1(object sender, EventArgs e)
        {
            if (!Varios.ValidarEmail(txtRemitente.Text))
                mensaje.Error("La dirección de correo no es válida");
            else
                mensaje.Informacion("La dirección de correo es válida");
        }

        private void CargarGrilla()
        {
            DataTable dt = new DataTable();
            string coneccion = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            MySqlConnection cn = new MySqlConnection(coneccion);

            Grilla.Columns.Clear();

            Grilla.DataSource = BaseDatos.ObternerDataAdapter(cn, "SELECT nombre, descripcion, estado, runpath, idmodulos FROM t_cobol.modulos");

            Grilla.Columns[4].IsVisible = false;
            Grilla.Columns[0].HeaderText = "Programa";
            Grilla.Columns[1].HeaderText = "Descripcion";
            Grilla.Columns[2].HeaderText = "Estado";
            Grilla.Columns[3].HeaderText = "RUNPATH";
            Grilla.Columns[4].HeaderText = "id ";

            Grilla.Columns[0].Width = 100;
            Grilla.Columns[1].Width = 300;
            Grilla.Columns[2].Width = 100;
            Grilla.Columns[3].Width = 300;
            Grilla.Columns[4].Width = 20;

            Grilla.Columns[0].TextAlignment = ContentAlignment.MiddleLeft;
            Grilla.Columns[1].TextAlignment = ContentAlignment.MiddleLeft;
            Grilla.Columns[2].TextAlignment = ContentAlignment.MiddleLeft;
            Grilla.Columns[3].TextAlignment = ContentAlignment.MiddleLeft;

            ID = 1;
        }

        private void ProcesarAlta()
        {
            string estado = string.Empty;

            if (SelEstado.Value)
                estado = "Habilitado";
            else
                estado = "Inhabilitado";

            try
            {
                MySqlConnection cn = BaseDatos.Conectar();
                BaseDatos.EjecutarSQL(cn, $"INSERT INTO t_cobol.modulos (nombre, estado, descripcion, runpath) VALUES ('{TxtNombre.Text}', '{estado}', '{TxtDescripcion.Text}', '{TxtRunpath.Text}')");
                BaseDatos.Desconectar(cn);
            }
            catch (MySqlException ex)
            {
                mensaje.Error(ex.Message, "Error de Base de datos");
            }
        }

        private void ProcesarModificacion()
        {
            string estado = string.Empty;

            if (SelEstado.Value)
                estado = "Habilitado";
            else
                estado = "Inhabilitado";
            try
            {
                MySqlConnection cn = BaseDatos.Conectar();
                string SqlCommand = $"UPDATE t_cobol.modulos SET nombre = '{TxtNombre.Text}', estado = '{estado}', descripcion = '{TxtDescripcion.Text}', runpath = '{TxtRunpath.Text}' WHERE idmodulos = {ID}";
                BaseDatos.EjecutarSQL(cn, SqlCommand);
                BaseDatos.Desconectar(cn);
            }
            catch (MySqlException ex)
            {
                mensaje.Error(ex.Message, "Error de Base de datos");
            }
        }

        private void ProcesarBaja()
        {
            try
            {
                MySqlConnection cn = BaseDatos.Conectar();
                BaseDatos.EjecutarSQL(cn, $"DELETE FROM t_cobol.modulos WHERE nombre = '{TxtNombre.Text}'");
                BaseDatos.Desconectar(cn);
            }
            catch (MySqlException ex)
            {
                mensaje.Error(ex.Message, "Error de Base de datos");
            }
        }

        private void AutoStart_ValueChanged(object sender, EventArgs e)
        {
        }

        private void RadButton4_Click(object sender, EventArgs e)
        {
            txtRuntime.Text = Dialogo.DlgSeleccionarArchivo("Seleccionar Runtime RM/Cobol", "Archivos ejecutables (*.exe)|*.exe");
        }

        private void RadMenuItemAgregar_Click(object sender, EventArgs e)
        {
            Opcion = 'A';
            LblOpcion.Text = "ALTA";
            TxtDescripcion.Text = string.Empty;
            TxtNombre.Text = string.Empty;
            TxtRunpath.Text = string.Empty;
            SelEstado.Value = true;
            RadMenuItemConfirmar.Visibility = ElementVisibility.Visible;
            TxtNombre.Focus();
        }

        private void RadMenuItemModificar_Click(object sender, EventArgs e)
        {
            Opcion = 'M';
            LblOpcion.Text = "MODIFICACION";
            RadMenuItemConfirmar.Visibility = ElementVisibility.Visible;
        }

        private void RadMenuItemEliminar_Click(object sender, EventArgs e)
        {
            Opcion = 'B';
            LblOpcion.Text = "BAJA";
            RadMenuItemConfirmar.Visibility = ElementVisibility.Visible;
        }

        private void RadMenuItemConfirmar_Click(object sender, EventArgs e)
        {
            RadMenuItemConfirmar.Visibility = ElementVisibility.Hidden;

            if (mensaje.Pregunta("Confirma la operación?", "Actualización"))
            {
                switch (Opcion)
                {
                    case 'A':
                        ProcesarAlta();
                        break;

                    case 'B':
                        ProcesarBaja();
                        break;

                    case 'M':
                        ProcesarModificacion();
                        break;
                }
            }
            CargarGrilla();
            LblOpcion.Text = "";
        }

        private void Grilla_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.Value.ToString() == "Inhabilitado")
            {
                SelEstado.Value = true;
                Grilla.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Habilitado";
                ProcesarModificacion();
            }
            else if (e.Value.ToString() == "Habilitado")
            {
                SelEstado.Value = false;
                ProcesarModificacion();
            }
        }

        private void Grilla_SelectionChanged(object sender, EventArgs e)
        {
            int indice = 0;

            RadMenuItemConfirmar.Visibility = ElementVisibility.Hidden;

            if (Grilla.RowCount > 0)
            {
                if (Grilla.CurrentRow != null)
                {
                    if (Grilla.CurrentRow.Index >= 0)
                    {
                        indice = Grilla.Rows.IndexOf(Grilla.CurrentRow);
                        TxtNombre.Text = Grilla.Rows[indice].Cells["nombre"].Value.ToString();
                        TxtDescripcion.Text = Grilla.Rows[indice].Cells["descripcion"].Value.ToString();
                        TxtRunpath.Text = Grilla.Rows[indice].Cells["runpath"].Value.ToString();
                        ID = (Int32)Grilla.Rows[indice].Cells[4].Value;
                        SelEstado.Value = Grilla.Rows[indice].Cells["estado"].Value.ToString() == "Habilitado";
                    }
                    else
                    {
                        TxtNombre.Text = "";
                        TxtDescripcion.Text = "";
                        TxtRunpath.Text = "";
                        SelEstado.Value = true;
                    }
                }
            }
        }

        private void RadButton6_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection cn = new MySqlConnection(TxtStringConexion.Text);
                cn.Open();
                mensaje.Informacion("Conexión establecida correctmente");
                cn.Close();
                cn.Dispose();
            }
            catch
            {
                mensaje.Error("No se pudo establecer la conexión, revise los parámetros");
            }
        }

        private void SelEstado_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}