using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace ChatApp.DAL.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
