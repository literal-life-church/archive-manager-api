using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using Microsoft.Graph;
using Microsoft.Graph.Me.Drive;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;

internal class DriveNetworkDataSource : IDriveNetworkDataSource
{
    private readonly GraphServiceClient _client;

    public DriveNetworkDataSource(IGetAuthenticatedClientUseCase getAuthenticatedClientUseCase)
    {
        _client = getAuthenticatedClientUseCase.Execute();
    }

    public DriveRequestBuilder GetMyDrive()
    {
        return _client.Me.Drive;
    }
}
