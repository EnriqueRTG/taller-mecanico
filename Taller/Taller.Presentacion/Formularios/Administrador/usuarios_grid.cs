using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Taller.Presentacion.Formularios.Administrador
{
    public partial class usuarios_grid : Form
    {
        private DataTable usersTable;
        public usuarios_grid(DataTable tablaUsuarios)
        {
            InitializeComponent();
            this.usersTable = tablaUsuarios;
        }

        private void crudUusario_Click(object sender, EventArgs e)
        {

        }

        private void usuario_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
