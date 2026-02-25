using HR.LeaveManagement.API.Middlewares;

namespace HR.LeaveManagement.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UsePresentationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }


}
