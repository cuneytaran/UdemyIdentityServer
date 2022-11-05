using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UdemyIdentityServer.AuthServer.Seeds;

namespace UdemyIdentityServer.AuthServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();//Startup daki bilgileri almak için host yöntemi kullanacağız.

            using (var serviceScope = host.Services.CreateScope())//using=sadece bir kere çalışır.
            {
                var services = serviceScope.ServiceProvider;//configurationdbcontexte erişebileceiğiz ve  oluşturduğumuz sevisleri alıyoruz.

                var context = services.GetRequiredService<ConfigurationDbContext>();//servis yoksa bize hata versin.ConfigurationDbContext bana ver diyoruz

                IdentityServerSeedData.Seed(context);//seed i çalıştıracak.
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}