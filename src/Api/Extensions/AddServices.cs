
namespace Api.Extensions
{
    public static class AddServices
    {
        public static IServiceCollection ApplyServices(this IServiceCollection service, IConfiguration configration)
        {
            service.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configration.GetConnectionString("DefaultConnection"));
            });

            service.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            service.AddScoped<AuthUseCase>();
            service.AddScoped<AccountUseCase>();
            service.AddScoped<RoleUseCase>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            service.AddScoped<IRoleRepository, RoleRepository>();


            return service;
        }

    }
}
