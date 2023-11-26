using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using MusicPlayerServer.Models;

namespace MusicPlayerServer
{
    public class Authorization
    {
        public static async ValueTask<object?> RequiresSignIn(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            int? userID = Convert.ToInt32(GetCookie("userID", context.HttpContext));
            string? password = GetCookie("password", context.HttpContext);

            if(userID == null || password == null)
            {
                return Results.Unauthorized();
            }

            using(var db = new MusicPlayerServerContext())
            {
                var query = from user
                            in db.Users
                            where user.UserID == userID && user.Password == password
                            select user;
                if(query.Count() == 0)
                {
                    return Results.Unauthorized();
                }
            }
            return await next(context);
        }

        public static string? GetCookie(string name, HttpContext context)
        {
            return context.Request.Cookies[name];
        }
    }
}
