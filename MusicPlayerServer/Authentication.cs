using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MusicPlayerServer.Models;

namespace MusicPlayerServer
{
    public class Authentication
    {
        public static async Task<IResult> SignUp(User user)
        {
            int userID;
            using(var context = new MusicPlayerServerContext())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                var addedUser = context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                userID = addedUser.UserID;
            }
            return Results.Ok(userID);
        }

        public static async Task<IResult> SignIn(User user)
        {
            int userID;
            using(var context = new MusicPlayerServerContext())
            {
                var userFound = context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                userID = userFound.UserID;
            }
            return Results.Ok(userID);
        }
        
    }
}
