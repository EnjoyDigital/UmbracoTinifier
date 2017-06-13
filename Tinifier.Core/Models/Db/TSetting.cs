﻿using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Tinifier.Core.Models
{
    [TableName(PackageConstants.DbSettingsTable)]
    public class TSetting
    {
        [PrimaryKeyColumn(AutoIncrement = true, Clustered = true)]
        public int Id { get; set; }

        public string ApiKey { get; set; }

        public bool EnableOptimizationOnUpload { get; set; }
    }
}