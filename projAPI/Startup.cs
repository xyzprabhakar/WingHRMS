using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using projAPI.Classes;
using projContext;
using projAPI.Services;

namespace projAPI
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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                //options.AddPolicy("AllowSpecificOrigin",
                //builder => builder.WithOrigins("http://192.168.10.6:1010", "http://localhost:51625", "https://localhost:51625").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                //builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                //builder => builder.WithOrigins("http://192.168.10.129:1010", "http://192.168.10.129:1016", "http://14.143.182.168:1010", "http://14.143.182.168:1016", "http://localhost:51625", "https://localhost:51625").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                //builder => builder.WithOrigins("http://192.168.10.129:1013").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            });

            //add context
            services.AddDbContext<projContext.Context>();
            
            services.AddScoped<IsrvSettings>(ctx => new srvSettings(ctx.GetRequiredService<projContext.Context>(), ctx.GetRequiredService<IConfiguration>()));
            services.AddScoped<IsrvUsers>(ctx => new srvUsers(ctx.GetRequiredService<projContext.Context>(),  ctx.GetRequiredService<IsrvSettings>()));
            services.AddScoped<IsrvCurrentUser>(ctx => new srvCurrentUser(ctx.GetRequiredService<IHttpContextAccessor>()));
            services.AddScoped<IsrvEmployee>(ctx => new srvEmployee(ctx.GetRequiredService<projContext.Context>(), ctx.GetRequiredService<IsrvSettings>(), ctx.GetRequiredService<IConfiguration>()));
            services.AddScoped<IsrvDistributer>(ctx => new srvDistributer(ctx.GetRequiredService<projContext.Context>(), ctx.GetRequiredService<IsrvSettings>()));

            
            //add authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                };
            });

            services.Configure<AppSettings>(Configuration);

            //add authorization
            //  IEnumerable<string> allAccessRights = new projContext.Context().tbl_claim_master.Select(p => p.claim_master_id.ToString()).Distinct().ToList();
            services.AddAuthorization(options =>
            {
                foreach (enmDocumentMaster _enm in Enum.GetValues(typeof(enmDocumentMaster)))
                {
                    var DocumentType=_enm.GetDocumentDetails().DocumentType;                    
                        options.AddPolicy(_enm.ToString()+ enmDocumentType.Create.ToString(), policy => policy.Requirements.Add(new AccessRightRequirement(_enm, enmDocumentType.Create)));                    
                        options.AddPolicy(_enm.ToString() + enmDocumentType.Update.ToString(), policy => policy.Requirements.Add(new AccessRightRequirement(_enm, enmDocumentType.Update)));                    
                        options.AddPolicy(_enm.ToString() + enmDocumentType.Approval.ToString(), policy => policy.Requirements.Add(new AccessRightRequirement(_enm, enmDocumentType.Approval)));
                        options.AddPolicy(_enm.ToString() + enmDocumentType.Delete.ToString(), policy => policy.Requirements.Add(new AccessRightRequirement(_enm, enmDocumentType.Delete)));                    
                        options.AddPolicy(_enm.ToString() + enmDocumentType.Report.ToString(), policy => policy.Requirements.Add(new AccessRightRequirement(_enm, enmDocumentType.Report)));                    
                        options.AddPolicy(_enm.ToString() + enmDocumentType.DisplayMenu.ToString(), policy => policy.Requirements.Add(new AccessRightRequirement(_enm, enmDocumentType.DisplayMenu)));
                    

                }
            });
            services.AddScoped<IAuthorizationHandler, AccessRightHandler>();
            //services.AddSingleton<IAuthorizationHandler, AccessRightHandler>();
            //for Accessing HttpRequest in authorization handler class;
            services.AddHttpContextAccessor();
            //previous code

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /* The relevant part for Forwarded Headers */
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            //var context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            #region for scheduler
            // services.AddQuartz(typeof(ScheduledJob));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            //app.UseMvc();

            // for getting the Client IP address need to integrates
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            #region for schedulerCompanyReport
            //app.UseQuartz();
            app.UseMvc();
            #endregion
        }

    }


}
