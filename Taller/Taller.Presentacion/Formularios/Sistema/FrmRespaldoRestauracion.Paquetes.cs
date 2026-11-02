namespace Taller.Presentacion.Formularios.Sistema;
public partial class FrmRespaldoRestauracion
{
    private TabPage tabPaquetes = null!;
    private DataGridView dgvPaquetes = null!;
    private Button btnExportarPaquete = null!;
    private void PrepararPaquetes()
    {
        tabPaquetes = new TabPage("Google Drive") { Name = "tabPaquetes", AutoScroll = true, Font = Font, BackColor = Color.White };
        tabs.TabPages.Add(tabPaquetes);
        var panel = Columna(); panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        var ayuda = Ayuda("EXPORTAR: seleccione una copia de la tabla y guarde su paquete en SGTM-Respaldos dentro de Google Drive. Incluye la completa base si es diferencial. Verificamos el paquete; Drive realiza la subida. Compruebe la sincronización en Drive.\n\nRECUPERAR: descargue y extraiga la carpeta completa, impórtela a una carpeta local con permisos SQL y continúe en Restaurar. Importar prepara la copia; no reemplaza los datos. Para una instalación perdida se requiere recuperación técnica separada.");
        dgvPaquetes = GrillaCopias("dgvPaquetes");
        btnExportarPaquete = Boton("Exportar copia a Drive…", (_, _) => { }, true);
        var exportar = btnExportarPaquete;
        var importar = Boton("Importar paquete descargado…", (_, _) => { });
        var botones = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = false };
        botones.Controls.AddRange([exportar, importar]); panel.Controls.Add(ayuda, 0, 0); panel.Controls.Add(dgvPaquetes, 0, 1); panel.Controls.Add(botones, 0, 2);
        tabPaquetes.Controls.Add(panel);
        dgvPaquetes.SelectionChanged += (_, _) => { if (!_configurando) ActualizarAcciones(); };
        exportar.Click += async (_, _) =>
        {
            if (OperacionEnCurso) return;
            if (dgvPaquetes.SelectedRows.Count != 1 || dgvPaquetes.SelectedRows[0].DataBoundItem is not Taller.Aplicacion.DTOs.Respaldos.CopiaRespaldoDto copia)
            {
                MessageBox.Show(this, "Seleccione una copia en esta tabla.", "Exportar respaldo");
                return;
            }
            using var carpeta = new FolderBrowserDialog { Description = "Seleccione SGTM-Respaldos dentro de Google Drive", UseDescriptionForTitle = true };
            if (carpeta.ShowDialog(this) != DialogResult.OK) return;
            await EjecutarAsync(async () =>
            {
                var ruta = await _servicio.ExportarPaqueteAsync(copia.Id, carpeta.SelectedPath);
                await CargarDatosAsync();
                MessageBox.Show(this, "Paquete generado y verificado:\n" + ruta + "\n\nEspere a que Drive indique que la sincronización terminó. SGTM no verifica la subida a Internet.", "Paquete exportado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        };
        importar.Click += async (_, _) =>
        {
            if (OperacionEnCurso) return;
            using var origen = new FolderBrowserDialog { Description = "Seleccione la carpeta SGTM del paquete descargado (contiene sgtm-respaldo.json)", UseDescriptionForTitle = true };
            if (origen.ShowDialog(this) != DialogResult.OK) return;
            using var destino = new FolderBrowserDialog { Description = "Seleccione la carpeta LOCAL de recuperación con permisos para SQL Server", UseDescriptionForTitle = true };
            if (destino.ShowDialog(this) != DialogResult.OK) return;
            await EjecutarAsync(async () =>
            {
                var id = await _servicio.ImportarPaqueteAsync(origen.SelectedPath, destino.SelectedPath);
                await CargarDatosAsync();
                tabs.SelectedTab = tabRestaurar;
                foreach (DataGridViewRow fila in dgvRestaurar.Rows)
                    if (fila.DataBoundItem is Taller.Aplicacion.DTOs.Respaldos.CopiaRespaldoDto c && c.Id == id) { dgvRestaurar.CurrentCell = fila.Cells[0]; fila.Selected = true; break; }
                MessageBox.Show(this, "Paquete importado y verificado. Los datos activos no se modificaron. Vaya a Restaurar para ensayar la copia y, si corresponde, autorizar la recuperación.", "Paquete recuperado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        };
    }
}
