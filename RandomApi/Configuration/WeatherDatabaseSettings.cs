﻿using EventDriven.DependencyInjection.URF.Mongo;

namespace RandomApi.Configuration
{
    public class WeatherDatabaseSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}
