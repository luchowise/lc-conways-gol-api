using Lc.MicroService.ConwayGoL.Persistence.Interfaces;
using Lc.MicroService.ConwayGoL.Services.Interfaces;
using Lc.MicroService.ConwayGoL.Services;
using Lc.MicroService.ConwayGoL.Persistence;
using Lc.MicroService.ConwayGoL.Middlewares;

namespace Lc.MicroService.ConwayGoL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add application-specific services to the container.
            var filePath = Configuration.GetValue<string>("BoardStatesFilePath");
            services.AddSingleton<IBoardStateRepository>(provider => new FileBoardStateRepository(filePath));

            services.AddSingleton<INeighborCounterService, NeighborCounterService>();
            services.AddSingleton<IStateCalculatorService, StateCalculatorService>();
            services.AddSingleton<IGameOfLifeService, GameOfLifeService>();

            // Add WebAPI services.
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //Add Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        /// <summary>
        /// Configures the pipeline for the application.
        /// </summary>
        /// <param name="app">The application builder to configure.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
