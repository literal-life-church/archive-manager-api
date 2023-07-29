using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.DataSource;
using Microsoft.Graph.Drives.Item.Items.Item.Children;
using Microsoft.Graph.Drives.Item.SharedWithMe;
using Microsoft.Graph.Me.Drive;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;

internal interface IDriveNetworkDataSource : IDataSource
{
    ChildrenRequestBuilder GetAllFilesInFolder(string driveId, string fileId);
    SharedWithMeRequestBuilder GetAllRootFilesSharedWithMe(string myDriveId);
    DriveRequestBuilder GetMyDrive();
}
