using Blazor_Quiniegol.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

PdfSharp.Fonts.GlobalFontSettings.UseWindowsFontsUnderWindows = true;

var builder = WebApplication.CreateBuilder(args);

RutaDatosService.Configurar(builder.Environment.ContentRootPath);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
    });

builder.Services.AddScoped<DatosPronosticosService>();
builder.Services.AddScoped<DatosMundialService>();
builder.Services.AddScoped<PartidoAdministracionService>();
builder.Services.AddScoped<ReporteWebService>();
builder.Services.AddScoped<SesionUsuarioService>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    provider => provider.GetRequiredService<SesionUsuarioService>());
builder.Services.AddSingleton<FechaSimuladaService>();
builder.Services.AddScoped(
    _ => new JsonRepository<Usuario>(
        RutaDatosService.ObtenerRuta("usuarios.json")));
builder.Services.AddScoped<UsuarioController>();
builder.Services.AddScoped<LoginController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
