using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace UdemyIdentityServer.AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            //Identity servis kuruyoruz.
            services.AddIdentityServer()
                .AddInMemoryApiResources(Config.GetApiResources())//identity den memory servislerin şunlar.Yani memoryden ver veritabaından verme
                .AddInMemoryApiScopes(Config.GetApiScopes())//confi den scope ları getir.
                .AddInMemoryClients(Config.GetClients())//get clientleride aldık. config.cs den
                .AddInMemoryIdentityResources(Config.GetIdentityResources())//IDENTITY SERVER ASIMETRIK ŞIFRELEME KULLANIR...Simetrik Şifreleme=hem tokeni doğrulamak hemde tokeni imzalama işlemi Asimterik Şifreleme=Private ve Public key olur. Private tutulur public key şifreyi kim çözecekse onla paylaşılır ve public key sahip olan kişi private keyi doğrulayabilir.
                .AddTestUsers(Config.GetUsers().ToList())//Test userlerini tanımlıyoruz.config dosyasında test userlerini tanımlama yaptık.
                .AddDeveloperSigningCredential();//Public ve Private key oluşturur.private elinde tutar publici app lere dağıtacak. ve app den geldiğinde elindeki public ile karşılaştıracak ve ona göre kapıları açacak veya kapatacak.

            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();//Identity server middelware olarak tanımlıyoruz. yani çalıştırıyoruz.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}