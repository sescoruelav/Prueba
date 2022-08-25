using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls.SyntaxEditor.UI;
using Telerik.WinControls.UI;
using Telerik.WinForms.Controls.SyntaxEditor.Taggers;
using Telerik.WinForms.Controls.SyntaxEditor.UI;
using Telerik.WinForms.SyntaxEditor.Core.Editor;
using Telerik.WinForms.SyntaxEditor.Core.Tagging;
using Telerik.WinForms.SyntaxEditor.Core.Text;
using Newtonsoft.Json;
/* Pablo Boix ene 22
 * AuxEdit es una clase de ayuda a la visualizacion y edicion de ficheros auxiliares tipo json o XML.
 * sew usa llamando a la clase con el nombre de fichero o con el nombre y tipo
 * en el caso de que un json simple lo etiquetemos como tipo datatable nos lo muestra en un grid y permite guardarlo 
 * y versionar las copias de los ficheros originales. 
 *
 *V1.0 Visualizador general
 *V1.1 Editor para ciertos jsons
 *V1.2 Copia de seguridad al guardar lo editado
 *TODO V1.3 Comprobar si se ha modificado una datatable.
 *TODO V1.4 Manejar permisos de solo lectura
 *TODO V2.0 Editor de configuracion SERVICES.CONFIG.
 */

namespace XML_Edit
{
    public enum AuxType { XML, JSON, XAML, CS, CPP, VB, JS, SQL, DataTable }
    public enum ClassStatus { empty, fileLoaded, fileUnsaved}
    public class AuxEditor : Telerik.WinControls.UI.RadRibbonForm
    {
        #region fields
        private string auxFile;
        private AuxType auxType = AuxType.XML;//valor por defecto;
        private DataTable dt;
        private ClassStatus status = ClassStatus.empty;
        private bool readOnly=true; 
        private RadSyntaxEditor rsEditor;
        private RadForm form = new RadForm();
        private TaggerBase<ClassificationTag> currentLanguageTagger;
        private FoldingTaggerBase foldingTagger;
        private RadGridView grid;
        private RadButtonTextBox btnGuardar;
        #endregion fields
        public AuxEditor(string file)
        {
            auxFile = file;
            auxType = CalculaTipo(file);
            Mostrar(file);
        }
        public  AuxEditor(string file, AuxType tipo) {
            auxFile = file;
            auxType = tipo;
            Mostrar(file);
        }
        private void Mostrar(string file)
        {   //funcion principal.
            bool cargadoOK;
            switch(auxType)
            {
                case AuxType.DataTable:
                    cargadoOK=CargaDataTable(file);//instanciamos datatable
                    break;
                default:
                    rsEditor = new RadSyntaxEditor();//instanciamos editor
                    if (AuxLoad(file))
                    {
                        RegisterTagger(auxType); //Configuramos colores palabras clave
                        form.Text = auxFile;
                        form.Controls.Add(rsEditor);
                        rsEditor.Dock = DockStyle.Fill;
                    }
                    break;
            }
            AjustarForm();           //ajustar ventana
            form.ShowDialog();
        }
        private bool CargaDataTable(string file)
        {
            try
            {
                StreamReader read = new StreamReader(file);
                string json = @read.ReadToEnd();
                dt = JsonConvert.DeserializeObject<DataTable>(json);
                //Instancio un grid 
                grid = new RadGridView();
                grid.DataSource = dt;
                grid.Dock = DockStyle.Fill;
                grid.BestFitColumns();
                grid.AutoSizeRows = true;
                grid.BindingContext = new System.Windows.Forms.BindingContext();//si no inicializas el bindincontext las columnas parecen estar vacias. 
                //Y lo añado al form
                form.Controls.Add(grid);
                grid.Columns["SQL"].MaxWidth = 500;//La columna de SQL es la mas grande, limitamos su anchura para que no crezca en exceso
                grid.Columns["SQL"].WrapText = true;//Y que pueda ocupar varias lineas
                //Instancio un botón de guardar 
                btnGuardar = new RadButtonTextBox();
                btnGuardar.Dock = DockStyle.Top;
                btnGuardar.Location = new System.Drawing.Point(0, 0);
                btnGuardar.Name = "btnGuardar";
                btnGuardar.Size = new System.Drawing.Size(1032, 47);
                btnGuardar.Text = "guardar";//TODO cambiar por icono guardar para no traducir
                btnGuardar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                btnGuardar.TextChanged += new System.EventHandler(this.radButtonTextBox1_TextChanged);
                form.Controls.Add(btnGuardar);//Y lo añado al grid
                return true;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("No se ha encontrado el fichero"+"\n"+file);
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //TODO cambiar textchanged por boton presionado
        private void radButtonTextBox1_TextChanged(object sender, EventArgs e)
        {
            ActualizarFichero();
        }

        private void ActualizarFichero()
        {
            bool haCambiado = true;
            try
            {
                //TODO checkear que ha cambiado
                if (!haCambiado)
                    return;
                //Serializar el objeto
                string json = JsonConvert.SerializeObject(dt);
                json=json.Replace("},{", "},\n{");//Añado saltos de lineas entre elementos para una mejor legibilidad si se usase notepad
                //renombramos fichero
                string copiaSeg = RenameFile(auxFile);
                //Guardar el fichero actual. Ojo si petase entre el paso anterior y este, habriamos cambiado ell nombre al fichero y no habria sustituto
                using (StreamWriter str = new StreamWriter(auxFile))
                {
                    str.Write(json);
                    str.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("se ha producido un error actualizando el fichero:" + "\n"+ ex.Message +"\n"+auxFile);
                throw;
            }
        }
        private string RenameFile(string original)
        {
            string extension;
            string fileName;
            string path;
            string targetFileName="";
            if (original is null)
                return "";
            try
            {
                int counter = 0;
                extension = System.IO.Path.GetExtension(original);
                fileName = System.IO.Path.GetFileNameWithoutExtension(original);
                path = System.IO.Path.GetDirectoryName(original);
                targetFileName = System.IO.Path.Combine(path, fileName + counter.ToString() + extension);
                while (System.IO.File.Exists(targetFileName))
                {
                    counter += 1;
                    targetFileName = System.IO.Path.Combine(path, fileName + counter.ToString() + extension);
                }
                System.IO.File.Move(original, targetFileName);
                return targetFileName;
            }
            catch (Exception ex)
            {
                //TODO Mejorar gestion  de errores
                MessageBox.Show("Se ha producido un error renombrando el fichero:"+ "\n" + ex.Message+"\n"+original + "\n" + targetFileName);
                return "";
            }
        }

        private bool AuxLoad(string file)
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
                    reader.Close();//Ojo, dejarlo abierto tiene cierta funcion de bloqueo. Pero si queremos re-escribir hay que cerrar
                }
                status = ClassStatus.fileLoaded;
                return true;
            }catch(FileNotFoundException ex)
            {
                MessageBox.Show("No se ha encontrado el fichero" + "\n"+ file);
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            } 
        }


