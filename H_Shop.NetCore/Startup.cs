using AutoMapper;
using DataAndServices.Admin_Services.AccountService;
using DataAndServices.Admin_Services.Admin_Acc_Services;
using DataAndServices.Admin_Services.Checkout_Customer_Services;
using DataAndServices.Admin_Services.CheckoutOrderServices;
using DataAndServices.Admin_Services.Products;
using DataAndServices.Admin_Services.UserServices;
using DataAndServices.Client_Services;
using DataAndServices.Data;
using DataAndServices.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace H_Shop.NetCore
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
            //services.AddDbContext<OnlineShopEntities>(options =>
            //    options.UseS(Configuration.GetConnectionString("OnlineShop")));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllers();
            services.AddScoped(p => new DataContext(Configuration["Data:ConnectionString"], Configuration["Data:DbName"]));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "ASP.NET Core Web API Sample Example"
                });
            });
            services.AddScoped<IAccountService, AccountService>();
            services.AddTransient<IAdminAccService, AdminAccService>();
            services.AddTransient<ICheckoutCustomerService, CheckoutCustomerService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartServices, CartService>();
            services.AddTransient<IHomeServices, HomeService>();
            services.AddTransient<IProductClientServices, ProductClientService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUsers, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
