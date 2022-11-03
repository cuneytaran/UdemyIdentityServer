using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Models;

namespace UdemyIdentityServer.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _context;

        //CustomDbContext startupda tanımlandı. Startupda tanımlanan şey heryerden çağılırılabilir!!!!
        public CustomUserRepository(CustomDbContext context)
        {
            _context = context;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _context.customUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _context.customUsers.FindAsync(id);//FindAsync=direk olarak primary key e göre arama yaptığı için çok hızlıdır.
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _context.customUsers.AnyAsync(x => x.Email == email && x.Password == password);//AnyAsync= true veya false döndürür

        }
    }
}
