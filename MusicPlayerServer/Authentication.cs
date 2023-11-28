using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        
    }
}
