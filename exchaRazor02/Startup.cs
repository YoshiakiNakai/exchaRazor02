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

using exchaRazor02.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace exchaRazor02
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//クッキーの設定
			services.Configure<CookiePolicyOptions>(options => {
				options.CheckConsentNeeded = context => !context.User.Identity.IsAuthenticated;
				options.MinimumSameSitePolicy = SameSiteMode.None;
				options.Secure = CookieSecurePolicy.Always;
				options.HttpOnly = HttpOnlyPolicy.Always;
			});
			//認証にクッキーを使用する
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

			//DBの使用
			services.AddDbContext<ExchaDContext5>(options => options.UseSqlServer(Configuration.GetConnectionString("ExchaDContext5")));

			//Razorの使用
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseCookiePolicy();      //クッキーの設定を適用
			app.UseAuthentication();    //認証機能を使用

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
