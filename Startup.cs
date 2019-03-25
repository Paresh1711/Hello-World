using HellofreshTest.DataContext;
using HellofreshTest.Interface;
using HellofreshTest.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace HellofreshTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();
            //Configuration = builder.Build();

            Func<string, string> settingsResolver = (name) => Configuration[name];

            MongoDbConnection.ServerAddress = Configuration.GetSection("MongoCon:ServerAddress").Value;
            MongoDbConnection.ServerPort = int.Parse(Configuration.GetSection("MongoCon:ServerPort").Value);
            MongoDbConnection.DatabaseName = Configuration.GetSection("MongoCon:DatabaseName").Value;
            MongoDbConnection.UserName = Configuration.GetSection("MongoCon:UserName").Value;
            MongoDbConnection.UserPassword = Configuration.GetSection("MongoCon:UserPassword").Value;
        }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();

        //    Func<string, string> settingsResolver = (name) => Configuration[name];

        //    MongoDbConnection.ServerAddress = settingsResolver("MongoDb.ServerAddress");
        //    MongoDbConnection.ServerPort = int.Parse(settingsResolver("MongoDb.ServerPort"));
        //    MongoDbConnection.DatabaseName = settingsResolver("MongoDb.DatabaseName");
        //    MongoDbConnection.UserName = settingsResolver("MongoDb.UserName");
        //    MongoDbConnection.UserPassword = settingsResolver("MongoDb.UserPassword");
        //}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });
            services.AddMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }
            );
            //.AddXmlSerializerFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IRecipes, Recipe>();

           // services.AddSwagger();

           // services.AddDbContext<ApplicationDBContext>(options => options.UseMongoDB)
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();


        }
    }
}
