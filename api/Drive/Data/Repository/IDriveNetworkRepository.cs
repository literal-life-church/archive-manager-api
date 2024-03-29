﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;

internal interface IDriveNetworkRepository : IRepository
{
    Task<List<FileDomainModel>> GetAllFilesInFolderAsync(string driveId, string fileId);
    Task<List<FileDomainModel>> GetAllRootFilesSharedWithMeAsync(string myDriveId);
    Task<string> GetMyDriveIdAsync();
}
