using Demo.API.Filters;
using Demo.API.MediatR;
using Demo.Business;
using Demo.Business.Behaviours;
using Demo.Business.ItemMaster;
using Demo.Business.PurchaseOrder;
using Demo.Business.Supplier;
using Demo.DataContext.UintofWorks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PartialResponse.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Demo.API
{
    public class Startup
    {
        readonly string CorsOnly = "CorsPolicy";
        readonly string AllowedOrg = "AllowedOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("0.0.0.0"));
            });
            services.AddLogging();
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //additional filters
            services
             .AddMvc(options =>
             {
                 options.Filters.Add(typeof(ApiClientValidationExceptionAttribute));
                 options.Filters.Add(typeof(FluentValidationExceptionAttribute));
                 options.Filters.Add(typeof(ValidateModelAttribute));
                 options.Filters.Add(typeof(GlobalExceptionFilter));
                 //options.Filters.Add(typeof(ApiIActionFilter));
                 options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
             }).AddPartialJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Configure MediatR design pattern
            services.AddMediatR(typeof(AssemblyMaker));
            services.AddScoped<IMediator, SynchronousMediator>();

            //Validation token keys
            string jwtPublicKey = Configuration.GetValue<string>("JwtPublicKey");
            string authTokenIssuer = Configuration.GetValue<string>("AuthTokenIssuer");
            // Configure Fluent Validation
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            //pgsql database connected
            // services.AddDbContext<RMKVCommonContext>(options => options.UseNpgsql(Configuration["DatabaseConfig:PostgresSQL"]));
            services.AddTransient<IUnitofwork, Unitofwork>(ctx => new Unitofwork(Configuration["DatabaseConfig:PostgresSQL"]));

            //Item Master
            services.AddScoped<IValidator<SaveItemMasterService>, SaveItemMasterValidation>();
            services.AddScoped<IValidator<ListItemMasterService>, ListItemMasterValidation>();
            services.AddScoped<IValidator<FetchItemMasterService>, FetchItemMasterValidation>();
            services.AddScoped<IValidator<ItemMasterActiveUpdateService>, ItemMasterActiveUpdateValidation>();

            //Supplier
            services.AddScoped<IValidator<SaveSupplierService>, SaveSupplierValidation>();
            services.AddScoped<IValidator<ListSupplierService>, ListSupplierValidation>();
            services.AddScoped<IValidator<FetchSupplierService>, FetchSupplierValidation>();
            services.AddScoped<IValidator<SupplierActiveUpdateService>, SupplierActiveUpdateValidation>();

            //Purchase Order
            services.AddScoped<IValidator<SavePurchaseOrderService>, SavePurchaseOrderValidation>();
            services.AddScoped<IValidator<ListPurchaseOrderService>, ListPurchaseOrderValidation>();
            services.AddScoped<IValidator<FetchPurchaseOrderService>, FetchPurchaseOrderValidation>();
            services.AddScoped<IValidator<PurchaseOrderActiveUpdateService>, PurchaseOrderActiveUpdateValidation>();
            services.AddScoped<IValidator<GetLookupDetailsService>, GetLookupDetailsValidation>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //c.DescribeAllEnumsAsStrings();
                //c.DescribeStringEnumsInCamelCase();
                c.DescribeAllParametersInCamelCase();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //    {
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //        Reference = new OpenApiReference
                //            {
                //            Type = ReferenceType.SecurityScheme,
                //            Id = "Bearer"
                //            },
                //            //Scheme = "oauth2",
                //            Scheme = "ApiKeys",
                //            Name = "Bearer",
                //            In = ParameterLocation.Header,

                //        },
                //        new List<string>()
                //        }
                //    }
                //);

            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsOnly,
                 builder =>
                 {
                     builder.WithOrigins(Configuration.GetSection(AllowedOrg)
                                          .Get<string[]>())
                                         .AllowAnyHeader()
                                         .AllowAnyMethod().AllowCredentials();
                 });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsOnly);//

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

           // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
