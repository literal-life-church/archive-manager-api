using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.Children;
using Microsoft.Graph.Drives.Item.SharedWithMe;
using Microsoft.Graph.Me.Drive;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;

internal class DriveNetworkDataSource : IDriveNetworkDataSource
{
    private readonly GraphServiceClient _client;

    public DriveNetworkDataSource(IGetAuthenticatedClientUseCase getAuthenticatedClientUseCase)
    {
        _client = getAuthenticatedClientUseCase.Execute();
    }

    public ChildrenRequestBuilder GetAllFilesInFolder(string driveId, string fileId)
    {
        return _client.Drives[driveId].Items[fileId].Children;
    }

    public SharedWithMeRequestBuilder GetAllRootFilesSharedWithMe(string myDriveId)
    {
        return _client.Drives[myDriveId].SharedWithMe;
    }

    public DriveRequestBuilder GetMyDrive()
    {
        return _client.Me.Drive;
    }
}
