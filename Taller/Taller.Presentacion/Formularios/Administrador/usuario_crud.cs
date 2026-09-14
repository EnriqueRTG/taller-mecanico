using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using Taller.Infraestructura.Persistencia;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;

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

            usuarios_grid.DataSource = usersTable;
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
            // Valida los datos
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
            row["Usuario"] = text_usuario.Text;
            row["Contraseña"] = text_usuario.Text;
            row["Nombre"] = text_nombre.Text;
            row["Apellido"] = text_apellido.Text;
            //row["Activo"] = text_apellido.Text;
            row["Rol"] = perfilSeleccionado;            

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

        private void usuario_crud_Click(object sender, EventArgs e)
        {

        }

        private void borrar_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void text_nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void text_apellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void modificar_Click(object sender, EventArgs e)
        {
            // Valida si hay una fila seleccionada antes de hacer alguna modificacion
            if (usuarios_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, selecciona una fila para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow filaSeleccionada = usuarios_grid.SelectedRows[0];
            int idUsuario = Convert.ToInt32(filaSeleccionada.Cells["Id"].Value);

            string nuevoNombre = text_nombre.Text.Trim();
            string nuevoApellido = text_apellido.Text.Trim();
            string nuevoUsuario = text_usuario.Text.Trim();
            string nuevoContraseña = text_contraseña.Text.Trim();

            // se actualiza la base de datos
            //ActualizarUsuarioEnBaseDeDatos(idUsuario, nuevoNombre, nuevoApellido, nuevoUsuario, nuevoContraseña);

            //refrescar la tabla y limpiar los campos
            //activos();
            ClearForm();
            usuarios_grid.ClearSelection();
        }

        private void tabla_usuario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == usuarios_grid.Columns["inhabilitar"].Index && e.RowIndex >= 0)
            {
                // Obtiene el id del usuario desde la fila seleccionada
                int idUsuario = Convert.ToInt32(usuarios_grid.Rows[e.RowIndex].Cells["Id"].Value);

                // Actualizar la base de datos
                UpdateStatusInDatabase(idUsuario, "No activo");

                // Remover la fila del DataGridView
                usuarios_grid.Rows.RemoveAt(e.RowIndex);                  
            }
        }
        private void UpdateStatusInDatabase(int idUsuario, string newState)
        {
            // Actualiza la base de datos
            using (SqlConnection conexion = TallerDbContext.ObtenerConexion())
            {
                conexion.Open();
                using (SqlTransaction transaction = conexion.BeginTransaction())
                {
                    try
                    {
                        string query = "UPDATE usuario SET Activo = @Activo WHERE idUsuario = @Id";

                        using (SqlCommand cmdUpdatePersona = new SqlCommand(query, conexion, transaction))
                        {
                            cmdUpdatePersona.Parameters.AddWithValue("@Id", idUsuario);
                            cmdUpdatePersona.Parameters.AddWithValue("@Activo", newState);
                            cmdUpdatePersona.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("El estado ha sido actualizado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el estado: " + ex.Message);
                    }
                }
            }
        }
    }
}
