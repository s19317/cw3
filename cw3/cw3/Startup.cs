using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.Middlewares;
using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cwicz_3
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
            //services.AddSingleton<IDbService>();
            services.AddScoped<IStudentsDbService, SqlServerDbService>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseMiddleware<LoggingMiddleware>();
            app.Use(async (context, next) =>
            {

                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Podaj indeks");
                    return;
                }
                else
                {
                    string index = context.Request.Headers["Index"].ToString();
                    var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19461;Integrated Security=True");
                    using (var com = new SqlCommand())
                    {
                        com.Connection = connection;
                        com.CommandText = "select IndexNumber from Student";
                        connection.Open();
                        var dr = com.ExecuteReader();
                        bool tmp = false;
                        while (dr.Read())
                        {
                            if (dr["IndexNumber"].ToString().Equals(index))
                                tmp = true;

                        }
                        if (!tmp)
                        {
                            context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound;
                            await context.Response.WriteAsync("nie ma takiego indeksu");
                            return;
                        }
                    }
                    connection.Close();
                }

                await next();
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}