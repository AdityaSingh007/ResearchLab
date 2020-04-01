using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Data
{
    public interface IUserRepository
    {
        User GetByUsernameAndPassword(string username, string password);
        User GetByGoogleId(string googleId);
    }
}
