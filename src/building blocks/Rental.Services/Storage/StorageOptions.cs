﻿namespace Rental.Services.Storage
{
    public class StorageOptions
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public bool UseSSL { get; set; } = false;
        public int PresignedExpirySeconds { get; set; } = 3600;
    }
}
