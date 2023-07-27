using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.DataSource;
using Microsoft.Graph.Me.Drive;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;

internal interface IDriveNetworkDataSource : IDataSource
{
    DriveRequestBuilder GetMyDrive();
}
