using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sql2Cobol.Interfases
{
    public interface IMain
    {
        Telerik.WinControls.UI.RadListView GlobalEdit { get; set; }
        string DirectorioInterfases { get; set; }
        string[] Parametros { get; set; }

        void GrabarDebug(string linea, string registro, bool pantalla);

        bool InformarError(string Mensaje, string Detalle, string Archivo);

        bool MoverInterfase(string Archivo, bool Estado);
    }
}