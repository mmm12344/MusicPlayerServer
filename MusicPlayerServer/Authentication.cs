using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration.UserSecrets;
using MusicPlayerServer.Models;

namespace MusicPlayerServer
{
    public record UserSignIn(string email, string password);

    public class Authentication
    {
        public static async Task<IResult> SignUp(User user)
        {
            int userID;
            using(var context = new MusicPlayerServerContext())
            {
                var currentUsers = context.Users.Where(u => u.Email == user.Email).ToList();
                if(currentUsers.Count() > 0)
                {
                    return Results.Problem();
                }
                context.Users.Add(user);
                await context.SaveChangesAsync();
                var addedUser = context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                userID = addedUser.UserID;
            }
            return Results.Ok(userID);
        }

        public static async Task<IResult> SignIn(UserSignIn user)
        {
            int userID;
            using(var context = new MusicPlayerServerContext())
            {
                var usersFound = from u
                                in context.Users
                                where u.Email == user.email && u.Password == user.password
                                select u;
                var userFound = usersFound.FirstOrDefault();
                if(userFound == null) 
                {
                    return Results.Problem();
                }
                userID = userFound.UserID;
            }
            return Results.Ok(userID);
        }

        public static async Task<IResult> ChangeUserInfo(User user, HttpContext httpContext)
        {
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            string password = Authorization.GetCookie("password", httpContext);
            using (var context = new MusicPlayerServerContext())
            {
                var usersFound = from u
                                in context.Users
                                 where u.UserID == userID && u.Password == password
                                 select u;
                User userFound = usersFound.FirstOrDefault();
                if (userFound == null)
                {
                    return Results.Problem();
                }
                if(user.FirstName != null)
                    userFound.FirstName = user.FirstName;
                if(user.LastName != null)
                    userFound.LastName = user.LastName;
                if (user.Password != null)
                    userFound.Password = user.Password;
                context.SaveChanges();
            }
            return Results.Ok();

        }
        
    }
}
