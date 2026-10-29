# Diagramas de la versión actual

- [DER editable](DER-SGTM-v2.0.drawio): ocho vistas del mismo esquema físico, con PK/FK, nulabilidad y registro de 43 relaciones; control separado.
- [Casos de uso, flujo, estados, arquitectura y clases](CU-Flujos-Arquitectura-v2.0.drawio): ocho páginas editables. CU01–CU10 coinciden con ERS/Apéndice A. Las tres vistas por rol repiten casos compartidos, no crean casos nuevos.

| Vista | Contenido |
|---|---|
| [1](vista-1.svg) | Casos de uso del administrador |
| [2](vista-2.svg) | Casos de uso del administrativo |
| [3](vista-3.svg) | Casos de uso del técnico |
| [4](vista-4.svg) | Circuito principal de recepción a entrega |
| [5](vista-5.svg) | Selección parcial exacta y reformulación |
| [6](vista-6.svg) | Estados de atención y transiciones principales |
| [7](vista-7.svg) | Dependencias entre capas y herramientas |
| [8](vista-8.svg) | Clases principales de dominio y relaciones abreviadas |

La vista de clases es una síntesis de diseño, no el inventario completo de atributos. Ese inventario está en el DER y diccionario. Los flujos no son FK; las referencias de proyectos no representan herencia de clases. Los casos de uso no usan include para expresar una mera secuencia temporal.

[Requisitos y trazabilidad](../especificacion/trazabilidad.md) · [Modelo relacional](../arquitectura/modelo-relacional.md)
