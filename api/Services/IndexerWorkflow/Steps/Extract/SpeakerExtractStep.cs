using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Extract.Speaker;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.SpeakerModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract
{
    // Extract something like: 200901AM - John Smith - Overcoming the World.mp4
    //                                    ^^^^^^^^^^
    // or
    // 200901AM - John Smith;Jane Doe - Overcoming the World.mp4
    //            ^^^^^^^^^^ ^^^^^^^^
    // 
    // or
    // 200901AM - John Smith; Jane Doe - Overcoming the World.mp4
    //            ^^^^^^^^^^  ^^^^^^^^
    // 
    // or
    // 200901AM - Bro. John Smith; Sis. Jane Doe - Overcoming the World.mp4
    //                 ^^^^^^^^^^       ^^^^^^^^
    // 
    // or
    // 200901AM - Brother John Smith; Jane Doe - Overcoming the World.mp4
    //                    ^^^^^^^^^^  ^^^^^^^^

    public class SpeakerExtractStep : IndexerWorkflowStep<DriveItem, SpeakerModel, SpeakerException>
    {
        private readonly string DefaultName;
        private readonly List<string> DefaultNames;
        private readonly SpeakerModel DefaultSpeakerModel;

        private ErrorType Error = ErrorType.None;
        private readonly int Index;
        private readonly string Split;

        public SpeakerExtractStep(ConfigurationModel config, string split, int index) : base(config)
        {
            Index = index;
            Split = split;

            DefaultName = "Unknown Speaker";
            DefaultNames = new List<string> {
                DefaultName
            };

            DefaultSpeakerModel = new SpeakerModel
            {
                AssumedValue = true,
                Id = GenerateId(DefaultName),
                ParseError = ErrorType.NoNames,
                Names = new List<SpeakerNameModel>
                {
                    new SpeakerNameModel
                    {
                        Given = DefaultName,
                        Id = GenerateId(DefaultName),
                        Normalized = DefaultName
                    }
                }
            };
        }

        public override SpeakerModel Run(DriveItem item)
        {
            Error = ErrorType.None;
            string namesOnly = item.Name.Split(Split).ElementAtOrDefault(Index).Trim();

            if (string.IsNullOrWhiteSpace(namesOnly))
            {
                Error = ErrorType.NoNames;

                return ErrorHandler
                    .SetExceptionType(new NoNamesException())
                    .SetDeveloperMessage("The given speaker name segement is completely empty.")
                    .SetPublicMessage("The given speaker name segement is completely empty.")
                    .ThrowIfAllowed()
                    .OrDefault(DefaultSpeakerModel);
            }

            List<string> givenNames = ExtractNames(namesOnly);
            List<SpeakerNameModel> speakerNames = new List<SpeakerNameModel>();
            StringBuilder speakerStringBuilder = new StringBuilder();

            foreach (string name in givenNames)
            {
                string normalized = NormalizeName(name);
                string lettersOnly = NameWithLettersOnly(normalized);

                speakerNames.Add(new SpeakerNameModel
                {
                    Given = name,
                    Id = GenerateId(lettersOnly),
                    Normalized = normalized
                });
            }

            speakerNames = speakerNames.OrderBy(speaker => speaker.Normalized).ToList();

            foreach (SpeakerNameModel speaker in speakerNames)
            {
                speakerStringBuilder.Append(speaker.Normalized);
            }

            return new SpeakerModel
            {
                AssumedValue = Error != ErrorType.None,
                Id = GenerateId(speakerStringBuilder.ToString()),
                ParseError = Error,
                Names = speakerNames
            };
        }

        // region Helper Methods

        private List<string> ExtractNames(string names)
        {
            List<string> extractedNames = names.Split(";").Select(name => name.Trim()).ToList();

            foreach (string name in extractedNames)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    Error = ErrorType.EmptyName;

                    return ErrorHandler
                        .SetExceptionType(new EmptyNameException())
                        .SetDeveloperMessage($"The given speaker name segement '{names}' contains an empty name.")
                        .SetPublicMessage($"The '{names}' in the speaker name segement includes an empty name.")
                        .ThrowIfAllowed()
                        .OrDefault(DefaultNames);
                }
            }

            extractedNames.Sort();
            return extractedNames;
        }

        private string NameWithLettersOnly(string name)
        {
            return Regex.Replace(name, "[^A-Za-z]*", "").ToLowerInvariant();
        }

        private string NormalizeName(string name)
        {
            // Regex vetted here: https://regex101.com/r/GaBIgH/1
            string nameOnly = Regex.Replace(name, @"^(bro(ther)?\.?)|(sis(ter)?\.?)", "", RegexOptions.IgnoreCase).Trim();

            if (string.IsNullOrWhiteSpace(nameOnly))
            {
                Error = ErrorType.EmptyNormalizedName;

                return ErrorHandler
                    .SetExceptionType(new EmptyNormalizedNameException())
                    .SetDeveloperMessage("Whenever normalizing one of the given speaker name segements, it resulted in an empty value.")
                    .SetPublicMessage("The given speaker name segement includes an empty name.")
                    .ThrowIfAllowed()
                    .OrDefault(DefaultName);
            }

            return nameOnly;
        }

        // endregion
    }
}
