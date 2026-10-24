# Identidad, exportación y vista previa de documentos

## Identidad del taller
El logo de la aplicación se incorpora al HTML y a la impresión de reportes, liquidaciones, recibos e informes de intervención. No convierte estos documentos internos en comprobantes fiscales.
En `Taller/Taller.Presentacion/appsettings.Local.json`, conservar la conexión y agregar o combinar esta sección con los datos reales:

```json
{
  "Empresa": {
    "Nombre": "Taller mecánico",
    "Direccion": "",
    "Telefono": "",
    "Correo": "",
    "IdentificacionFiscal": ""
  }
}
```

Los campos vacíos no se imprimen como datos ficticios. Reiniciar la aplicación después de configurarlos. El nombre es de hasta 100 caracteres; dirección 200, teléfono 80, correo 150 e identificación fiscal 80. La identidad se comparte mediante inyección de dependencias. El HTML embebe el logo PNG; puede abrirse sin conexión.

## Nombres
Los reportes proponen `SGTM-Atenciones-recibidas-y-situacion-actual_20261024_105039.html` (o `.csv`). La fecha corresponde a la elaboración del reporte en horario del taller; repetir una exportación del mismo documento conserva el nombre y el diálogo solicita confirmar una sobrescritura. No es un consecutivo fiscal ni una numeración persistente.
Las liquidaciones y recibos incluyen su número y fecha de emisión. El nombre del trabajo de impresión usa el mismo criterio. El controlador de PDF de Windows puede solicitar elegir nuevamente la ubicación o el nombre.

## CSV para Excel
Incluye todas las filas del detalle y agrega contexto por fila: reporte, generación, emisor, alcance, filtros, método, unidad del gráfico e identidad del taller. Conserva importes numéricos y valores ausentes vacíos. No incluye dibujos, logo, tarjetas ni gráficos: para el documento visual completo utilizar HTML o impresión/PDF.
Usa UTF-8 con BOM, separador punto y coma, comillas escapadas y protección contra fórmulas de texto. La primera línea `sep=;` indica el separador a Excel. En otros lectores CSV, omitir esta primera línea al importar. Si Excel no reconoce importes decimales, importar mediante Datos → Desde texto/CSV, separador punto y coma y configuración regional Español (Argentina). No utilizar la coma como separador: también forma parte de nombres y decimales.
Sin resultados, el CSV conserva las cabeceras y no inventa filas.

## Vista previa
La vista incluye Anterior/Siguiente, selector Página, cantidad total, zoom y botón Imprimir/PDF. La rueda del mouse sobre la vista cambia de página. El zoom permite leer el contenido sin alterar lo impreso. La impresión vuelve a generar todas las páginas desde el inicio; los filtros del reporte generado no cambian. Se utiliza el diálogo de selección de impresora antes de imprimir.
Comprobar con un reporte de 100 registros: ir a página 2 y última, volver a primera, cambiar zoom, imprimir el documento completo dos veces y comparar cantidad y contenido. Logo/contacto y pie numerado deben aparecer en cada página; no debe faltar texto en los saltos de página.
