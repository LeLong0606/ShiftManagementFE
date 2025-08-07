using ShiftManagementFE.Components;
using ShiftManagementFE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

// Register application services for DI
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<HolidayService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<UserRoleService>();

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