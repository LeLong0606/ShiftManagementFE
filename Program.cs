using ShiftManagementFE.Components;
using ShiftManagementFE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for dependency injection with BaseAddress
builder.Services.AddHttpClient("ApiClient", client =>
{
    // Thay thế URL này bằng URL backend API của bạn nếu khác
    client.BaseAddress = new Uri("https://localhost:7222/");
});

// Register application services for DI, inject named HttpClient
builder.Services.AddScoped<UserService>(sp =>
    new UserService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<AuthService>(sp =>
    new AuthService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<DepartmentService>(sp =>
    new DepartmentService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<HolidayService>(sp =>
    new HolidayService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<StoreService>(sp =>
    new StoreService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<RoleService>(sp =>
    new RoleService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<LogService>(sp =>
    new LogService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<ScheduleService>(sp =>
    new ScheduleService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<AttendanceService>(sp =>
    new AttendanceService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<ExportService>(sp =>
    new ExportService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<UserRoleService>(sp =>
    new UserRoleService(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
        sp.GetRequiredService<ErrorService>()));
builder.Services.AddScoped<ErrorService>(sp =>
    new ErrorService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();