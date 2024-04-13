namespace BitirmeProjesiUI.Services
{
    public class ConfigureService
    {
        private static IConfigurationRoot _configuration;

        static ConfigureService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        public static string ConnectionString
        {
            get
            {
                return _configuration.GetConnectionString("MsSqlConnection");
            }
        }

    }
}
