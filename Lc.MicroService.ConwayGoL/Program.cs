namespace Lc.MicroService.ConwayGoL
{
    public class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">CLI arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application startup failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates the host builder for the application.
        /// </summary>
        /// <param name="args">CLI Arguments</param>
        /// <returns>An initialized host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
