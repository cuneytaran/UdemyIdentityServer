using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UdemyIdentityServer.AuthServer
{
    public static class Config
    {
        //packet yöneticisinden IdentityServer4 yükle.
        public static IEnumerable<ApiResource> GetApiResources()//ApiResource identityserver tanıyor
        {
            return new List<ApiResource>()//1.API yi tanımlıyoruz.
            {
                new ApiResource("resource_api1"){//ismini biz verdik
                    Scopes={ "api1.read","api1.write","api1.update" },//tanımlanmış yetkileri atıyoruz (oku,yazma,güncelleme)
                    ApiSecrets = new []{new  Secret("secretapi1".Sha256())
                    }
                },
                new ApiResource("resource_api2")//2.API yi tanımlıyoruz
                {
                       Scopes={ "api2.read","api2.write","api2.update" },
                          ApiSecrets = new []{new  Secret("secretapi2".Sha256()) }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()//Yetkilerin tanımlandığı yer
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read","API 1 için okuma izni"),//API 1 için okuma izni verdik.
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),

                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(), //subId
                new IdentityResources.Profile(), ///
                new IdentityResource(){ Name="CountryAndCity", DisplayName="Country and City",Description="Kullanıcının ülke ve şehir bilgisi", UserClaims= new [] {"country","city"}},

                new IdentityResource(){ Name="Roles",DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new [] { "role"} }
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser{ SubjectId="1",Username="fcakiroglu16",  Password="password",Claims= new List<Claim>(){
                    new Claim("given_name","Fatih"),
                    new Claim("family_name","Çakıroğlu"),
                   new Claim("country","Türkiye"),
                      new Claim("city","Ankara"),
                      new Claim("role","admin")
                } },
                 new TestUser{ SubjectId="2",Username="ahmet16",  Password="password",Claims= new List<Claim>(){
                new Claim("given_name","Ahmet"),
                new Claim("family_name","Çakıroğlu"),
                  new Claim("country","Türkiye"),
                      new Claim("city","İstanbul"),
                    new Claim("role","customer")
                 } }
            };
        }

        public static IEnumerable<Client> GetClients()//Yukarıdaki API lere hangi clientler kullanacak 15 ve 32. satırlardan bahsediyorum
        {
            return new List<Client>(){//Client = identityserver üzerinden geliyor.
                new Client()
                {
                   ClientId = "Client1",//sitenin ClientId si olacak bunu kullanıcının username gibi düşünebilirsin
                   ClientName="Client 1 app uygulaması",//kullanıcının API den data almak için işimize yarayacak.
                   ClientSecrets=new[] {new Secret("secret".Sha256())},//şifre secret olarak belirledik. ve bunu şifreledik.yani hash ledik.datayı appsettings den almak daha doğru olur.
                   AllowedGrantTypes= GrantTypes.ClientCredentials,//ClientCredentials= bu akışa uygun token verecek. Akış tipini seçtik.bir çok tipler var. Ençok kullanılan bu tipdir.
                   AllowedScopes= {"api1.read"}//Hangi API ye erişmek isteyeceğimizi belirliyoruz. 
                },
                 new Client()
                {
                   ClientId = "Client2",
                   ClientName="Client 2 app uygulaması",
                   ClientSecrets=new[] {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.ClientCredentials,
                   AllowedScopes= {"api1.read" ,"api1.update","api2.write","api2.update"}
                },
                 new Client()
                 {
                   ClientId = "Client1-Mvc",
                   RequirePkce=false,
                   ClientName="Client 1 app  mvc uygulaması",
                   ClientSecrets=new[] {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.Hybrid,
                   RedirectUris=new  List<string>{ "https://localhost:5006/signin-oidc" },
                   PostLogoutRedirectUris=new List<string>{ "https://localhost:5006/signout-callback-oidc" },
                   AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                   AccessTokenLifetime=2*60*60,
                   AllowOfflineAccess=true,
                   RefreshTokenUsage=TokenUsage.ReUse,
                   RefreshTokenExpiration=TokenExpiration.Absolute,
                   AbsoluteRefreshTokenLifetime=(int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,

                   RequireConsent=false
        },

                  new Client()
                 {
                   ClientId = "Client2-Mvc",
                   RequirePkce=false,
                   ClientName="Client 2 app  mvc uygulaması",
                   ClientSecrets=new[] {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.Hybrid,
                   RedirectUris=new  List<string>{ "https://localhost:5011/signin-oidc" },
                   PostLogoutRedirectUris=new List<string>{ "https://localhost:5011/signout-callback-oidc" },
                   AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read","api2.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                   AccessTokenLifetime=2*60*60,
                   AllowOfflineAccess=true,
                   RefreshTokenUsage=TokenUsage.ReUse,
                   RefreshTokenExpiration=TokenExpiration.Absolute,
                   AbsoluteRefreshTokenLifetime=(int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,

                   RequireConsent=false
        }
    };
        }
    }
    //BURDA TANIMLAMALARI YAPTIKTAN SONRA STARTUP A GEÇ 
}