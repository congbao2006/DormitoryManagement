using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QuanLyKyTucXa.Application.Abstractions.Services;
using QuanLyKyTucXa.Application.Common.Behaviors;
using QuanLyKyTucXa.Application.Services;

namespace QuanLyKyTucXa.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IQuanLyKyTucXaService, QuanLyKyTucXaService>();

        return services;
    }
}