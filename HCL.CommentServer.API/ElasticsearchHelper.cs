﻿using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace HCL.CommentServer.API
{
    public static class ElasticsearchHelper
    {
        public static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
                .Enrich.WithProperty("Environment", environment!)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            string uriString = Environment.GetEnvironmentVariable("ElasticConfiguration__Uri")
                ?? configuration["ElasticConfiguration:Uri"];

            return new ElasticsearchSinkOptions(new Uri(uriString))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}