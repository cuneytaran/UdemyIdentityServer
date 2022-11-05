using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentityServer.AuthServer.Seeds
{
    public static class IdentityServerSeedData 
    {
        public static void Seed(ConfigurationDbContext context)//seed datalar static dir. bir kere çalışır ve bir daha ihtiyacımız olmayacak
        {
            //bu sayfada config.cs içindeki dataları sql veritabanına kaydetme işlemi yapılacak
            //uygulama ayağa kalktığında bir kere çalışacak. bir kere çalıştırabilmek için program css de tanımlayabiliriz.
            //NOT:PROGRAM.CS PROGRAM AYAĞA KALKTIĞNDA BİR KERE ÇALIŞIR.
            if (!context.Clients.Any()) //config.cs içindeki clientleri dolaşıyoruz.
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());//tüm clientleri veritabanındaki client tablosuna ekliyoruz.
                }
            }

            if (!context.ApiResources.Any())//config.cs içindeki Resources dolaşıyoruz.
            {
                foreach (var apiResource in Config.GetApiResources())
                {
                    context.ApiResources.Add(apiResource.ToEntity()); //tüm ApiResources veritabanındaki ApiResources tablosuna ekliyoruz.
                }
            }

            if (!context.ApiScopes.Any())//config.cs içindeki Scopes dolaşıyoruz.
            {
                Config.GetApiScopes().ToList().ForEach(apiscope =>
                {
                    context.ApiScopes.Add(apiscope.ToEntity());//tüm Scopes veritabanındaki Scopes tablosuna ekliyoruz.
                });
            }

            if (!context.IdentityResources.Any())//config.cs içindeki IdentityResources dolaşıyoruz.
            {
                foreach (var identityResource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(identityResource.ToEntity());//tüm IdentityResources veritabanındaki IdentityResources tablosuna ekliyoruz.
                }
            }

            context.SaveChanges();//veritabanına kaydetme işlemi yapılıyor
        }
    }
}