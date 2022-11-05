using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Repository;

namespace UdemyIdentityServer.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserRepository _customUserRepository;

        public CustomProfileService(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)//profil dataları alacak
        {
            var subId = context.Subject.GetSubjectId();

            var user = await _customUserRepository.FindById(int.Parse(subId));//useri bul

            //token içine ihtiyacınız olan bilgileri eklenmeli. veritabanına gitmeden kullanabileceğimiz alanları eklemek doğru olur.
            var claims = new List<Claim>()//kullanıcı login olduktan sonra token içine hangi bilgiler eklenecek
            {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),//ister JwtRegisteredClaimNames.Email yaz istersen "Email" yaz. ikiside olur. Çükü Claim içinde bu data yeri var.
               new Claim("name", user.UserName),//name olarak da tanımladık
               new Claim("city", user.City),//JwtRegisteredClaimNames içinde city olmadı için kendimiz tanımladık name olarak
            };

            if (user.Id == 1)//userin id si 1 olana admin rolü vermek istiyorum.Bizim tablomuzda role olmadığı için bunu yaptık. Ama tablonda role olsaydı yukardaki gibi rol tanımlayabilirsin.
            {
                claims.Add(new Claim("role", "admin"));
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claims);//contex in içine claimler ekleriyoruz.
            // yukarıda user bilgilerini jwt token içinde görmek istiyorsanız aşağıdaki kodu aç.Ama bu uygun değildir. User bilgilerini Çekmek için User.Infodan bu dataların çekmeesidir.
            //context.IssuedClaims = claims;

            //işlemler bittiğinde servisi startupda tanımlamayı unutma!!! .AddProfileService<CustomProfileService>();
        }

        public async Task IsActiveAsync(IsActiveContext context)//böyle bir kullanıcı varmı yokmu kontrolü yapacak
        {
            var userId = context.Subject.GetSubjectId();//kullanıcının id sini alacağız.

            var user = await _customUserRepository.FindById(int.Parse(userId));//böle bir user varmı yokmu kontrel edecek.eğer varsa gidecek dataları alacak.

            context.IsActive = user != null ? true : false;//usur var ise true yoksa false olsun
            //işlemler bittiğinde startupda tanımlamayı unutma!!!
        }
    }
}