using System;
using System.IO;
using System.Text.Json;

namespace WalletApp.DataLayer.Configuration
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;

        public DatabaseConfiguration(string jsonFilePath)
        {
            LoadConfiguration(jsonFilePath);
        }

        private void LoadConfiguration(string jsonFilePath)
        {
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    throw new FileNotFoundException($"Configuration file not found: {jsonFilePath}");
                }

                string jsonContent = File.ReadAllText(jsonFilePath);
                var configData = JsonSerializer.Deserialize<ConfigurationData>(jsonContent);

                if (configData?.Database?.ConnectionString == null)
                {
                    throw new InvalidOperationException("ConnectionString not found in configuration file");
                }

                ConnectionString = configData.Database.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load configuration: {ex.Message}", ex);
            }
        }

        private class ConfigurationData
        {
            public DatabaseSection? Database { get; set; }
        }

        private class DatabaseSection
        {
            public string? ConnectionString { get; set; }
        }
    }
}