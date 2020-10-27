using LiteralLifeChurch.ArchiveManagerApi.Exceptions;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer.Steps
{
    public abstract class IndexerStepService<ErrorType, ExceptionType, Model> : IIndexerService where ExceptionType : AppException where Model : IndexerModel<ErrorType>
    {
        protected readonly ConfigurationModel Config;
        protected ErrorHandlerService<ExceptionType> ErrorHandler;

        protected IndexerStepService(ConfigurationModel config)
        {
            Config = config;
            ErrorHandler = new ErrorHandlerService<ExceptionType>(config);
        }

        // From: https://stackoverflow.com/a/17001289
        protected string GenerateId(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using SHA256 hash = SHA256.Create();
            byte[] hashedInputBytes = hash.ComputeHash(inputBytes);
            StringBuilder hashedInputStringBuilder = new StringBuilder(128);

            foreach (byte inputByte in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(inputByte.ToString("X2"));
            }

            return hashedInputStringBuilder.ToString();
        }

        protected string RemoveExtension(string fileName)
        {
            List<string> parts = fileName.Split(".").ToList();

            if (parts.Count <= 1)
            {
                return fileName;
            }
            else
            {
                return string.Join(".", parts.SkipLast(1));
            }
        }

        public abstract Model Transform(DriveItem item, string split, int index);
    }
}
