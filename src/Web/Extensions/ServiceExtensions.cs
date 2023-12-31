﻿using FluentValidation;
using MediatR;
using BlazorApp1.Behaviors;
using BlazorApp1.Features.Chat;

namespace BlazorApp1.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(ServiceExtensions)));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        //services.AddTodoControllers();

        //services.AddScoped<IChatNotificationService, ChatNotificationService>();

        return services;
    }
}
