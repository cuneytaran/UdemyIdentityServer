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
                    Scopes={ "api1.read","api1.write","api1.update" },//tanımlanmış yetkileri atıyoruz (oku,yazma,güncelleme).//INTROSPECTION ENDPOINT için API nin userName= resource_api1 
                    ApiSecrets = new []{new  Secret("secretapi1".Sha256())}//INTROSPECTION ENDPOINT için API nin Password=secretapi1
                },

                new ApiResource("resource_api2")//2.API yi tanımlıyoruz
                {
                       Scopes={ "api2.read","api2.write","api2.update" },//INTROSPECTION ENDPOINT için API nin userName= resource_api2 
                       ApiSecrets = new []{new  Secret("secretapi2".Sha256()) }//INTROSPECTION ENDPOINT için API nin Password=secretapi2
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
                //identity de ön tanımlı bilgiler bunlar.
                new IdentityResources.OpenId(), //olmazsa olmazı bu.Token döndüğünde içinde mutlaka kullanıcının id si olmalı buda subjectid olarak geçer.Yani userId si için.
                new IdentityResources.Profile(), //User profil bilgileri. Bunun içinde bir sürü claim tutabilirsiniz.bilgi için adres : https://developer.okta.com/blog/2017/07/25/oidc-primer-part-1
                new IdentityResource(){ Name="CountryAndCity", DisplayName="Country and City",Description="Kullanıcının ülke ve şehir bilgisi", UserClaims= new [] {"country","city"}},//ülke ve şehir bilgisini tutuyoruz ve istediğimiz clientlere vereceğiz.Custom bilgiler ekliyoruz

                new IdentityResource(){ Name="Roles",DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new [] { "role"} }//Roller tanımlanıyor ve role isminde claim oluşturduk.Yani token içinde role de gelecek
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            //Test aşamasında test etmek için User sınıfı oluşturuyoruz. Burda veritabanı oluşturmadan deneme yapmak için. Yani developer için. işler tamam oldğunda veritabanı oluşutracağız.

            return new List<TestUser>()
            {
                new TestUser{ SubjectId="2",Username="caran",  Password="12345",
                    Claims= new List<Claim>(){
                    new Claim("given_name","Cüneyt"),
                    new Claim("family_name","Aran"),
                    new Claim("country","Türkiye"),
                    new Claim("city","İstanbul"),
                    new Claim("role","admin")
                 } },
                new TestUser{ SubjectId="1",Username="caran2",  Password="12345",
                    Claims= new List<Claim>(){ //SubjectId=userID aslında.Bu Calimler token içinde bulunacak datalardır.Aynı zamanda Claim bazlı yetkilendirme yapmak için gereken datalardır.
                    new Claim("given_name","Fatih"),//given_name,family_name,country vs...= bunlar identity serverin içinde hazırda bulunan claimlerdir.https://developer.okta.com/blog/2017/07/25/oidc-primer-part-1
                    new Claim("family_name","Çakıroğlu"),
                    new Claim("country","Türkiye"),
                    new Claim("city","Ankara"),
                    new Claim("role","customer")//userin rolü tanımlanıyor
                } }

            };
            //Bu tanımlama yaptıktan sonra startupda tanımla yapman gerekiyor..AddTestUsers(Config.GetUsers().ToList())
        }

        public static IEnumerable<Client> GetClients()//Yukarıdaki API lere hangi clientler kullanacak 15 ve 32. satırlardan bahsediyorum
        {
            return new List<Client>(){//Client = identityserver üzerinden geliyor.

                new Client()//BU CLIENTLE ÇIKIŞ YAPARSAN SADECE API1 ERİŞEBİLİRSİN
                {
                   ClientId = "Client1",//sitenin ClientId si olacak bunu kullanıcının username gibi düşünebilirsin
                   ClientName="Client 1 app uygulaması",//kullanıcının API den data almak için işimize yarayacak.
                   ClientSecrets=new[] {new Secret("secret".Sha256())},//şifre secret olarak belirledik. ve bunu şifreledik.yani hash ledik.datayı appsettings den almak daha doğru olur.
                   AllowedGrantTypes= GrantTypes.ClientCredentials,//ClientCredentials= bu akışa uygun token verecek. Akış tipini seçtik.bir çok tipler var. Ençok kullanılan bu tipdir.
                   AllowedScopes= {"api1.read"}////erişim tanımlandığı yer.Hangi API den nasıl bir yetki ye erişmek isteyeceğimizi belirliyoruz. 
                },
                 new Client()//BU CLIENTLE ÇIKIŞ YAPARSAN API1 ve API2 YE ERİŞEBİLİRSİN
                {
                   ClientId = "Client2",
                   ClientName="Client 2 app uygulaması",
                   ClientSecrets=new[] {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.ClientCredentials,
                   AllowedScopes= {"api1.read" ,"api1.update","api2.write","api2.update"}//erişim tanımlandığı yer
                },
                 new Client()
                 {
                   ClientId = "Client1-Mvc",
                   RequirePkce=false,//secret devre dışı bırakayımmı. Randon challenge ve modifie yapalımmı
                   ClientName="Client 1 app  mvc uygulaması",
                   ClientSecrets=new[] {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.Hybrid,//code id_token kullanıdığımız için hybrid kullanıyoruz.
                   RedirectUris=new  List<string>{ "https://localhost:5006/signin-oidc" },//token alma işlemini gerçekleştiren URL dir.Authorize endpoint den token bu adrese yönlenecek.
                   PostLogoutRedirectUris=new List<string>{ "https://localhost:5006/signout-callback-oidc" },//identity serverden logout işlemi olduğunda bu adrese yönlenecek.
                   AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},//identityResource den yetkileri alıyoruz.OfflineAccess startup tarafında tanımladık.
                   AccessTokenLifetime=2*60*60,//access token ömrü tanımlıyoruz.Biz 2 saatlik yaptık.Defaultda bunu saniye bazında yapıyoruz.Defaultu 3600 sn yani 1 saat. 2*60*60=saat*dakika*saniye ikinci yol (int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds
                   AllowOfflineAccess=true,//refresh token oluşturma komutu
                   RefreshTokenUsage=TokenUsage.ReUse,//refresh token birdende fazla kulllanılabilsin.Eğer biz OneTimOnly seçersek bir kez refresh token kullanılır. Eğer ReUse seçersek sürekli kendisini yenileme özelliği olmuş olur.
                   RefreshTokenExpiration=TokenExpiration.Absolute,//refresh token da Absolute seçersek kesin bir süre vermiş oluruz. Mesela 5 gün ömrü bitsin. Sliding verirsek default 15 gün. eğer 15 gün içersinde bir refresh token yaparsak tekrar 15 gün daha uzar. 
                   AbsoluteRefreshTokenLifetime=(int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,//Absolute kesin ömür vermiştik. Default da 30 gündür. Ama biz 60 günlük bir refresh token yaptık. 
                   RequireConsent=false//bunu true yaparsan onay sayfasına otomatik yönlendirir.login olduktan sonra ikinci bir onay sayfasına yönlendirir.Yani token içinde hangi bilgiler olsun olmasın diye checkbox tikleyerek seçebiliyorsun.
                   //yukarıda 2 saatlik token ve 60 günlük refresh token hazırladık.
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
                   AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read","api2.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},//"CountryAndCity","Roles"=token içinde country ve rollerde görünecek
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