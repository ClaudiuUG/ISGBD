using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KeyValueDatabaseApi.Commands;
using KeyValueDatabaseApi.Exceptions;
using KeyValueDatabaseApi.Models;
using Newtonsoft.Json;

namespace KeyValueDatabaseApi.Context
{
    public class DbContext
    {
        private const string MetadataFilePath = @"C:\Projects\KeyValueDatabase\KeyValueDatabaseApi\DatabaseStorage\Metadata.json";

        public Metadata DatabaseMetadata { get; set; }

        private static DbContext _dbContext = new DbContext();

        public static DbContext GetDbContext()
        {
            return _dbContext;
        }

        private DbContext()
        {
            LoadMetadataFile();
        }

        public IEnumerable<string> Databases { get; }

        public DatabaseMetadataEntry CurrentDatabase { get; set; }

        private void LoadMetadataFile()
        {
            DatabaseMetadata = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(MetadataFilePath));
            if (DatabaseMetadata == null)
            {
                DatabaseMetadata = new Metadata(new List<DatabaseMetadataEntry>());
            }
        }

        public void SaveMetadataToFile()
        {
            var serializedDatabaseMetadata = JsonConvert.SerializeObject(DatabaseMetadata);
            File.WriteAllText(MetadataFilePath, serializedDatabaseMetadata);
        }

        public void UseDatabase(string databaseName)
        {
            _dbContext.CurrentDatabase = GetDatabase(databaseName);
        }

        public DatabaseMetadataEntry GetDatabase(string databaseName)
        {
            var matchedDatabase = DatabaseMetadata.Databases.SingleOrDefault(database => database.DatabaseName.Equals(databaseName));
            return matchedDatabase ?? throw new DataBaseDoesNotExistException();
        }

        public TableMetadataEntry GetTableFromCurrentDatabase(string tableName)
        {
            var foundTable = CurrentDatabase.Tables.SingleOrDefault(table => table.TableName.Equals(tableName));
            return foundTable ?? throw new TableDoesNotExistException(CurrentDatabase.DatabaseName, tableName);
        }
    }
}