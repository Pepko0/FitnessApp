using Microsoft.AspNetCore.Http;

namespace FitnessApp.Helpers
{
    public class AdminOnlyMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminOnlyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            var path = ctx.Request.Path.Value ?? "";

            if (path.StartsWith("/zarzadzanie"))
            {
                if (ctx.Session.GetInt32("OperatorId") == null)
                {
                    ctx.Response.Redirect("/login");
                    return;
                }
            }

            await _next(ctx);
        }
    }
}
