using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Crawler
{
    public class FileListService : ICrawlerService
    {
        private readonly ConfigurationModel Config;

        public FileListService(ConfigurationModel config)
        {
            Config = config;
        }

        public async Task<List<DriveItem>> ListAllMediaItems()
        {
            return await GetChildrenInAllSharedFolders();
        }

        // region Helper Methods

        private async Task<List<DriveItem>> GetChildrenInAllSharedFolders()
        {
            // Shares
            IDriveSharedWithMeCollectionPage sharesPage = await AuthenticationService.GetSharedDrivesAsync(Config);
            IList<DriveItem> shares = sharesPage.CurrentPage;

            // Years
            List<DriveItem> years = await GetAllChildrenInFolders(shares, driveItem =>
            {
                return ShouldProcessYear(driveItem);
            });

            // Months
            List<DriveItem> months = await GetAllChildrenInFolders(years, driveItem =>
            {
                return ShouldProcessMonth(driveItem);
            });

            // Categories
            List<DriveItem> categories = await GetAllChildrenInFolders(months, driveItem =>
            {
                return ShouldProcessCategory(driveItem);
            });

            // Media items
            List<DriveItem> media = await GetAllChildrenInFolders(categories, driveItem =>
            {
                return driveItem.File != null; // Only process files
            });

            LoggerService.Info($"Crawler returned {media.Count} item(s)", LoggerService.Crawler);
            return media;
        }

        private async Task<List<DriveItem>> GetAllChildrenInFolders(IList<DriveItem> folders, Func<DriveItem, bool> shouldProcessPredicate)
        {
            List<DriveItem> response = new List<DriveItem>();

            foreach (DriveItem folder in folders)
            {
                IDriveItemChildrenCollectionPage itemsPage = await AuthenticationService.GetChildrenAsync(Config, folder);
                IList<DriveItem> items = itemsPage.CurrentPage;

                foreach (DriveItem item in items)
                {
                    if (!shouldProcessPredicate(item))
                    {
                        continue;
                    }

                    response.Add(item);
                }
            }

            return response;
        }

        private bool ShouldProcessCategory(DriveItem item)
        {
            if (item.Folder == null)
            {
                return false;
            }

            string folderName = item.Name.Trim().ToLowerInvariant();
            return folderName == Config.FolderFullService ||
                folderName == Config.FolderFullServiceAudio ||
                folderName == Config.FolderPreaching ||
                folderName == Config.FolderSpecials;
        }

        private bool ShouldProcessMonth(DriveItem item)
        {
            if (item.Folder == null)
            {
                return false;
            }

            string folderName = item.Name.Trim().ToLowerInvariant();
            string[] parts = folderName.Split("-");

            if (parts.Length != 2)
            {
                return false;
            }

            try
            {
                int monthNumber = Convert.ToInt32(parts[0].Trim());

                if (monthNumber < 1 || monthNumber > 12)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            /*string[] monthNames = new string[] {
                "january", "february", "march", "april", "may", "june", "july",
                "august", "september", "october", "november", "december"
            };*/

            string[] monthNames = new string[] {
                "january"
            };

            return monthNames.Contains(parts[1].Trim());
        }

        private bool ShouldProcessYear(DriveItem item)
        {
            if (item.Folder == null)
            {
                return false;
            }

            try
            {
                int lowerYear = Config.ValidYearLowerBound;
                int upperYear = DateTime.Today.Year;
                
                string folderName = item.Name.Trim().ToLowerInvariant();
                int mappedYear = Convert.ToInt32(folderName);
                return mappedYear == 2020; // TODO revert
                // return lowerYear <= mappedYear && mappedYear <= upperYear;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // endregion
    }
}
