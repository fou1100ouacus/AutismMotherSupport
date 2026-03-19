
namespace Api.Extensions
{
    public static class AddAuthorizeSwagger
    {
        public static IServiceCollection AddAuthorizeSwaggerAsync(
            this IServiceCollection service, 
            IConfiguration configuration)
        {
            // JWT Authentication Setup
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtOptions:issuer"],
                    ValidAudience = configuration["JwtOptions:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Environment.GetEnvironmentVariable("secretKey") 
                            ?? configuration["JwtOptions:secretKey"]!
                        ))
                };
            });

            // Swagger Configuration - FIXED for Swashbuckle 10+
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                // This is the key change
                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                  //  [new OpenApiSecuritySchemeReference("Bearer", document)] = Array.Empty<string>()
                });
            });

            return service;
        }
    }
}