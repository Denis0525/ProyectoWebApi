using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PruebaApi.Data;
using PruebaApi.Modelos;
using PruebaApi.PeliculasMapper;
using PruebaApi.Repositorios;
using PruebaApi.Repositorios.IRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Asp.Versioning.Builder;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
//Inyeccion de depencia para la migracion..
builder.Services.AddDbContext<AplicatioDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'CategoriaDbContext' not found.")));
builder.Services.AddControllers(Options =>
{// cahce profile un cache global no se ponde en cada controller o en cada parte del codigo
Options.CacheProfiles.Add("Pordefecto20Segundos", new CacheProfile(){Duration = 30});
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//sirve para la autontication configurar Swagger para aguntar el builder asp net core
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description =
  "Autenticacion JWT usando el esquema Bearer. \r\n\r\n" +
  "Ingresa la palabra 'Bearer' seguido de un [espacio] y despues de un token en el campo de abajo. \r\n\r\n" +
  "Ejemplo: \"Bearer thifkjgiggigigi\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Scheme = "Bearer"
  });
  options.AddSecurityRequirement(new OpenApiSecurityRequirement()
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id ="Bearer"
        },
        Scheme ="oauth2",
       Name ="Bearer",
       In = ParameterLocation.Header
        },
        new List<string>()
        }
      });
       options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1.0",
            Title = "Pelicula API",
            Description = "API pelicula version1",
            TermsOfService = new Uri("https://render2web.com/promociones"),
            Contact = new OpenApiContact
            {
                Name = "render2web",
                Url = new("https://render2web.com/promociones"),
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://render2web.com/promociones"),
            },
  });
    options.SwaggerDoc(
        "v2",
        new OpenApiInfo
        {
            Version = "v2.0",
            Title = "Peliculas Api V2",
            Description = "Pelicula version 2",
            TermsOfService = new Uri("https://render2web.com/promociones"),
            Contact = new OpenApiContact
            {
                Name = "render2web",
                Url = new Uri("https://render2web.com/promociones"),
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://render2web.com/promociones"),
            },
        }
    );
});
//Soporte para CORS
//Usamos el ejemplo del dominio :http://localhost:3223, se debe cambiar por el correcto
// se en el caso si quremos quese publico el dominio se usa para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PoliticasCors", build =>
{
  build.WithOrigins("http://localhost:3223").AllowAnyMethod().AllowAnyHeader();
}));

//soporte para autentication con .NET Indentity
builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<AplicatioDbContext>();



//Soporte para cache
var AddApiVersioning = builder.Services.AddApiVersioning( opcion =>
{
   opcion.AssumeDefaultVersionWhenUnspecified = true;
   opcion.DefaultApiVersion = new ApiVersion(1,0);
   opcion.ReportApiVersions = true;
  //  opcion.ApiVersionReader = ApiVersionReader.Combine(new  QueryStringApiVersionReader("api-version")
   });
// });

AddApiVersioning.AddApiExplorer(opciones=>
{
      opciones.GroupNameFormat = "'v'VVV";
      opciones.SubstituteApiVersionInUrl = true;
});
//Se agregan los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();


//soporte para el versionamiento
builder.Services.AddApiVersioning();

//Agregamos AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");
// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,  
        ValidateAudience = false 
    };
});
var app = builder.Build();
// var key = builder.Configuration.GetValue<string>("ApiSettings: Secreta");
// //aqui se configura la Autentication
// builder.Services.AddAuthentication(options =>
//     {
//       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//         {
//           options.RequireHttpsMetadata = false;
//           options.SaveToken = true;
//           options.TokenValidationParameters = new TokenValidationParameters
//           {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
//             ValidateIssuer = false,
//             ValidateAudience = false
//           };
//         });
// var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(opciones =>
  {
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json","ApiPeliculasV1");
    opciones.SwaggerEndpoint("/swagger/v2/swagger.json","ApiPeliculasV2");
  });
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();  // ¡Importante que esté antes de Cors, Auth y MapControllers!

app.UseCors("PoliticasCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
