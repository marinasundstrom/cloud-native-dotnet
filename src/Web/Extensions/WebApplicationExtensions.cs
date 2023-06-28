using BlazorApp1.Features.Users;
using BlazorApp1.Features.Test;

namespace BlazorApp1.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapApplicationEndpoints(this WebApplication app)
    {
        app.MapUsersEndpoints()
           .MapTestEndpoints();

        return app;
    }

    public static WebApplication MapApplicationHubs(this WebApplication app)
    {
       //app.MapTodoHubs();

        return app;
    }
}
