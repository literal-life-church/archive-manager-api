using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps
{
    public abstract class IndexerWorkflowStep<InputType, OutputType, ExceptionType> : IIndexerWorkflow where ExceptionType : IndexerWorkflowException
    {
        protected readonly ConfigurationModel Config;
        protected IndexerWorkflowErrorHandler<ExceptionType> ErrorHandler;

        protected IndexerWorkflowStep(ConfigurationModel config)
        {
            Config = config;
            ErrorHandler = new IndexerWorkflowErrorHandler<ExceptionType>(config);
        }

        public abstract OutputType Run(InputType item);

        // region Shared Helper Methods

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

        // endregion
    }
}
