using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumboSGA.Presentation
{
    public partial class SFConfiguradorUbicacion : SFDetalles
    {
        List<CheckBoxNameValue> lstCheckBoxNameValues;
        public SFConfiguradorUbicacion(string nombre, List<TableScheme> _lstEsquemasParam, dynamic _valuesParam, modoForm _modoAperturaParam, GridScheme _esquemGridParam = null, Hashtable _diccParamNuevoParam = null)
            : base(nombre, _lstEsquemasParam, (object)_valuesParam, _modoAperturaParam, _esquemGridParam, _diccParamNuevoParam)
        {

        }
        protected override void CargarDatos()
        {
            CheckBoxNameValue CRUDCntrl;
            lstCheckBoxNameValues = new List<CheckBoxNameValue>();

            int filas = 0;
            int columnas = 0;
            TamañoTabla(ref filas, ref columnas, _lstEsquemas);


            TablaPanelContenido = new TableLayoutPanel();
            TablaPanelContenido.ColumnCount = columnas;

            //int AnchoTotal = dataDialog1.PanelControles.Width;
            int AnchoTotal = Width;

            for (int i = 0; i < columnas; i++)
            {
                TablaPanelContenido.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, AnchoTotal / columnas));
            }

            int rowPosition = 0;
            int altoFilaDefecto = 30;
            int altoTotal = 0;
            bool bolNuevaFila = false;
            List<RowStyle> arrayFilas = new List<RowStyle>();
            TablaPanelContenido.RowCount += filas;
            if (filas > 0)
            {
                //rowPosition = tablaPanelControles.RowCount;
                bolNuevaFila = true;
                for (int a = 0; a < filas; a++)
                {
                    TablaPanelContenido.RowCount += 1;
                    float mayorAlto = GetMayorAltoFila(_lstEsquemas, a);
                    //tablaPanelControles.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

                    arrayFilas.Add(new RowStyle(SizeType.AutoSize, mayorAlto));
                }


                for (int i = 0; i < _lstEsquemas.Count; i++)
                {


                    if (_lstEsquemas[i].Fila > rowPosition)
                    {
                        bolNuevaFila = true;
                    }

                    if (bolNuevaFila)
                    {
                        for (int j = rowPosition; j < filas /*_lstEsquemas[i].Fila*/; j++)
                        {
                            int mayorAlto = GetMayorAltoFila(_lstEsquemas, rowPosition);
                            if (arrayFilas[rowPosition].Height != 0)
                            {
                                TablaPanelContenido.RowStyles.Add(arrayFilas[j]);

                                altoTotal += (int)arrayFilas[rowPosition].Height;
                                rowPosition += 1;
                            }
                            else
                            {
                                TablaPanelContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, altoFilaDefecto));
                                altoTotal += altoFilaDefecto;
                                rowPosition += 1;
                            }

                            bolNuevaFila = false;
                        }
                    }

                    string valor = string.Empty;

                    try
                    {
                        if (_values != null)
                        {
                            var z = JsonConvert.DeserializeObject(Convert.ToString(_values));
                            var pn = (string)z[_lstEsquemas[i].Nombre];
                            if (pn != null) //César: causa un error throwable
                                valor = pn.ToString();
                        }
                    }
                    catch (System.NullReferenceException ex)
                    {
                    }
                    catch (Exception e)
                    {
                        ExceptionManager.GestionarError(e);

                    }

                    switch (ModoAperturaForm)
                    {
                        case modoForm.clonar:
                            if (_lstEsquemas[i].EsPK)
                            {
                                //valor = string.Empty;
                            }
                            if (_diccParamNuevo.Count > 0 && _diccParamNuevo.ContainsKey(_lstEsquemas[i].Nombre))
                            {
                                if (_diccParamNuevo[_lstEsquemas[i].Nombre].GetType() == typeof(Hashtable))
                                {
                                    Hashtable diccAux = (Hashtable)_diccParamNuevo[_lstEsquemas[i].Nombre];

                                    int _valorMayor = Business.GetMayorElemento(diccAux["CampoMostrado"].ToString(), diccAux["TablaRelacionada"].ToString(), diccAux["filtro"].ToString());

                                    if (!string.IsNullOrEmpty(diccAux["ValorDefecto"].ToString()))
                                    {
                                        _valorMayor += Convert.ToInt32(diccAux["ValorDefecto"].ToString());
                                    }

                                    valor = _valorMayor.ToString();
                                }
                                else
                                {
                                    valor = _diccParamNuevo[_lstEsquemas[i].Nombre].ToString();
                                }
                            }
                            break;

                        case modoForm.nuevo:

                            valor = string.Empty;

                            if (!string.IsNullOrEmpty(_lstEsquemas[i].ValorDefecto))
                            {
                                valor = _lstEsquemas[i].ValorDefecto;
                            }
                            if (_diccParamNuevo.Count > 0 && _diccParamNuevo.ContainsKey(_lstEsquemas[i].Nombre))
                            {
                                //valor = _diccParamNuevo[_lstEsquemas[i].Nombre].ToString();
                                if (_diccParamNuevo[_lstEsquemas[i].Nombre].GetType() == typeof(Hashtable))
                                {
                                    Hashtable diccAux = (Hashtable)_diccParamNuevo[_lstEsquemas[i].Nombre];

                                    int _valorMayor = Business.GetMayorElemento(diccAux["CampoMostrado"].ToString(), diccAux["TablaRelacionada"].ToString(), diccAux["filtro"].ToString());

                                    if (!string.IsNullOrEmpty(diccAux["ValorDefecto"].ToString()))
                                    {
                                        _valorMayor += Convert.ToInt32(diccAux["ValorDefecto"].ToString());
                                    }

                                    valor = _valorMayor.ToString();
                                }
                                else
                                {
                                    valor = _diccParamNuevo[_lstEsquemas[i].Nombre].ToString();
                                }
                            }
                            break;
                    }

                    CRUDCntrl = new CheckBoxNameValue(_lstEsquemas[i], valor);

                    if (_esquemGrid != null)
                    {
                        if (_esquemGrid.Filtro == _lstEsquemas[i].Nombre)
                        {
                            _esquemGrid.ValorFiltro = valor;
                        }
                        //César: _esquemGrid controla el GridAdjunto en parte
                        _esquemGrid.SetValorClave(_lstEsquemas[i].Nombre, valor);
                    }

                    TablaPanelContenido.Controls.Add(CRUDCntrl, _lstEsquemas[i].Columna, _lstEsquemas[i].Fila);
                    lstCheckBoxNameValues.Add(CRUDCntrl);
                }

                _filasTotalesTabla = TablaPanelContenido.RowCount; //Guardar número de filas para la devolución del JSON y para la Edición
                                                                   //tablaPanelControles.RowCount = tableLayoutPanel1.RowStyles.Count;


                TablaPanelContenido.Size = new System.Drawing.Size(Width - SystemInformation.VerticalScrollBarWidth - 30, /*ClientSize.Height*/ altoTotal);
                TablaPanelContenido.AutoSize = true;
                base.dataDialog1.PanelControles.AutoScroll = true;
                if (dataDialog1.PanelControles.AutoScrollMargin.Width < 5 ||
                    dataDialog1.PanelControles.AutoScrollMargin.Height < 5)
                {
                    dataDialog1.PanelControles.Invoke((MethodInvoker)delegate { dataDialog1.PanelControles.SetAutoScrollMargin(5, 5); });

                }

                if (dataDialog1.PanelControles.InvokeRequired)
                {
                    dataDialog1.PanelControles.Invoke((MethodInvoker)delegate {
                        dataDialog1.PanelControles.Controls.Add(TablaPanelContenido);
                    });

                }
                else
                {
                    dataDialog1.PanelControles.Controls.Add(TablaPanelContenido);
                }

                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { HabilitarDeshabilitarControles(ModoAperturaForm); });
                }
                else
                {
                    HabilitarDeshabilitarControles(ModoAperturaForm);
                }

            }

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { CargaDatosGridAdjunto(); });
            }
            else
            {
                CargaDatosGridAdjunto();
            }
            //Probar cambiar de estilo aquí
        }

        protected override string ComponerJSON()
        {
            var jsonObject = new JObject();

            if (this.dataDialog1.PanelControles.Controls[0] != null)
            {
                foreach (CheckBoxNameValue par in lstCheckBoxNameValues)
                {
                    if (par.checkBoxLineaEditada.Checked)
                    {
                        NameValue.SacarValorALinea(ref jsonObject, par);
                    }
                }
            }

            if (ModoAperturaForm == modoForm.clonar)
            {
                //ComponerJSONGrid(ref jsonObject);
            }
            Debug.WriteLine(jsonObject.ToString());
            return jsonObject.ToString();
        }
    }
}
