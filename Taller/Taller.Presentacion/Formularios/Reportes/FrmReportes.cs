using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Presentacion.Formularios.Usuarios; 

namespace Taller.Presentacion.Formularios.Reportes
{
    public partial class FrmReportes : Form
    {
        private readonly UsuarioServicio _usuarioServicio;
        private List<Usuario> _usuarios = [];
        public FrmReportes(UsuarioServicio usuarioServicio)
        {
            InitializeComponent();
            _usuarioServicio =
            usuarioServicio
            ?? throw new ArgumentNullException(
                nameof(usuarioServicio));
        }

        private void pnlFiltros_Paint(object sender, PaintEventArgs e)
        {

        }

        private void desde_ValueChanged(object sender, EventArgs e)
        {
            desde.MaxDate = DateTime.Today;
            desde.Value = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1
            );
        }

        private void hasta_ValueChanged(object sender, EventArgs e)
        {
            hasta.MaxDate = DateTime.Today;
            hasta.Value = DateTime.Today;
        }

        private void tipo_reporte_SelectedIndexChanged(object? sender, EventArgs e)
        {
            //AplicarFiltros();
        }

        private bool ValidarFechas()
        {
            if (desde.Value.Date > hasta.Value.Date)
            {
                MessageBox.Show(
                    "La fecha desde no puede ser mayor que la fecha hasta.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return false;
            }

            return true;
        }

        private void btn_generar_Click(object sender, EventArgs e)
        {
            if (!ValidarFechas())
            {
                return;
            }
            //GenerarReporte();
        }

        private void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (!ValidarFechas())
            {
                return;
            }
            //GenerarReporte();
        }
    }
}
