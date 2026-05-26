using JuezSeguro.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 4;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()                       
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.HttpOnly = true;           
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin",       p => p.RequireRole("Administrador"));
    options.AddPolicy("SoloJuez",        p => p.RequireRole("Juez"));
    options.AddPolicy("SoloAuditor",     p => p.RequireRole("Auditor"));
    options.AddPolicy("JuezOAdmin",      p => p.RequireRole("Juez", "Administrador"));
    options.AddPolicy("UsuarioInterno",  p => p.RequireRole("Juez", "Auditor", "Administrador", "OperadorTecnico"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Aplicar migraciones pendientes antes de usar RoleManager/Identity.
    // Evitar intentar crear tablas que ya existen (por ejemplo si la BD fue restaurada
    // o las tablas fueron creadas manualmente) para prevenir errores como
    // "There is already an object named 'AspNetUsers' in the database".
    var db = services.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (await db.Database.CanConnectAsync())
        {
            var conn = db.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUsers'";
                var scalar = await cmd.ExecuteScalarAsync();
                var exists = Convert.ToInt32(scalar) > 0;
                if (!exists)
                {
                    await db.Database.MigrateAsync();
                }
                else
                {
                    // Las tablas de Identity ya existen; omitimos MigrateAsync.
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        else
        {
            // Si no puede conectar, intentar ejecutar MigrateAsync para que EF intente crear la BD
            await db.Database.MigrateAsync();
        }
    }
    catch (Microsoft.Data.SqlClient.SqlException ex)
    {
        // Si hay un error de creación por objeto ya existente, lo registramos y continuamos.
        // Esto evita que la aplicación falle en entornos donde la BD fue preparada manualmente.
        Console.WriteLine($"SQL error applying migrations: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Juez", "Auditor", "Administrador", "OperadorTecnico" };

    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    const string adminEmail = "ceo@lynxcores.com";
    const string adminPassword = "Admin@JuezSeguro2026";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Administrador");
            Console.WriteLine($"✅ Usuario admin creado: {adminEmail}");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<JuezSeguro.Hubs.ChatHub>("/chathub");

app.Run();