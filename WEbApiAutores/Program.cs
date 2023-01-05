using WEbApiAutores;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup( builder.Configuration );

startup.ConfigureService(builder.Services);

var app = builder.Build();

//De esta forma se tiene el servicio ILogger
var servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

startup.Configure(app, app.Environment, servicioLogger);


app.Run();
