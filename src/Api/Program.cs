using AutoMapper;
namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            
            builder.Services.ApplyServices(builder.Configuration);
            builder.Services.AddAuthorizeSwaggerAsync(builder.Configuration);

            object value = builder.Services.ValidationResponse();

            var app = builder.Build();

            await app.ApplyMigrationWithSeed();
            app.UseMiddleware<GlobalErrorMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}