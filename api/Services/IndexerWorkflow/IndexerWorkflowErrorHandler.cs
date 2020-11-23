using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using System.Net;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping.ConfigurationModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow
{
    public class IndexerWorkflowErrorHandler<ExceptionType> : IIndexerWorkflow where ExceptionType : IndexerWorkflowException
    {
        private readonly ConfigurationModel Config;
        private string DeveloperMessage;
        private string PublicMessage;
        private HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
        private ExceptionType Type;

        public IndexerWorkflowErrorHandler(ConfigurationModel config)
        {
            Config = config;
        }

        public DefaultModel OrDefault<DefaultModel>(DefaultModel model)
        {
            return model;
        }

        public IndexerWorkflowErrorHandler<ExceptionType> SetDeveloperMessage(string message)
        {
            DeveloperMessage = message;
            return this;
        }

        public IndexerWorkflowErrorHandler<ExceptionType> SetExceptionType(ExceptionType type)
        {
            Type = type;
            return this;
        }

        public IndexerWorkflowErrorHandler<ExceptionType> SetPublicMessage(string message)
        {
            PublicMessage = message;
            return this;
        }

        public IndexerWorkflowErrorHandler<ExceptionType> SetStatusCode(HttpStatusCode status)
        {
            StatusCode = status;
            return this;
        }

        public IndexerWorkflowErrorHandler<ExceptionType> ThrowIfAllowed()
        {
            if (Config.FaultResponse != FaultResponseType.Default)
            {
                Type.DeveloperMessage = DeveloperMessage;
                Type.Message = PublicMessage;
                Type.Status = StatusCode;

                throw Type;
            }

            return this;
        }
    }
}
