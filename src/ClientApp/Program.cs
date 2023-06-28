using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FeatureManagement;
using MudBlazor.Services;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using BlazorApp1;
using BlazorApp1.Theming;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddFeatureManagement();

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri("https://localhost:5013/"));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddHttpClient<ITestClient>(nameof(TestClient), (sp, http) =>
{
    http.BaseAddress = new Uri("https://localhost:5013/");
})
.AddTypedClient<ITestClient>((http, sp) => new TestClient(http))
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
.SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
.AddPolicyHandler(GetRetryPolicy());

builder.Services.AddHttpClient<IUsersClient>(nameof(UsersClient), (sp, http) =>
{
    http.BaseAddress = new Uri("https://localhost:5013/");
})
.AddTypedClient<IUsersClient>((http, sp) => new UsersClient(http))
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
//.SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
//.AddPolicyHandler(GetRetryPolicy());

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddScoped<BlazorApp1.Services.IAccessTokenProvider, BlazorApp1.Services.AccessTokenProvider>();

builder.Services.AddScoped<BlazorApp1.Services.ICurrentUserService, BlazorApp1.Services.CurrentUserService>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

//builder.Services.AddScoped<MudEmojiPicker.Data.EmojiService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddThemeServices();

builder.Services.AddLocalization();

var app = builder.Build();

await app.Services.Localize();

await app.RunAsync();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
         .HandleTransientHttpError()
         .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5));
}