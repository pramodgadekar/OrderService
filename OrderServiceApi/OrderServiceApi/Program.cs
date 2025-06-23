
using MediatR;
using OrderServiceApi.KafkaProvider;
using OrderServiceApi.Service.Handler;
using OrderServiceApi.Service.Interface;
using OrderServiceApi.Service.Services;
using StackExchange.Redis;

namespace OrderServiceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
            
            var muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
                {
                    EndPoints = { { "redis-15520.c9.us-east-1-4.ec2.redns.redis-cloud.com", 15520 } }, // Should be coming from config
                    User = "default",  // Should coming from Secret Manager
                Password = "IuFYRolQqKwSuH7TzzOpekD8VCUna6RS" // Should coming from Secret Manager
            }
            );
            builder.Services.AddMediatR(typeof(CreateOrderHandler).Assembly);
            builder.Services.AddSingleton<IConnectionMultiplexer>(muxer);
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<INotificationClient, NotificationClient>();
            builder.Services.AddHttpClient(); // Generic registration

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
