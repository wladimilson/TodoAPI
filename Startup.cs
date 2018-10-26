using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using TodoAPI.Data;
using TodoAPI.Repositories;

namespace TodoAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
              .AddJwtBearer(options => {  
                  options.TokenValidationParameters =   
                       new TokenValidationParameters  
                  {  
                      ValidateIssuer = true,  
                      ValidateAudience = true,  
                      ValidateLifetime = true,  
                      ValidateIssuerSigningKey = true,  
  
                      ValidIssuer = "TodoAPI",  
                      ValidAudience = "TodoAPI",  
                      IssuerSigningKey =   
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes("todo-security-key"))
                  };
              });  

            services.AddDbContext<TodoContext>(options =>
                  options.UseInMemoryDatabase("Todo"));

            services.AddTransient<IItemRepository, ItemRepository>();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Todo API",
                    Description = "Um exemplo de aplicação ASP.NET Core Web API",
                    TermsOfService = "Não aplicável",
                    Contact = new Contact
                    {
                        Name = "Wladimilson",
                        Email = "contato@treinaweb.com.br",
                        Url = "https://treinaweb.com.br"
                    },
                    License = new License
                    {
                        Name = "CC BY",
                        Url = "https://creativecommons.org/licenses/by/4.0"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                opt.AddSecurityDefinition(
                    "Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Copie 'Bearer ' + token'",
                        Name = "Authorization",
                        Type = "apiKey"
                    });

                opt.AddSecurityRequirement(security);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //Ativa o Swagger
            app.UseSwagger();

            // Ativa o Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoAPI V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
