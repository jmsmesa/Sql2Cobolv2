using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Net.Mail;
using System.IO.Compression;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace cFunciones
{
    public enum TipoApertura
    {
        Crear,
        Agregar
    }

    public enum RegExpTipo
    {
        Numero = 0,
        Importe = 1,
        Alfanumerico = 2,
        Alfabetico = 3
    }

    public enum Alcance
    {
        general = 0,
        usuario = 1,
        terminal = 2,
        desarrollo = 3
    }

    public enum Encriptacion
    {
        No = 0,
        Si = 1,
    }

    public class Seguridad
    {
        public DateTime Hoy { get; set; } = DateTime.Now;
        /* TIPS
              public decimal Importe { get; set; }
              public decimal iva { get; set; }
              public decimal Gravado => Importe;

              public decimal Total => iva == 0 ? Importe : (Importe * iva);

              private Func<decimal, decimal, decimal> multiplier = ( n, m ) => n * m;

              Importe = 100.0m; iva = 1.21m; MessageBox.Show(Convert.ToString(Gravado));
              MessageBox.Show(Convert.ToString(Total));
              MessageBox.Show(Convert.ToString(multiplier(Gravado, iva)));
        */

        public string Encriptar(string Texto)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Texto);
            return Convert.ToBase64String(encryted);
        }

        public string DesEncriptar(string Texto)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(Texto);
            return System.Text.Encoding.Unicode.GetString(decryted);
        }

        public bool VerficarAcceso(string[] Argumentos)
        {
            try
            {
                if (Argumentos.Length > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EsDesarrollo()
        {
            if (Debugger.IsAttached)
                return true;
            else
                return false;
        }

        public int EjecutarProceso(string Proceso, string Parametros, bool Espera = true)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Proceso, Parametros)
                {
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };

                Process proc = Process.Start(startInfo);
                if (Espera)
                    proc.WaitForExit();

                return proc.ExitCode;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int EjecutarProcesoOculto(string Proceso, string Parametros, bool Espera = true)
        {
            try
            {
                Process proc = new Process();

                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.FileName = Proceso;
                proc.StartInfo.Arguments = Parametros;
                proc.StartInfo.CreateNoWindow = true;

                proc.Start();

                if (Espera)
                    proc.WaitForExit();

                return proc.ExitCode;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int EjecutarComandoCMD(string comando)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process
                {
                    EnableRaisingEvents = false
                };
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                proc.StandardInput.WriteLine(comando);
                proc.StandardInput.Flush();
                proc.StandardInput.Close();
                proc.Close();
                return proc.ExitCode;
            }
            catch
            {
                return -1;
            }
        }

        public bool EsAdministrador()
        {
            try
            {
                Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal myUser = (WindowsPrincipal)Thread.CurrentPrincipal;
                return myUser.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class AppSetting
    {
        /// <summary>
        /// Seter el valor de una clave del App.config
        /// </summary>
        /// <param name="key">Clave a guardar</param>
        /// <param name="value">Valor a guardar</param>
        /// <param name="tipo">Tipo de alcance. Del enum alcance</param>
        /// <param name="enc">Encripta o no</param>
        /// <returns>Regresa true o false</returns>
        /// <example>
        /// <code>
        ///    cFunciones.AppSetting App = new cFunciones.AppSetting;
        ///    App.SetearAppSettings("ambiente", "hola que tal", cFunciones.alcance.desarrollo, cFunciones.encriptacion.Si);
        /// </code>
        /// </example>
        public bool SetearAppSettings(string key, string value, Alcance tipo = Alcance.general, Encriptacion enc = Encriptacion.No)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                switch (tipo)
                {
                    case Alcance.usuario:
                        key = $"{key}.{Environment.UserName.ToLower()}";
                        break;

                    case Alcance.terminal:
                        key = $"{key}.{Environment.MachineName.ToLower()}";
                        break;

                    case Alcance.desarrollo:
                        key = $"{key}.gds";
                        break;
                }
                if (enc == Encriptacion.Si && value != "")
                {
                    cFunciones.Seguridad seg = new cFunciones.Seguridad();
                    value = seg.Encriptar(value);
                }

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtener el valor de una clave del App.config
        /// </summary>
        /// <param name="key">Clave a buscar</param>
        /// ///
        /// <param name="tipo">Tipo de alcance</param>
        /// ///
        /// <param name="enc">Encripta o no</param>
        /// <returns>Regresa el valor o null si no lo encuentra</returns>
        /// <example>
        /// <code>
        ///    cFunciones.AppSetting App = new cFunciones.AppSetting;
        ///    string vambiente = App.ObtenerAppSettings("ambiente", cFunciones.alcance.desarrollo, cFunciones.encriptacion.Si);
        /// </code>
        /// </example>
        public string ObtenerAppSettings(string key, Alcance tipo = Alcance.general, Encriptacion enc = Encriptacion.No)
        {
            try
            {
                switch (tipo)
                {
                    case Alcance.usuario:
                        key = $"{key}.{Environment.UserName.ToLower()}";
                        break;

                    case Alcance.terminal:
                        key = $"{key}.{Environment.MachineName.ToLower()}";
                        break;

                    case Alcance.desarrollo:
                        key = $"{key}.gds";
                        break;

                    default:
                        break;
                }

                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";

                if (enc == Encriptacion.Si && result != "")
                {
                    cFunciones.Seguridad seg = new cFunciones.Seguridad();
                    result = seg.DesEncriptar(result);
                }
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "";
            }
        }
    }

    public class Fechas
    {
        public enum TimeType
        {
            Segundos,
            Minutos,
            Horas,
            Dias,
            Meses,
            Años
        }

        public int DateDiff(DateTime FechaIncial, DateTime FechaFinal)
        {
            try
            {
                TimeSpan ts = FechaIncial - FechaFinal;
                return ts.Days;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public DateTime DateAdd(TimeType Identificador, DateTime Fecha, double Valor)
        {
            try
            {
                switch (Identificador)
                {
                    case TimeType.Segundos:
                        return Fecha.AddSeconds(Valor);

                    case TimeType.Minutos:
                        return Fecha.AddMinutes(Valor);

                    case TimeType.Horas:
                        return Fecha.AddHours(Valor);

                    case TimeType.Dias:
                        return Fecha.AddDays(Valor);

                    case TimeType.Meses:
                        return Fecha.AddMonths(Convert.ToInt16(Valor));

                    case TimeType.Años:
                        return Fecha.AddYears(Convert.ToInt16(Valor));

                    default:
                        return Fecha.AddMilliseconds(Valor);
                }
            }
            catch (Exception)
            {
                return Fecha;
            }
        }

        public String DateAñosMesesDias(DateTime newdt, DateTime olddt)
        {
            Int32 anios;
            Int32 meses;
            Int32 dias;
            String str = "";

            anios = (newdt.Year - olddt.Year);
            meses = (newdt.Month - olddt.Month);
            dias = (newdt.Day - olddt.Day);

            if (meses < 0)
            {
                anios--;
                meses += 12;
            }
            if (dias < 0)
            {
                meses--;
                dias += DateTime.DaysInMonth(newdt.Year, newdt.Month);
            }

            if (anios < 0)
            {
                return "Fecha Invalida";
            }
            if (anios == 1)
                str = str + anios.ToString() + " año ";
            if (anios > 1)
                str = str + anios.ToString() + " años ";
            if (meses == 1)
                str = str + meses.ToString() + " mes ";
            if (meses > 1)
                str = str + meses.ToString() + " meses ";
            if (dias == 1)
                str = str + dias.ToString() + " día";
            if (dias > 1)
                str = str + dias.ToString() + " días";

            return str;
        }

        public System.DateTime JulianToDate(Int32 vDate)
        {
            //!Uso para CMA
            // No toma la hora. Solo la fecha
            // Ej. textBox1.Text = Julian2Gregorian(12121).ToString("dd/MM/yyyy");

            string y = "";

            if (vDate.ToString().Length == 5)
            {
                y = vDate.ToString().Substring(0, 2);
            }
            else if (vDate.ToString().Length == 7)
            {
                y = vDate.ToString().Substring(0, 4);
            }
            return Convert.ToDateTime("01/01/" + y).AddDays(Convert.ToInt32(vDate.ToString().Substring(vDate.ToString().Length - 3)) - 1);
        }

        public Int32 DateToJulian(string fecha)
        {
            //!Uso para CMA
            //Ej.  DateToJulian("30/04/2012");

            var myDate = Convert.ToDateTime(fecha);//   new DateTime(2012, 4, 30);
            var resultado = string.Format("{0:yy}{1:D3}", myDate, myDate.DayOfYear);

            return Convert.ToInt32(resultado);
        }

        public DateTime JulianToDateTime(double julianDate)
        {
            double unixTime = (julianDate - 2440587.5) * 86400;

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds(unixTime).ToLocalTime();
        }

        public double DateTimeToJulian(DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = dateTime.ToUniversalTime() - origin;
            double unixTime = Math.Floor(diff.TotalSeconds);
            double julianDate = (unixTime / 86400) + 2440587.5;

            return julianDate;
        }
    }

    public class Xml
    {
        public string ObtenerValor(string Grupo, string Variable, string Archivo)
        {
            try

            {
                Archivo = $@"{Application.StartupPath}\{Archivo}.xml";

                XmlDocument xDoc = new XmlDocument();
                string Retorno = "";
                xDoc.Load(Archivo);

                XmlNodeList xPersonas = xDoc.GetElementsByTagName(Grupo);
                XmlNodeList xLista = ((XmlElement)xPersonas[0]).GetElementsByTagName(Variable);

                foreach (XmlElement nodo in xLista)
                {
                    Retorno = nodo.InnerText;
                }
                return Retorno;
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public class IO
    {
        public int ErrorCodigo { get; set; }
        public string ErrDescripcion { get; set; }

        private StreamWriter reader;

        public bool Abrir(string Archivo, TipoApertura Tipo = TipoApertura.Agregar)
        {
            bool Tp = false;

            ErrorCodigo = 0;
            ErrDescripcion = string.Empty;

            if (TipoApertura.Agregar == Tipo)
            {
                Tp = true;
            }
            try
            {
                reader = new StreamWriter(Archivo, Tp);
                return true;
            }
            catch (Exception ex)
            {
                ErrorCodigo = ex.HResult;
                ErrDescripcion = ex.Message;

                return false;
            }
        }

        public bool Cerrar()
        {
            ErrorCodigo = 0;
            ErrDescripcion = string.Empty;

            try
            {
                reader.Close();
                return true;
            }
            catch (Exception ex)
            {
                ErrorCodigo = ex.HResult;
                ErrDescripcion = ex.Message;

                return false;
            }
        }

        public bool Grabar(string Mensaje, bool ConNL = true)
        {
            ErrorCodigo = 0;
            ErrDescripcion = string.Empty;

            try
            {
                if (ConNL)
                    reader.WriteLine(Mensaje);
                else
                    reader.Write(Mensaje);

                return true;
            }
            catch (Exception ex)
            {
                ErrorCodigo = ex.HResult;
                ErrDescripcion = ex.Message;

                return false;
            }
        }

        public bool GrabarAcct(string Mensaje, string archivo = @"i:\gdsAcct.log")
        {
            ErrorCodigo = 0;
            ErrDescripcion = string.Empty;

            try
            {
                if (Abrir(archivo))
                {
                    reader.WriteLine($"{Application.ProductName.ToUpper()};{Application.StartupPath.ToLower()};{Application.ProductVersion};{Environment.UserName.ToLower()};{Environment.MachineName.ToLower()};{DateTime.Now};{Mensaje}");
                }

                Cerrar();
                return true;
            }
            catch (Exception ex)
            {
                ErrorCodigo = ex.HResult;
                ErrDescripcion = ex.Message;

                return false;
            }
        }

        public bool GrabarLog(string Mensaje, string MasInfo = "", string archivo = @"i:\gdsCma.log")
        {
            string spaces = string.Empty;
            ErrorCodigo = 0;
            ErrDescripcion = string.Empty;

            try
            {
                if (Abrir(archivo))
                {
                    reader.WriteLine($"{Application.ProductName.ToUpper()}:");
                    reader.WriteLine($"     [{Application.StartupPath.ToLower()}] [{Application.ProductVersion}]");
                    reader.WriteLine($"     [{Environment.UserName.ToLower()}] [{Environment.MachineName.ToLower()}] [{DateTime.Now}]{Environment.NewLine}");
                    reader.WriteLine($"      >>> {Mensaje}");
                    if (MasInfo != "")
                    {
                        reader.WriteLine($"          {MasInfo}{Environment.NewLine}{spaces.PadLeft(Application.ProductName.Length + 1, '.')}{Environment.NewLine}");
                    }
                    else
                    {
                        reader.WriteLine($"{spaces.PadLeft(Application.ProductName.Length + 1, '.')}{Environment.NewLine}");
                    }

                    Cerrar();
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorCodigo = ex.HResult;
                ErrDescripcion = ex.Message;

                return false;
            }
        }
    }

    public class Registry
    {
        private RegistryKey key;

        public void SetearValor(string Nombre, string Valor, string Clave = "Software\\cma")
        {
            try
            {
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Clave);
                key.SetValue(Nombre, Valor);

                key.Close();
            }
            catch
            {
            }
        }

        public string ObtenerValor(string Nombre, string DefaultValueReturn = "", string Clave = "Software\\cma")
        {
            try
            {
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Clave, true);
                string Valor = key.GetValue(Nombre).ToString();
                key.Close();
                return Valor;
            }
            catch (Exception)
            {
                return DefaultValueReturn;
            }
        }

        public void EliminarValor(string Nombre, string Clave = "Software\\cma")
        {
            try
            {
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Clave, true);
                key.DeleteValue(Nombre);
                key.Close();
            }
            catch
            {
            }
        }

        public bool AgregarAutoRun(string nombreClave, string nombreApp)
        {
            try
            {
                RegistryKey runK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                // añadirlo al registro Si el path contiene espacios se debería incluir entre
                // comillas dobles
                if (!nombreApp.StartsWith("\"") && nombreApp.IndexOf(" ") > -1)
                {
                    nombreApp = "\"" + nombreApp + "\"";
                }
                runK.SetValue(nombreClave, nombreApp);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //
        public bool BorrarAutoRun(string nombreClave)
        {
            try
            {
                RegistryKey runK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                runK.DeleteValue(nombreClave, false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //
        }

        //
        public string ObtenerAutoRun(string nombreClave)
        {
            try
            {
                RegistryKey runK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);
                return runK.GetValue(nombreClave, "").ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    public class Errores
    {
        public string DescripcionError { get; set; }

        public bool StatusError { get; set; }

        public enum TipoMensajes
        {
            Error,
            Advertencia,
            Mensaje
        }
    }

    public class Varios
    {
        public enum Color { Yellow = 1, Blue, Green }

        public void Dummy()
        {
            DateTime thisDate = DateTime.Now;

            string s = "";

            s = String.Format(
                "(C) Currency: . . . . . . . . {0:C}\n" +
                "(D) Decimal:. . . . . . . . . {0:D}\n" +
                "(E) Scientific: . . . . . . . {1:E}\n" +
                "(F) Fixed point:. . . . . . . {1:F}\n" +
                "(G) General:. . . . . . . . . {0:G}\n" +
                "    (default):. . . . . . . . {0} (default = 'G')\n" +
                "(N) Number: . . . . . . . . . {0:N}\n" +
                "(P) Percent:. . . . . . . . . {1:P}\n" +
                "(R) Round-trip: . . . . . . . {1:R}\n" +
                "(X) Hexadecimal:. . . . . . . {0:X}\n",
                -123, -123.45f);
            MessageBox.Show(s, "Standard Numeric Format Specifiers");

            s = String.Format(
                "(d) Short date: . . . . . . . {0:d}\n" +
                "(D) Long date:. . . . . . . . {0:D}\n" +
                "(t) Short time: . . . . . . . {0:t}\n" +
                "(T) Long time:. . . . . . . . {0:T}\n" +
                "(f) Full date/short time: . . {0:f}\n" +
                "(F) Full date/long time:. . . {0:F}\n" +
                "(g) General date/short time:. {0:g}\n" +
                "(G) General date/long time: . {0:G}\n" +
                "    (default):. . . . . . . . {0} (default = 'G')\n" +
                "(M) Month:. . . . . . . . . . {0:M}\n" +
                "(R) RFC1123:. . . . . . . . . {0:R}\n" +
                "(s) Sortable: . . . . . . . . {0:s}\n" +
                "(u) Universal sortable: . . . {0:u} (invariant)\n" +
                "(U) Universal sortable: . . . {0:U}\n" +
                "(Y) Year: . . . . . . . . . . {0:Y}\n",
                thisDate);
            MessageBox.Show(s, "Standard DateTime Format Specifiers");

            s = String.Format(
                "(G) General:. . . . . . . . . {0:G}\n" +
                "    (default):. . . . . . . . {0} (default = 'G')\n" +
                "(F) Flags:. . . . . . . . . . {0:F} (flags or integer)\n" +
                "(D) Decimal number: . . . . . {0:D}\n" +
                "(X) Hexadecimal:. . . . . . . {0:X}\n",
                Color.Green);

            MessageBox.Show(s, "Standard Enumeration Format Specifiers");
        }

        public List<Process> ObtenerProcesosActivos(string Proceso)
        {
            List<Process> Procesos = null;
            Process[] procesos = Process.GetProcesses();
            foreach (Process proc in procesos)
            {
                if (Proceso?.Length == 0 || string.Equals(proc.ProcessName, Proceso, StringComparison.OrdinalIgnoreCase))
                    Procesos.Add(proc);
            }
            return Procesos;
        }

        public bool FileInfoCount(string archivo, out Int32 lineas, out Int32 caracteres, out Int32 min, out Int32 max, bool cbl = false)
        {
            string Registro;
            Int32 Record;
            lineas = 0;
            caracteres = 0;
            min = 999999999;
            max = 0;
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    while ((Registro = reader.ReadLine()) != null)
                    {
                        Record = Registro.Length;
                        lineas++;
                        caracteres += Record;
                        if (cbl)
                        {
                            Record = Registro.IndexOf("*>") == -1 ? Record : Registro.IndexOf("*>") + 1;

                            if (Registro.Length > 5)
                                Record = Registro.Substring(6, 1) == "*" ? 0 : Record;
                        }
                        min = Record < min ? Record : min;
                        max = Record > max ? Record : max;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FileFieldsCount(string archivo, char separador, out Int32 lineas, out Int32 min, out Int32 max)
        {
            string Registro;
            min = 999999999;
            max = 0;
            lineas = 0;
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    while ((Registro = reader.ReadLine()) != null)
                    {
                        lineas++;

                        string[] campos = Registro.TrimEnd().Split(separador);

                        min = campos.Length < min ? campos.Length : min;
                        max = campos.Length > max ? campos.Length : max;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FileConvertFixedToDelimitedFormat(string archivo, Int32[] pos, char separador = ';')
        {
            string registro, salida = $@"{Path.GetDirectoryName(archivo)}\{Path.GetFileNameWithoutExtension(archivo)}.out";
            StreamWriter swtr = new StreamWriter(salida);

            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    while ((registro = reader.ReadLine()) != null)
                    {
                        swtr.WriteLine(RecordConvertFixedToDelimitedFormat(registro, pos, separador));
                    }
                }
                swtr.Close();
                swtr.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string RecordConvertFixedToDelimitedFormat(string reg, Int32[] pos, char separador = ';')
        {
            string registro = string.Empty;
            Int32 desde = 0, cantidad = 0;
            try
            {
                for (Int32 c = 0; c < pos.Length; c++)
                {
                    desde = pos[c];

                    if (c == pos.Length - 1)
                    {
                        registro = $"{registro}{reg.Substring(desde)}{separador}";
                    }
                    else
                    {
                        cantidad = (pos[c + 1]) - pos[c];
                        registro = $"{registro}{reg.Substring(desde, cantidad)}{separador}";
                    }
                }
                return registro;
            }
            catch
            {
                return null;
            }
        }

        /*
           Int32[] posiciones = { 0, 4, 5, 8 };
           // Convierte todos los registros del archivo

            FileConvertFixedToDelimitedFormat(@"c:\tmp\entrada.txt", posiciones);

            // Convierte un registro
            string Res = RecordConvertFixedToDelimitedFormat("1234567890", posiciones, '#');
            if (Res == null)
               MessageBox.Show("Revise los separadores");
            else
               MessageBox.Show(Res);
        */

        public bool VerificarExistenciaConfigXml(bool MostrarMensaje = true)
        {
            string archivo = Application.ExecutablePath.ToLower().Replace(".exe", ".xml").Replace(".EXE", ".xml");

            if (File.Exists(archivo))
            {
                return true;
            }
            else
            {
                if (MostrarMensaje)
                {
                    MessageBox.Show($"No se encuentra el archivo {archivo}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }

        public System.Globalization.CultureInfo SetearLenguaje(string lenguaje)
        {
            try
            {
                System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(lenguaje);
                Culture.NumberFormat.CurrencyGroupSeparator = lenguaje == "en-US" ? "," : ".";
                Culture.NumberFormat.CurrencyDecimalSeparator = lenguaje == "en-US" ? "." : ",";

                return Culture;
            }
            catch
            {
                return null;
            }
            /* USO de funcion
                  System.Threading.Thread.CurrentThread.CurrentCulture = clase.SetearLenguaje("en-US");
            */
        }

        public bool ValidarEmail(string Email)
        {
            const String sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(Email, sFormato))
            {
                if (Regex.Replace(Email, sFormato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ValidarRut(string rut)
        {
            bool validacion = false;
            try
            {
                rut = rut.ToUpper();
                rut = rut.Replace(".", "").Replace("-", "");
                int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));
                char dv = char.Parse(rut.Substring(rut.Length - 1, 1));

                int m = 0, s = 1;
                for (; rutAux != 0; rutAux /= 10)
                {
                    s = (s + (rutAux % 10 * (9 - (m++ % 6)))) % 11;
                }

                if (dv == (char)(s != 0 ? s + 47 : 75))
                {
                    validacion = true;
                }
            }
            catch
            {
            }
            return validacion;
        }

        public bool ValidarFormatoEmail(string Email)
        {
            try
            {
                MailAddress MailAdr = new MailAddress(Email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidarCUIT(string CUIT)
        {
            Regex rg = new Regex("[A-Z_a-z]");
            CUIT = CUIT.Replace("-", "");

            if (rg.IsMatch(CUIT) || CUIT.Length != 11)
                return false;

            char[] cuitArray = CUIT.ToCharArray();
            double sum = 0;
            int bint = 0;
            int j = 7;
            for (int i = 5, c = 0; c != 10; i--, c++)
            {
                if (i >= 2)
                    sum += (Char.GetNumericValue(cuitArray[c]) * i);
                else
                    bint = 1;
                if (bint == 1 && j >= 2)
                {
                    sum += (Char.GetNumericValue(cuitArray[c]) * j);
                    j--;
                }
            }
            if ((cuitArray.Length - (sum % 11)) == Char.GetNumericValue(cuitArray[cuitArray.Length - 1]))
            {
                return true;
            }
            return false;
        }

        public string ObtenerStringConexionMySql(string ConfigFile)
        {
            Xml xml = new Xml();
            string Servidor = xml.ObtenerValor("Conexion", "Servidor", ConfigFile);
            string Usuario = xml.ObtenerValor("Conexion", "Usuario", ConfigFile);
            string Clave = xml.ObtenerValor("Conexion", "Clave", ConfigFile);
            string Base = xml.ObtenerValor("Conexion", "Base", ConfigFile);

            return $"Server={Servidor};Uid={Usuario};password={Clave};Database={Base}";
        }

        public string ObtenerExtensionArchivo(string archivo)
        {
            return Path.GetExtension(archivo);
        }

        public string ObtenerNombreArchivo(string archivo)
        {
            return Path.GetFileName(archivo);
        }

        public string ObtenerArchivoSinExtension(string archivo)
        {
            return Path.GetFileNameWithoutExtension(archivo);
        }

        public string ObtenerDrive(string archivo)
        {
            return Path.GetPathRoot(archivo);
        }

        public string RegExpObtenerValor(string entrada, RegExpTipo Tipo)
        {
            switch (Tipo)
            {
                case RegExpTipo.Alfabetico:
                    return Regex.Replace(entrada, "[^A-Za-z ]", "");

                case RegExpTipo.Alfanumerico:
                    return Regex.Replace(entrada, @"[^\w ]", "");

                case RegExpTipo.Importe:
                    return Regex.Replace(entrada, @"[^\d.,+-]", "");

                case RegExpTipo.Numero:
                    return Regex.Replace(entrada, @"[^\d]", "");

                default:
                    return entrada;
            }
        }
    }

    public class Zip
    {
        public List<string> ZipLeerArchivo(string archivoZip = "none")
        {
            Dialogo dlg = new Dialogo();

            List<string> archivos = new List<string>();
            ZipArchive zip;

            if (archivoZip != "none" && File.Exists(archivoZip))
            {
                zip = ZipFile.OpenRead(archivoZip);
            }
            else if (archivoZip != "none" && !File.Exists(archivoZip))
            {
                return null;
            }
            else
            {
                archivoZip = dlg.DlgSeleccionarArchivo("Seleccione un archivo");

                if (archivoZip?.Length == 0)
                {
                    return null;
                }
                else
                {
                    zip = ZipFile.OpenRead(archivoZip);
                }
            }

            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                archivos.Add(entry.FullName);
            }

            return archivos;
        }

        public bool ZipExtraer(string archivo, string directorioDestino)
        {
            Dialogo dlg = new Dialogo();

            if (archivo?.Length == 0)
            {
                archivo = dlg.DlgSeleccionarArchivo("Seleccione archivo");
            }
            else if (!File.Exists(archivo))
            {
                return false;
            }
            if (directorioDestino?.Length == 0)
            {
                directorioDestino = dlg.DlgSeleccionarDirectorio();
            }
            if (Directory.Exists(directorioDestino) && File.Exists(archivo))
            {
                ZipFile.ExtractToDirectory(archivo, directorioDestino);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ZipComprimir(string archivoZip, List<string> archivos)
        {
            //List<string> lista = new List<string>();
            //lista.Add("c:\\temp\\s0-2017070611293533.wsms");
            //lista.Add("c:\\temp\\s0-2017070614023389.wsms");
            //ZipComprimir("c:\\tmp\\zip.zip", lista).ToString();
            Dialogo dlg = new Dialogo();

            if (archivoZip?.Length == 0)
            {
                archivoZip = dlg.DlgSalvarArchivo("Seleccione archivo");
            }
            if (Directory.Exists(archivos[0]))
            {
                if (File.Exists(archivoZip))
                    File.Delete(archivoZip);

                ZipFile.CreateFromDirectory(archivos[0], archivoZip);
            }
            else
            {
                try
                {
                    if (File.Exists(archivoZip))
                        File.Delete(archivoZip);

                    ZipArchive zip = ZipFile.Open(archivoZip, ZipArchiveMode.Create);

                    foreach (string s in archivos)
                    {
                        zip.CreateEntryFromFile(s, Path.GetFileName(s), CompressionLevel.Optimal);
                    }
                    zip.Dispose();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Dialogo
    {
        public string[] DlgSeleccionarArchivos(string titulo, string filtro = "Todos los archivos (*.*)|*.*", bool multipelSeleccion = false)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Multiselect = multipelSeleccion,
                Title = titulo,
                Filter = filtro,

                FileName = ""
            };

            DialogResult resultado = openFileDialog1.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                string[] files = openFileDialog1.FileNames;
                return files;
            }
            else
            {
                return null;
            }
        }

        public string DlgSeleccionarArchivo(string titulo, string filtro = "Todos los archivos (*.*)|*.*", bool multipelSeleccion = false)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Multiselect = multipelSeleccion,
                Title = titulo,
                Filter = filtro,
                FileName = ""
            };

            DialogResult resultado = openFileDialog1.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        public string DlgSalvarArchivo(string titulo, string filtro = "Todos los archivos (*.*)|*.*")
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            openFileDialog1.Title = titulo;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = filtro;

            DialogResult resultado = saveFileDialog1.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        public string DlgSeleccionarDirectorio()
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult resultado = folderBrowserDialog1.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath;
            }
            else
            {
                return "";
            }
        }
    }

    public class Convertir
    {
        public string ByteToHexa(byte chr)
        {
            // Obtener ESC para imprimir: a = string.Format("{0:X}", (char)27)); Obtener NL para
            // imprimir: a = string.Format("{0:X}{1:X}", (char)12)(char)15)); Obtener 0 Binario para
            // imprimir: a = string.Format("{0:X}", (char)00));

            return string.Format("{0:X2}", Convert.ToByte(chr));
        }

        public byte HexaToByte(char chr)
        {
            return Convert.ToByte(chr);
        }

        public char HexaToChar(char chr)
        {
            return Convert.ToChar(Convert.ToByte(chr));
        }
    }

    public class Mensajes
    {
        public void Error(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Informacion(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Advertencia(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public bool Pregunta(string Mensaje)
        {
            if (MessageBox.Show(Mensaje, "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                return true;
            else
                return false;
        }
    }
}