        private void AjustarForm()
        { 
            form.MaximizeBox=true;
            form.Dock=DockStyle.Fill;
        }

        #region Colores palabras clave
        private AuxType CalculaTipo(String fileName)
        {
            try
            {
                string extension = System.IO.Path.GetExtension(fileName);
                switch (extension)
                {
                    case ".cs":
                        return AuxType.CS;
                    case ".cpp":
                        return AuxType.CPP;
                    case ".vb":
                        return AuxType.VB;
                    case ".js":
                        return AuxType.JS;
                    case ".sql":
                        return AuxType.SQL;
                    case ".xaml":
                    case ".xml":
                    case ".licx":
                    case ".html":
                    case ".csproj":
                    case ".vbproj":
                    case ".user":
                    case ".config":
                    case ".resx":
                    case ".settings":
                    case ".sln":
                        return AuxType.XML;
                    case ".json":
                        return AuxType.JSON;
                    default:
                        return AuxType.XML;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RegisterTagger(AuxType tipo)
        {
            try
            {
                this.UnregisterTaggers();
                switch (tipo)
                {
                    case AuxType.CS:
                        this.currentLanguageTagger = new CSharpTagger(this.rsEditor.SyntaxEditorElement);
                        this.ClearXmlFormatDefinitions();
                        this.foldingTagger = new CSharpFoldingTagger(this.rsEditor.SyntaxEditorElement);
                        break;
                    case AuxType.CPP:
                        this.currentLanguageTagger = new CSharpTagger(this.rsEditor.SyntaxEditorElement);
                        this.ClearXmlFormatDefinitions();
                        this.foldingTagger = new BracketFoldingTagger(this.rsEditor.SyntaxEditorElement);
                        break;
                    case AuxType.VB:
                        this.currentLanguageTagger = new VisualBasicTagger(this.rsEditor.SyntaxEditorElement);
                        this.ClearXmlFormatDefinitions();
                        this.foldingTagger = new VisualBasicFoldingTagger(this.rsEditor.SyntaxEditorElement);
                        break;
                    case AuxType.JS:
                        this.currentLanguageTagger = new JavaScriptTagger(this.rsEditor.SyntaxEditorElement);
                        this.ClearXmlFormatDefinitions();
                        this.foldingTagger = new JavaScriptFoldingTagger(this.rsEditor.SyntaxEditorElement);
                        break;
                    case AuxType.SQL:
                        this.currentLanguageTagger = new SqlTagger(this.rsEditor.SyntaxEditorElement);
                        this.ClearXmlFormatDefinitions();
                        if (this.rsEditor.TaggersRegistry.IsTaggerRegistered(this.foldingTagger))
                        {
                            this.rsEditor.TaggersRegistry.UnregisterTagger(this.foldingTagger);
                        }
                        break;
                    case AuxType.XML:
                        this.currentLanguageTagger = new XmlTagger(this.rsEditor.SyntaxEditorElement);
                        this.AddXmlFormatDefinitions();
                        this.foldingTagger = new XmlFoldingTagger(this.rsEditor.SyntaxEditorElement);
                        break;
                    case AuxType.JSON:
                        this.currentLanguageTagger = new JsonTagger(this.rsEditor.SyntaxEditorElement);
                        //this.rsEditor.TaggersRegistry.RegisterTagger(this.currentLanguageTagger);
                        System.Drawing.Color color = Colors.Green;
                        this.rsEditor.TextFormatDefinitions.AddLast(JsonTypes.Key,
                                new TextFormatDefinition(new System.Drawing.SolidBrush(color)));
                        color = Colors.DarkRed;
                        this.rsEditor.TextFormatDefinitions.AddLast(JsonTypes.StringLiteral,
                                new TextFormatDefinition(new System.Drawing.SolidBrush(color)));
                        color = Colors.DarkGreen;
                        this.rsEditor.TextFormatDefinitions.AddLast(JsonTypes.Number,
                                new TextFormatDefinition(new System.Drawing.SolidBrush(color)));
                        color = Colors.Fuchsia;
                        this.rsEditor.TextFormatDefinitions.AddLast(JsonTypes.TrueFalseNull,
                                new TextFormatDefinition(new System.Drawing.SolidBrush(color)));
                        this.foldingTagger = new BracketFoldingTagger(this.rsEditor.SyntaxEditorElement);
                        //this.rsEditor.TaggersRegistry.RegisterTagger(this.foldingTagger);
                        break;
                    default:
                        this.ClearXmlFormatDefinitions();
                        this.currentLanguageTagger = null;
                        if (this.rsEditor.TaggersRegistry.IsTaggerRegistered(this.foldingTagger))
                        {
                            this.rsEditor.TaggersRegistry.UnregisterTagger(this.foldingTagger);
                        }
                        break;
                }

                if (this.currentLanguageTagger != null)
                {
                    this.rsEditor.TaggersRegistry.RegisterTagger(this.currentLanguageTagger);
                }

                if (this.foldingTagger != null)
                {
                    this.rsEditor.TaggersRegistry.RegisterTagger(this.foldingTagger);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UnregisterTaggers()
        {
            if (this.currentLanguageTagger != null && this.rsEditor.TaggersRegistry.IsTaggerRegistered(this.currentLanguageTagger))
            {
                this.rsEditor.TaggersRegistry.UnregisterTagger(this.currentLanguageTagger);
            }
            if (this.foldingTagger != null && this.rsEditor.TaggersRegistry.IsTaggerRegistered(this.foldingTagger))
            {
                this.rsEditor.TaggersRegistry.UnregisterTagger(this.foldingTagger);
                this.foldingTagger = null;
            }
        }

        private void AddXmlFormatDefinitions()
        {
            this.rsEditor.TextFormatDefinitions.AddLast(XmlSyntaxHighlightingHelper.XmlAttribute, XmlSyntaxHighlightingHelper.XmlAttributeFormatDefinition);
            this.rsEditor.TextFormatDefinitions.AddLast(XmlSyntaxHighlightingHelper.XmlElement, XmlSyntaxHighlightingHelper.XmlElementFormatDefinition);
            this.rsEditor.TextFormatDefinitions.AddLast(XmlSyntaxHighlightingHelper.XmlComment, XmlSyntaxHighlightingHelper.XmlCommentFormatDefinition);
            this.rsEditor.TextFormatDefinitions.AddLast(XmlSyntaxHighlightingHelper.XmlContent, XmlSyntaxHighlightingHelper.XmlContentFormatDefinition);
            this.rsEditor.TextFormatDefinitions.AddLast(XmlSyntaxHighlightingHelper.XmlString, XmlSyntaxHighlightingHelper.XmlStringFormatDefinition);
            this.rsEditor.TextFormatDefinitions.AddLast(XmlSyntaxHighlightingHelper.XmlTag, XmlSyntaxHighlightingHelper.XmlTagFormatDefinition);
        }

        private void ClearXmlFormatDefinitions()
        {
            this.rsEditor.TextFormatDefinitions.Remove(XmlSyntaxHighlightingHelper.XmlAttribute);
            this.rsEditor.TextFormatDefinitions.Remove(XmlSyntaxHighlightingHelper.XmlElement);
            this.rsEditor.TextFormatDefinitions.Remove(XmlSyntaxHighlightingHelper.XmlComment);
            this.rsEditor.TextFormatDefinitions.Remove(XmlSyntaxHighlightingHelper.XmlContent);
            this.rsEditor.TextFormatDefinitions.Remove(XmlSyntaxHighlightingHelper.XmlString);
            this.rsEditor.TextFormatDefinitions.Remove(XmlSyntaxHighlightingHelper.XmlTag);
        }
        #endregion Colores palabras clave
        #region taggers
        /*
        private class TorreControlTagger : WordTaggerBase
        {
            private static readonly string[] Keywords = new string[]
            {
        "False", "None", "True", "and", "as", "assert","break", "class",
        "continue", "def", "del", "elif", "else", "except", "for", "from",
        "global", "if", "import", "in", "is", "lambda", "nonlocal", "not",
        "or", "pass", "raise", "finally", "return", "try", "while", "with", "yield",
        "nombre", "tipoGrafica", "sql", "reglas", "evaluar", 
        "select", "from", "where", "group by"
            };

            private static readonly string[] Comments = new string[]
            {
                    "#"
            };

            private static readonly string[] Operators = new string[]
            {
                    "+", "-",  "*", "/"
            };
            
            private static readonly Dictionary<string, ClassificationType> WordsToClassificationType = new Dictionary<string, ClassificationType>();

            static TorreControlTagger()
            {
                WordsToClassificationType = new Dictionary<string, ClassificationType>();

                foreach (var keyword in Keywords)
                {
                    WordsToClassificationType.Add(keyword, ClassificationTypes.Keyword);
                }

                foreach (var preprocessor in Operators)
                {
                    WordsToClassificationType.Add(preprocessor, ClassificationTypes.Operator);
                }

                foreach (var comment in Comments)
                {
                    WordsToClassificationType.Add(comment, ClassificationTypes.Comment);
                }

                
            }
            
            public TorreControlTagger(Telerik.Windows.Controls.RadSyntaxEditor editor)
              : base(editor)
            {
            }
            
            protected override Dictionary<string, ClassificationType> GetWordsToClassificationTypes()
            {
                return TorreControlTagger.WordsToClassificationType;
            }

            protected override bool TryGetClassificationType(string word, out ClassificationType classificationType)
            {
                int number;

                if (int.TryParse(word, out number))
                {
                    classificationType = ClassificationTypes.NumberLiteral;
                    return true;
                }

                return base.TryGetClassificationType(word, out classificationType);
            }

            protected override IList<string> SplitIntoWords(string value)
            {
                List<string> words = new List<string>();
                string word;
                int lastCharType = -1;
                int startIndex = 0;
                for (int i = 0; i < value.Length; i++)
                {
                    int charType = GetCharType(value[i]);
                    if (charType != lastCharType)
                    {
                        word = value.Substring(startIndex, i - startIndex);
                        words.Add(word);
                        startIndex = i;
                        lastCharType = charType;
                    }
                }

                word = value.Substring(startIndex, value.Length - startIndex);
                words.Add(word);

                return words;
            }

            internal static int GetCharType(char c)
            {
                if (c == '#' || c == '_')
                {
                    return 0;
                }

                if (char.IsWhiteSpace(c))
                {
                    return 1;
                }

                if (char.IsPunctuation(c) || char.IsSymbol(c))
                {
                    return 2;
                }

                return 0;
            }
        }*/
        public static class JsonTypes
        {
            static JsonTypes()
            {
                Key = new ClassificationType("Key");
                Number = new ClassificationType("Number");
                TrueFalseNull = new ClassificationType("TrueFalseNull");
                StringLiteral = new ClassificationType("StringLiteral");
            }

            public static ClassificationType Key
            {
                get;
                private set;
            }

            public static ClassificationType Number
            {
                get;
                private set;
            }

            public static ClassificationType TrueFalseNull
            {
                get;
                private set;
            }

            public static ClassificationType StringLiteral
            {
                get;
                private set;
            }
        }
        public class JsonTagger : TaggerBase<ClassificationTag>
        {
            public JsonTagger(ITextDocumentEditor editor)
                : base(editor)
            {
            }

            public override IEnumerable<TagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
            {
                string[] regexGroups = new string[] { "torreControl" , "key", "stringLiteral", "number", "TFN"};

                foreach (var span in spans)
                {
                    string spanText = span.GetText();
                    string stringToMatch = PrepareRegexString();

                    Regex regularExpression = new Regex(stringToMatch, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                    MatchCollection matches = regularExpression.Matches(spanText);

                    foreach (Match match in matches)
                    {
                        foreach (var groupName in regexGroups)
                        {
                            if (match.Groups[groupName].Success)
                            {
                                int start = span.Start + match.Index;
                                TagSpan<ClassificationTag> tagSpan = CreateTagSpan(this.Document.CurrentSnapshot, start, match.Length, groupName);

                                yield return tagSpan;
                            }
                        }
                    }
                }

                yield break;
            }

            private static string PrepareRegexString()
            {
                string keys = @"(?<key>\s*""([^\""\n]|\.)*""?\s*)(?=:)";
                string stringLiterals = @"(?<stringLiteral>(?<=:)\s*""([^\""\n]|\.)*""?\s*)";
                string numbers = @"(?<number>(-?)(0|([1-9][0-9]*))(\.[0-9]+)?)";
                string trueFalseNulls = @"(?<TFN>(true|false|null))";
                

                string[] xamlClassifications = new string[] {  numbers, trueFalseNulls, stringLiterals, keys };

                StringBuilder builder = new StringBuilder();

                builder.Append(@"\s*");
                for (int i = 0; i < xamlClassifications.Count() - 1; i++)
                {
                    builder.AppendFormat("{0}|", xamlClassifications[i]);
                }
                builder.AppendFormat("{0}", xamlClassifications[xamlClassifications.Count() - 1]);
                builder.Append(@"\s*");

                return builder.ToString();
            }

            private static TagSpan<ClassificationTag> CreateTagSpan(TextSnapshot snapshot, int start, int len, string groupName)
            {
                var textSnapshotSpan = new TextSnapshotSpan(snapshot, new Telerik.WinForms.SyntaxEditor.Core.Text.Span(start, len));
                var classificationType = GetJsonClassificationType(groupName);
                var classificationTag = new ClassificationTag(classificationType);
                var tagSpan = new TagSpan<ClassificationTag>(textSnapshotSpan, classificationTag);

                return tagSpan;
            }

            private static ClassificationType GetJsonClassificationType(string typestring)
            {
                if (typestring == "key")
                {
                    return JsonTypes.Key;
                }
                else if (typestring == "stringLiteral")
                {
                    return JsonTypes.StringLiteral;
                }
                else if (typestring == "number")
                {
                    return JsonTypes.Number;
                }
                else if (typestring == "TFN")
                {
                    return JsonTypes.TrueFalseNull;
                }
                return null;
            }
        }
        #endregion taggers
    }



}
