using HelloWebApplication;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("toto.json", true);
//builder.Services.AddSingleton<IMagicService, MagicImplementation>();  // une seule instance => toujours le même magic number
//builder.Services.AddTransient<IMagicService, MagicImplementation>();  // chaque demande d'instanciation a son magic number
//builder.Services.AddScoped<IMagicService, MagicImplementation>();     // chaque requete http a son instance de magic number
//builder.Services.AddScoped<IMagicService, MagicConfImplementation>();
builder.Services.AddScoped<IMagicService>(sp => new MagicRangeImplementation(888, 999));

var app = builder.Build();


app.UseChronoMiddleware();

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapGet("/", () => "Hello World!"/*.Substring(125,60)*/);

app.Run();
