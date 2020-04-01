using AzureADAuth.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Data
{
    public class UserRepository : IUserRepository
    {
        private List<User> users = new List<User>
        {
            new User { Id = 3522, Name = "roland", Password = "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=",
                FavoriteColor = "blue", Role = "Admin", GoogleId = "115035533904273209556" }
        };

        public User GetByUsernameAndPassword(string username, string password)
        {
            var user = users.SingleOrDefault(u => u.Name == username &&
                u.Password == password.Sha256());
            return user;
        }

        public User GetByGoogleId(string googleId)
        {
            var user = users.SingleOrDefault(u => u.GoogleId == googleId);
            return user;
        }
    }
}
