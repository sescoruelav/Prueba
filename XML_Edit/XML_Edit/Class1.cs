using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinForms.Controls.SyntaxEditor.Palettes;
using Telerik.WinForms.Controls.SyntaxEditor.Taggers;
using Telerik.WinForms.SyntaxEditor.Core.Tagging;
using Telerik.WinForms.SyntaxEditor.Core.Text;

namespace XML_Edit
{
     public enum AuxType { XML, JSON, XAML}
    public enum ClassStatus { empty, fileLoaded, fileUnsaved}
    public class AuxEditor
    {
        private string auxFile;
        private AuxType auxType = AuxType.XML;//valor por defecto;
        private ClassStatus status = ClassStatus.empty;
        private bool readOnly=true; 
        private RadSyntaxEditor rsEditor = new RadSyntaxEditor();
        private 
        AuxEditor(string file)
        {
            //TODO posible mejora de deteccion automática de tipo por la extension del fichero
            AuxLoad(file);
        }
        public  AuxEditor(string file, AuxType tipo) {
            auxType = tipo;

        }
        private void AuxLoad(string file)
        {
            if (!status.Equals(ClassStatus.empty))
            {
                Exception ex = new Exception("Ya hay un fichero abierto\n " + auxFile);
                throw ex;
            }
            try
            {
                auxFile = file;
                using (StreamReader reader = new StreamReader(file))
                {
                    this.rsEditor.Document = new TextDocument(reader);
                }
                rsEditor.Show();
            }catch(Exception ex)
            {
                throw ex;
            }
            

        }

    }
    


}
