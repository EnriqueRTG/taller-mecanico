namespace Taller.Presentacion.Formularios.Sistema;
public partial class FrmRespaldoRestauracion
{
    private TabPage tabPaquetes = null!;
    private void PrepararPaquetes()
    {
        tabPaquetes = new TabPage("Drive / copia transportable") { Name = "tabPaquetes", AutoScroll = true };
        tabs.TabPages.Add(tabPaquetes);
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(12) };
        var ayuda = new Label { AutoSize = true, MaximumSize = new Size(660, 0), Text = "Google Drive debe estar abierto con su sesión iniciada. Primero cree una copia local en Copias manuales. Selecciónela y exporte el paquete a su carpeta SGTM-Respaldos en Drive. La diferencial incluye su completa base. SGTM verifica el paquete escrito; compruebe en Drive que terminó la sincronización.\n\nPara recuperar: descargue una carpeta de paquete completa desde Drive, impórtela a una carpeta local con permisos SQL y luego ensaye y autorice la restauración. Importar no reemplaza los datos. Esta operación recupera copias de la misma base y versión; reconstruir una instalación perdida requiere recuperación técnica separada." };
        var exportar = new Button { Text = "Exportar copia seleccionada a Drive…", AutoSize = true, Name = "btnExportarPaquete" };
        var importar = new Button { Text = "Recuperar paquete desde Drive…", AutoSize = true, Name = "btnImportarPaquete" };
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(exportar);
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(importar);
        panel.Controls.AddRange([ayuda, exportar, importar]);
        tabPaquetes.Controls.Add(panel);
        exportar.Click += async (_, _) =>
        {
            if (OperacionEnCurso) return;
            if (dgvCopias.SelectedRows.Count != 1 || dgvCopias.SelectedRows[0].DataBoundItem is not Taller.Aplicacion.DTOs.Respaldos.CopiaRespaldoDto copia)
            {
                MessageBox.Show(this, "Seleccione primero una copia en Copias manuales.", "Exportar respaldo");
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
                tabs.SelectedTab = tabCopias;
                foreach (DataGridViewRow fila in dgvCopias.Rows)
                    if (fila.DataBoundItem is Taller.Aplicacion.DTOs.Respaldos.CopiaRespaldoDto c && c.Id == id) { dgvCopias.CurrentCell = fila.Cells[0]; fila.Selected = true; break; }
                MessageBox.Show(this, "Paquete importado y verificado. Los datos activos no se modificaron. Vaya a Restaurar para ensayar la copia y, si corresponde, autorizar la recuperación.", "Paquete recuperado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        };
    }
}
