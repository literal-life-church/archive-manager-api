using LiteralLifeChurch.ArchiveManagerApi.Exceptions.Indexer.OneDriveMetadata;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using Microsoft.Graph;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.OneDriveMetadataModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer.Steps
{
    public class OneDriveMetadataStepService : IndexerStepService<ErrorType, IndexerOneDriveMetadataException, OneDriveMetadataModel>
    {
        private bool AssumedValue = false;

        public OneDriveMetadataStepService(ConfigurationModel config) : base(config)
        {
        }

        public override OneDriveMetadataModel Transform(DriveItem item, string split, int index)
        {
            long duration = ExtractDuration(item);
            long fileSize = ExtractFileSize(item);

            return new OneDriveMetadataModel
            {
                AssumedValue = AssumedValue,
                DriveId = item.ParentReference.DriveId,
                Duration = duration,
                FileSize = fileSize,
                FullPath = string.Format("{0}/{1}", item.ParentReference.Path, item.Name),
                Id = item.Id,
                MimeType = item.File.MimeType,
                Name = item.Name,
                NameWithoutExtension = RemoveExtension(item.Name),
                ParseError = ErrorType.None,
                Type = ExtractMediaType(item),
                WebUrl = item.WebUrl
            };
        }

        // region Helper Methods

        private long ExtractDuration(DriveItem item)
        {
            long duration = 0L;

            if (item.Audio != null && item.Audio.Duration != null && item.Audio.Duration != 0L)
            {
                duration = (long)item.Audio.Duration;
            }
            else if (item.Video != null && item.Video.Duration != null && item.Video.Duration != 0L)
            {
                duration = (long)item.Video.Duration;
            }
            else
            {
                AssumedValue = true;
            }

            return duration;
        }

        private long ExtractFileSize(DriveItem item)
        {
            long size = 0L;

            if (item.Size != null && item.Size != 0L)
            {
                size = (long)item.Size;
            }
            else
            {
                AssumedValue = true;
            }

            return size;
        }

        private MediaType ExtractMediaType(DriveItem item)
        {
            if (item.Audio != null)
            {
                return MediaType.Audio;
            }
            else if (item.Video != null)
            {
                return MediaType.Video;
            }
            else
            {
                return MediaType.Other;
            }
        }

        // endregion
    }
}
