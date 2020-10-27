using LiteralLifeChurch.ArchiveManagerApi.Exceptions.Indexer.Name;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using Microsoft.Graph;
using System.Globalization;
using System.Linq;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.NameModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer.Steps
{
    // Extract something like: 200901AM - John Smith - Overcoming the World.mp4
    //                                                 ^^^^^^^^^^^^^^^^^^^^

    public class NameStepService : IndexerStepService<ErrorType, IndexerNameException, NameModel>
    {
        private readonly NameModel DefaultNameModel;
        private ErrorType Error = ErrorType.None;

        public NameStepService(ConfigurationModel config) : base(config)
        {
            string defaultName = "Unknown Media Name";

            DefaultNameModel = new NameModel
            {
                AssumedValue = true,
                Given = defaultName,
                Id = GenerateId(defaultName),
                ParseError = ErrorType.NoName,
                Normalized = defaultName
            };
        }

        public override NameModel Transform(DriveItem item, string split, int index)
        {
            Error = ErrorType.None;
            string givenName = RemoveExtension(item.Name.Split(split).ElementAtOrDefault(index).Trim());

            if (string.IsNullOrWhiteSpace(givenName))
            {
                Error = ErrorType.NoName;

                return ErrorHandler
                    .SetExceptionType(new NoNameException())
                    .SetDeveloperMessage("The given media name segement is completely empty.")
                    .SetPublicMessage("The given media name segement is completely empty.")
                    .ThrowIfAllowed()
                    .OrDefault(DefaultNameModel);
            }

            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            string normalizedName = textinfo.ToTitleCase(givenName);

            return new NameModel
            {
                AssumedValue = Error != ErrorType.None,
                Given = givenName,
                Id = GenerateId(normalizedName),
                Normalized = normalizedName,
                ParseError = Error
            };
        }
    }
}
