using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VideoGameCollector.Web
{
    public class Startup
    {
        private string apiUrl = "https://api-endpoint.igdb.com";
        private string apiKey = ""; // free api key available at https://api.igdb.com/signup

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // Basic usage
            services.AddHttpClient();
            // Named instance
            services.AddHttpClient("IgdbNamedClient", client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("user-key", apiKey);
            });
            // Typed instance
            services.AddHttpClient<IgdbClient>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("user-key", apiKey); // GitHub requires a user-agent
            });            
            // Message handler
            services.AddTransient<ValidateUserKeyHandler>();
            services.AddHttpClient<IgdbClient>().AddHttpMessageHandler<ValidateUserKeyHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
