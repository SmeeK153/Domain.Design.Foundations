using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Tests.Foundations.Infrastructure.TestApplication
{
    public class Program
    {
        public static IWebHostBuilder CreateHostBuilder([AllowNull] string[] args = null) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}