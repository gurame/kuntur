using Kuntur.Backoffice;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.Cookie.Name = builder.Configuration["Authentication:Schemes:Cookie:Name"];
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
}).AddOpenIdConnect(options =>
{
    options.Authority = builder.Configuration["Authentication:Schemes:OIDC:Authority"];
    options.ClientId = builder.Configuration["Authentication:Schemes:OIDC:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Schemes:OIDC:ClientSecret"];
    options.CallbackPath = builder.Configuration["Authentication:Schemes:OIDC:CallbackPath"];
    options.SignedOutCallbackPath = builder.Configuration["Authentication:Schemes:OIDC:SignedOutCallbackPath"];
    options.ResponseType = builder.Configuration["Authentication:Schemes:OIDC:ResponseType"]!;
    options.Prompt = builder.Configuration["Authentication:Schemes:OIDC:Prompt"];
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.MapInboundClaims = false;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.ClaimActions.MapJsonKey("organization", "organization");
    options.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.UseAuthentication()
   .UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
