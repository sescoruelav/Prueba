using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas.Ventanas;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen.RumControlPersonalizado
{
    public class RumControlOperarios : RumControlGeneral
    {
        private string idUsuario = "";
        private string nombreUsuario = "";
        private string idOperario = "";

        //string Estado = "";
        private RadColorDialog dialog = new RadColorDialog();

        public RumControlOperarios(string nombreJson, RadRibbonBar barra) : base(nombreJson)
        {
            BotonesOperario(barra); //Funcion de añadir los nuevos botones en SFPrincipal
            colorEstadoOperario();
            GridView.CellFormatting += colorEstadoOperario;
        }

        private void BotonesOperario(RadRibbonBar barra)
        {
            //Hereda de RadRibbon Bar para agregar los nuevos botones
            RadRibbonBarGroup radRibbonBarGroup1 = new RadRibbonBarGroup();
            radRibbonBarGroup1.Text = Lenguaje.traduce("Operario");
            ((RibbonTab)barra.CommandTabs[0]).Items.Insert(3, radRibbonBarGroup1);
            //((RibbonTab)barra.CommandTabs[0]).Items.Add(radRibbonBarGroup1);

            // Añadir Grupo
            RadButtonElement addGroup = new RadButtonElement();
            addGroup.Text = Lenguaje.traduce("Añadir Grupo");
            addGroup.Click += AddGroup_OnClick;
            addGroup.Image = new Bitmap(Resources.add_group);
            addGroup.ImageAlignment = ContentAlignment.MiddleCenter;
            addGroup.DisplayStyle = DisplayStyle.ImageAndText;
            addGroup.TextImageRelation = TextImageRelation.ImageAboveText;
            addGroup.TextAlignment = ContentAlignment.BottomCenter;
            radRibbonBarGroup1.Items.Add(addGroup);

            //Añadir Usuario
            RadButtonElement addUser = new RadButtonElement();
            addUser.Text = Lenguaje.traduce("Añadir Usuario");
            addUser.Click += AddUser_OnClick;
            addUser.Image = new Bitmap(Resources.add_user);
            addUser.ImageAlignment = ContentAlignment.MiddleCenter;
            addUser.DisplayStyle = DisplayStyle.ImageAndText;
            addUser.TextImageRelation = TextImageRelation.ImageAboveText;
            addUser.TextAlignment = ContentAlignment.BottomCenter;
            radRibbonBarGroup1.Items.Add(addUser);

            //Cambiar Clave
            RadButtonElement changePassword = new RadButtonElement();
            changePassword.Text = Lenguaje.traduce("Cambiar Clave");
            changePassword.Click += ChangePassword_OnClick;
            changePassword.Image = new Bitmap(Resources.password);
            changePassword.ImageAlignment = ContentAlignment.MiddleCenter;
            changePassword.DisplayStyle = DisplayStyle.ImageAndText;
            changePassword.TextImageRelation = TextImageRelation.ImageAboveText;
            changePassword.TextAlignment = ContentAlignment.BottomCenter;
            radRibbonBarGroup1.Items.Add(changePassword);

            //Cambiar Grupo
            RadButtonElement changeGroup = new RadButtonElement();
            changeGroup.Text = Lenguaje.traduce("Cambiar Grupo");
            changeGroup.Click += ChangeGroup_OnClick;
            changeGroup.Image = new Bitmap(Resources.cambiar_grupo);
            changeGroup.ImageAlignment = ContentAlignment.MiddleCenter;
            changeGroup.DisplayStyle = DisplayStyle.ImageAndText;
            changeGroup.TextImageRelation = TextImageRelation.ImageAboveText;
            changeGroup.TextAlignment = ContentAlignment.BottomCenter;
            radRibbonBarGroup1.Items.Add(changeGroup);

            //Asignar usuario
            RadButtonElement asignarUsuario = new RadButtonElement();
            asignarUsuario.Text = Lenguaje.traduce("Asignar usuario");
            asignarUsuario.Click += AddRelacion_OnClick;
            asignarUsuario.Image = new Bitmap(Resources.add_relacion);
            asignarUsuario.ImageAlignment = ContentAlignment.MiddleCenter;
            asignarUsuario.DisplayStyle = DisplayStyle.ImageAndText;
            asignarUsuario.TextImageRelation = TextImageRelation.ImageAboveText;
            asignarUsuario.TextAlignment = ContentAlignment.BottomCenter;
            radRibbonBarGroup1.Items.Add(asignarUsuario);
        }

        // CLIC sobre una fila
        private void RadGridView_Click(object sender, EventArgs e)
        {
        }

        // Botón para añadir grupo
        private void AddGroup_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Ejecuta función para agregar grupo
                AgregarGrupo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar" + ex);
            }
        }

        //Botón para añadir usuario
        private void AddUser_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Ejecuta Función para agregar a un usuario
                AgregarUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar" + ex);
            }
        }

        //Botón para añadir usuario
        private void AddRelacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Ejecuta Función para asignar una relacion
                AsignarrUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar" + ex);
            }
        }

        //Botón para cambiar clave
        private void ChangePassword_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Ejecuta función para cambiar clave
                CambiarPassword();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar" + ex);
            }
        }

        //Botón para cambiar grupo
        private void ChangeGroup_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Llama función que cambia grupo
                CambiarGrupo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar" + ex);
            }
        }

        public void AgregarGrupo()
        {
            string idOperario;
            GridView.CurrentColumn = GridView.Columns[0];
            idOperario = GridView.CurrentCell.Value.ToString();
            AgregarGrupoOperario AgregarGrupo = new AgregarGrupoOperario(idOperario);
            AgregarGrupo.ShowDialog();
        }

        public void AgregarUsuario()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("operario")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("operario")].Value.ToString();
            }
            AgregarUsuarioOperario AgregarUsuarioOperario = new AgregarUsuarioOperario(idOperario);
            AgregarUsuarioOperario.ShowDialog();
            ElegirGrid();
        }

        public void AsignarrUsuario()
        {
            AsignarUsuarioOperario AsignarUsuarioOperario = new AsignarUsuarioOperario(idOperario, idUsuario);
            AsignarUsuarioOperario.ShowDialog();
            ElegirGrid();
        }

        public void CambiarPassword()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("NumUsuario")].Value != null)
            {
                idUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("NumUsuario")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("operario")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("operario")].Value.ToString();
            }
            CambiarPassword CambiarPassword = new CambiarPassword(idUsuario, idOperario);
            CambiarPassword.ShowDialog();
        }

        public void CambiarGrupo()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Usuario")].Value != null)
            {
                idUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("Usuario")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("operario")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("operario")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre")].Value != null)
            {
                nombreUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre")].Value.ToString();
            }
            if (nombreUsuario != "")
            {
                EditarGrupoOperario EditarGrupoOperario = new EditarGrupoOperario(idUsuario, idOperario, nombreUsuario);
                EditarGrupoOperario.ShowDialog();
                ElegirGrid();
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Por favor, cree un usuario para poder asignar un grupo nuevo al operario seleccionado"));
            }
        }

        private void colorEstadoOperario(object a, EventArgs e)
        {
            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                string colorEstado = GridView.Rows[i].Cells[Lenguaje.traduce("Estado")].Value.ToString();
                int index = GridView.Columns.IndexOf(Lenguaje.traduce("Estado"));
                var cell = GridView.Rows[i].Cells[index];
                if (colorEstado == Lenguaje.traduce("INACTIVO"))
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.GradientStyle = GradientStyles.Solid;
                    cell.Style.BackColor = Color.Red;
                }
                else if (colorEstado == Lenguaje.traduce("ACTIVO"))
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.GradientStyle = GradientStyles.Solid;
                    cell.Style.BackColor = Color.Green;
                }
            }
        }

        private void colorEstadoOperario()
        {
            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                string colorEstado = GridView.Rows[i].Cells[Lenguaje.traduce("Estado")].Value.ToString();
                int index = GridView.Columns.IndexOf(Lenguaje.traduce("Estado"));
                var cell = GridView.Rows[i].Cells[index];
                if (Lenguaje.traduce(colorEstado) == Lenguaje.traduce("INACTIVO"))
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.GradientStyle = GradientStyles.Solid;
                    cell.Style.BackColor = Color.Red;
                }
                else if (Lenguaje.traduce(colorEstado) == Lenguaje.traduce("ACTIVO"))
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.GradientStyle = GradientStyles.Solid;
                    cell.Style.BackColor = Color.Green;
                }
            }
        }
    }
}