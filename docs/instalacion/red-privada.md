# Dos estaciones Windows, una base compartida

## Disposición

Un equipo Windows hospeda SQL Server/Express y puede ejecutar recepción; el otro ejecuta SGTM como técnico. Ambos consultan la misma SGTM_DB_Demo. La base se carga una sola vez en el servidor. localhost en cada notebook apuntaría a dos instalaciones diferentes, no a una base compartida.

1. Elegir el equipo servidor, conectar ambos a una red privada controlada y comprobar que la red no aísle clientes Wi-Fi. Reservar una IP estable para la demostración. No depender del Wi-Fi público de la facultad.
2. En SQL Server Configuration Manager habilitar TCP/IP para la instancia del servidor y configurar un puerto TCP fijo disponible. Reiniciar SQL Server de manera coordinada. No asumir que Express ya escucha remotamente ni que usa siempre 1433.
3. En el firewall del servidor permitir ese puerto solo en perfil privado y limitado a la IP/subred de las estaciones. No abrirlo en el router hacia Internet ni desactivar todo el firewall.
4. En el cliente ejecutar Test-NetConnection IP_DEL_SERVIDOR -Port PUERTO. Si falla, resolver red/puerto antes de probar credenciales.
5. Configurar una identidad SQL autorizada para cada estación. Windows Authentication no funciona automáticamente por tener dos PC con usuarios de nombre parecido. En un grupo de trabajo puede configurarse un login SQL dedicado con permisos acotados; no usar sa ni compartir credenciales de mantenimiento en todas las estaciones. La cuenta interna de SGTM es independiente del login SQL.
6. En ambos perfiles configurar SGTM_CONN hacia Server=tcp:IP_DEL_SERVIDOR,PUERTO;Database=SGTM_DB_Demo;... con la autenticación y cifrado acordados. No es una cadena completa lista para pegar: incorporar la identidad instalada. Para datos reales usar un certificado confiable y TrustServerCertificate=False; la excepción de certificado local queda limitada a Demo.
7. Abrir SGTM en ambos con el mismo commit. Recepción crea una atención nueva y anota número/dominio; técnico usa Actualizar y la busca. Técnico envía propuesta; recepción actualiza, informa y acepta. Técnico actualiza, ejecuta y finaliza; recepción actualiza, liquida, cobra y entrega.
8. Probar dos intentos sobre una misma reserva/versión y dos cobros sobre el mismo saldo. Debe rechazarse la operación desactualizada, sin duplicados ni sobrepago. Una base compartida no implica refresco instantáneo: usar Actualizar; la ejecución sí consulta pausas periódicamente.

## Respaldos compartidos

Los .bak se escriben en el servidor SQL. Una carpeta C:\SGTM\Respaldos del servidor no es la carpeta de la segunda notebook. Instalar un único agente junto a SQL y compartir la misma BaseControl en ambas estaciones. Habilitar coordinación en todos los clientes antes de restaurar. La estación prevista para restauración necesita permisos técnicos separados; no otorgarlos a todos los operadores.

Ensayar previamente con ambas notebooks y llevar una copia externa verificada. La configuración real de red y cuentas debe realizarse en los equipos; el repositorio no puede habilitar su firewall ni comprobar su conectividad por sí solo.
