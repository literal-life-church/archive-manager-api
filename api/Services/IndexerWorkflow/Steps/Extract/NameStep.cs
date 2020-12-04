using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Extract.Name;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;
using Microsoft.Graph;
using System.Globalization;
using System.Linq;
using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.NameModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract
{
    // Extract something like: 200901AM - John Smith - Overcoming the World.mp4
    //                                                 ^^^^^^^^^^^^^^^^^^^^

    public class NameStep : IndexerWorkflowStep<DriveItem, NameModel, NameException>
    {
        private readonly NameModel DefaultNameModel;

        private ErrorType Error = ErrorType.None;
        private readonly int Index;
        private readonly string Split;

        public NameStep(ConfigurationModel config, string split, int index) : base(config)
        {
            Index = index;
            Split = split;

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

        public override NameModel Run(DriveItem item)
        {
            Error = ErrorType.None;
            string givenName = RemoveExtension(item.Name.Split(Split).ElementAtOrDefault(Index));

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

            givenName = givenName.Trim();

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
