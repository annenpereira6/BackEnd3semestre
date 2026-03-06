using FilmesMoura1.WebAPI.BdContextFilme;
using FilmesMoura1.WebAPI.Interfaces;
using FilmesMoura1.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados (exemplo com SQL Server
builder.Services.AddDbContext<FilmeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o repositório ao container de injeção de dependência
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddAuthentication(options =>

{
    options.DefaultChallengeScheme = "JwtBearer";
    options.DefaultAuthenticateScheme = "JwtBearer";

})
    .AddJwtBearer("JwtBearer", options =>
    {
        options.TokenValidationParameters = new
        Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("filmes-chave-autenticacao--webapi-dev")),

            ClockSkew = TimeSpan.FromMinutes(5),

            ValidIssuer = "api_filmes",

            ValidAudience = "api_filmes"
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "v1",
        Title = "Filmes API",
        Description = "Uma API com um catalogo de filmes",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "annedev",
            Url = new Uri("https://github.com/annedev")
        },
        License = new Microsoft.OpenApi.OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT:"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = Array.Empty<string>().ToList()
    });
});

// Adiciona serviço de Controllers
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger(options => { });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("CorsPolicy");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
