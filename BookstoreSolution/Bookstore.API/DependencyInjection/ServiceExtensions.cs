using Bookstore.BLL.Interfaces;
using Bookstore.BLL.Services;
using Bookstore.Common;
using Bookstore.DAL.Context;
using Bookstore.DAL.Interfaces;
using Bookstore.DAL.Repositories;

namespace Bookstore.API.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BookstoreDb");
            services.AddSingleton(new BookstoreDbContext(connectionString));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddLoggingServices(this IServiceCollection services, IConfiguration configuration)
        {
            var logPath = configuration["Logging:LogPath"] ?? "Logs/log-.txt";
            var level = configuration["Logging:MinimumLevel"] ?? "Information";
            Logger.Initialize(logPath, level);
            return services;
        }
    }
}
