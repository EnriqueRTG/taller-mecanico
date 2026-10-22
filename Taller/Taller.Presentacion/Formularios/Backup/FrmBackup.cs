using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Taller.Presentacion.Formularios.Backup
{
    /// <summary>Prototipo conservado para revisión del equipo. No forma parte de la navegación ni de DI.</summary>
    [Obsolete("Prototipo sin conexión. Utilizar FrmRespaldoRestauracion.")]
    public partial class FrmBackup : Form
    {
        public FrmBackup()
        {
            InitializeComponent();
        }

        private void RutaArchivo_TextChanged(object sender, EventArgs e)
        {
        }

        private void Btn_generar_Click(object sender, EventArgs e)
        {
        }
    }
}
