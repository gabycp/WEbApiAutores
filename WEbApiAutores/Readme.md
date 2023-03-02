---Espacio para tomar notas del proyecto--

Logger -> se puede procesar mensajes en la aplicacion y colocar dichos mensajes en algun lugar, ejemplo se puede colocar los mensajes de log
en una consola, colocar en una base de dats, etc., para utilizar esto se utiliza el servicio de Ilogger

Tipos de mensajes de Logger(DEl más critico al de menor categoría)
*Critical
*Error
*Warning
*Information
*Debug
*Trace
Donde se puede hacer la configuracion de cuales mensajes se guardaran en el archivo de appsetting.Development.json

Middleware
Una -tuberia- ed una cadena de procesos conectados de tal forma que la salida de  cada elemento de la cadena es la entrada de la proxima
A cada de uno de los procesos le llamamos --Middleware--
Los middleware se trabajan en el archivo Startup.cs en la parte de configure

Filtros
Los filtros nos ayudan a correr codigo en determinados momentos del ciclo de vida del procesamiento de una peticion HTTP.
Los filtros son utiles cuando tenemos la necesidad de ejecutar una logica en varias acciones de varios controladores y queremos evitar tener
que repetir codigo 
-Filtro de Autorizacion: es la determinacion que un usuario pueda consumir de forma autorizada
-Filtro de Recusos: Se ejecutan despues de la etapa de autorizacion
-Filtro de Accion: se ejecutan justo antes y despues de la ejecucion de una accion.
-Filtro de Excepcion: se ejecutan cuando hubo una excepcion no a trapada en un try catch durante la ejecucion de una accion, durante el binding de un modelo.
-Filtro de Resultado: se ejecutan antes y depues de una action result.

MAnera titpica de aplicar un filtro es a traves de una accion, a nivel de controlador, a nivel global.

El entity Framework core es un ORM, es decir un mapeador de objetos relacionados

--Actualizacion Parcial con Http Patch
Se utiliza el Http Patch para aplicar actualizaciones parciales en un recurso.
¿Como funciona?
El RFC 5789 es el que define el HTTP Patch
Se usa el JSON Patch(RFC 6902) --> indica como debe ser la estructura del cuerpo de la peticion http, que va indicar lo que el cliente quiere aplicar
sobre el recurso, se pueden hacer varias acciones como agregar, remover, reemplazar, copiar y probar.
Obs. Puede funcionar que una actuaizacion parcial, optimiza o eficiencia  recurso porque ya no se debe mandar toda una estructura completa del recurso sino
solo parcial.

--Configuraciones
Las configuraciones son datos externos de nuestra aplicacion que ayudan a nuestra aplicacion a funcionar correctamente.
ejemplo: connetionstring
Proveedores de configuracion --> nos permiten comunicarnos con varios tipos de fuentes externa, ya sea que se quiere comuncar con un JSON,
variables de memoria, argumentos de linea de comando.
IConfiguraciones nos permite acceder a los datos de confguracion

---Consideraciones de Seguridad--

*Existe data que es importante proteger y no tener a simple vista en un archivo de texto plano.
*Un ejemplo es un connection string de produccion.
*Podemos usar variables de ambiente

--Autenticación y Autorización--

La autenticacion 

