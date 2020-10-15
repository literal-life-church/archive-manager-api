using LiteralLifeChurch.ArchiveManagerApi.Exceptions;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using System.Net;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping.ConfigurationModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer
{
    public class ErrorHandlerService<ExceptionType> : IIndexerService where ExceptionType : AppException
    {
        private readonly ConfigurationModel Config;
        private string DeveloperMessage;
        private string PublicMessage;
        private HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
        private ExceptionType Type;

        public ErrorHandlerService(ConfigurationModel config)
        {
            Config = config;
        }

        public DefaultModel OrDefault<DefaultModel>(DefaultModel model)
        {
            return model;
        }

        public ErrorHandlerService<ExceptionType> SetDeveloperMessage(string message)
        {
            DeveloperMessage = message;
            return this;
        }

        public ErrorHandlerService<ExceptionType> SetExceptionType(ExceptionType type)
        {
            Type = type;
            return this;
        }

        public ErrorHandlerService<ExceptionType> SetPublicMessage(string message)
        {
            PublicMessage = message;
            return this;
        }

        public ErrorHandlerService<ExceptionType> SetStatusCode(HttpStatusCode status)
        {
            StatusCode = status;
            return this;
        }

        public ErrorHandlerService<ExceptionType> ThrowIfAllowed()
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
