using System.Data;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Taller.Presentacion.Formularios.Administrador
{
    public partial class usuario_crud : Form
    {
        private DataTable usersTable;
        public usuario_crud(DataTable tablaUsuarios)
        {
            InitializeComponent();
            this.usersTable = tablaUsuarios;            
        }
        private void InicializarTabla()
        {
            usersTable.Columns.Add("Nombre");
            usersTable.Columns.Add("Apellido");
            usersTable.Columns.Add("Usuario");
            usersTable.Columns.Add("Contraseña");
            usersTable.Columns.Add("Rol");

            tabla_usuario.DataSource = usersTable;
        }       

        private void crudUusario_Click(object sender, EventArgs e)
        {

        }

        private void text_contraseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void combo_rol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void usuario_crud_Load(object sender, EventArgs e)
        {
            combo_rol.Items.Clear();
            combo_rol.Items.Add("Administrador");
            combo_rol.Items.Add("Administrativo");
            combo_rol.Items.Add("Técnico");
        }
        //resticcion de que solo acepta letras y espacios
        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
        }

        private void guardar_Click(object sender, EventArgs e)
        {
            // Validar datos
            if (string.IsNullOrWhiteSpace(text_nombre.Text) ||
                string.IsNullOrWhiteSpace(text_apellido.Text) ||
                string.IsNullOrWhiteSpace(text_usuario.Text) ||
                string.IsNullOrWhiteSpace(text_contraseña.Text) ||
                combo_rol.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string perfilSeleccionado;
            try
            {
                perfilSeleccionado = combo_rol.SelectedItem?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al acceder al perfil seleccionado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidName(text_nombre.Text) || !IsValidName(text_apellido.Text))
            {
                MessageBox.Show("Nombre y Apellido solo deben contener letras.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Agregar el nuevo usuario a la tabla
            DataRow row = usersTable.NewRow();
            row["Nombre"] = text_nombre.Text;
            row["Apellido"] = text_apellido.Text;
            row["Rol"] = perfilSeleccionado;
            row["Usuario"] = text_usuario.Text;
            row["Contraseña"] = text_usuario.Text;

            usersTable.Rows.Add(row);

            MessageBox.Show("Usuario guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }

        private void ClearForm()
        {
            text_nombre.Clear();
            text_apellido.Clear();
            text_usuario.Clear();
            text_contraseña.Clear();
            combo_rol.SelectedIndex = -1;
        }
    }
}
