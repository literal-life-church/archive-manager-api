using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Repository;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;

internal interface IDriveNetworkRepository : IRepository
{
    Task<string> GetMyDriveIdAsync();
}
