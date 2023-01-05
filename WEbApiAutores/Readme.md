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