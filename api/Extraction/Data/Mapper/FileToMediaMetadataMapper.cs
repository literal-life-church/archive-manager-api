using System;
using System.Collections.Generic;
using System.Linq;
using LiteralLifeChurch.ArchiveManagerApi.Config;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extensions;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Mapper;

internal class FileToMediaMetadataMapper : IFileToMediaMetadataMapper
{
    private readonly AuthenticationOptionsDomainModel _authenticationOptionsDomainModel;
    private readonly ICultureInfoForwarder _cultureInfoForwarder;
    private readonly IDateTimeForwarder _dateTimeForwarder;
    private readonly ILogger<FileToMediaMetadataMapper> _logger;

    public FileToMediaMetadataMapper(ICultureInfoForwarder cultureInfoForwarder,
        IOptions<AuthenticationOptionsDomainModel> authenticationEnvironmentVariableDomainModel,
        IDateTimeForwarder dateTimeForwarder,
        ILogger<FileToMediaMetadataMapper> logger)
    {
        _authenticationOptionsDomainModel = authenticationEnvironmentVariableDomainModel.Value;
        _cultureInfoForwarder = cultureInfoForwarder;
        _dateTimeForwarder = dateTimeForwarder;
        _logger = logger;
    }

    public List<MediaMetadataDomainModel> Map(List<FileDomainModel> input)
    {
        if (input.IsNullOrEmpty()) return new List<MediaMetadataDomainModel>();

        return input
            .Select(file =>
            {
                if (string.IsNullOrWhiteSpace(file.ParentFolderName)) return null;

                var parts = GetPartsOfFileName(file);
                var isPartOfNamedSeries = parts.Length == ExtractionConfig.MaxNumberOfPartsInMediaFileName;

                return new MediaMetadataDomainModel(
                    file.ParentFolderName,
                    GetDate(parts[0]),
                    GetSpeakers(parts[1]),
                    isPartOfNamedSeries ? GetSeriesOrTitle(parts[2]) : null,
                    isPartOfNamedSeries ? GetSeriesOrTitle(parts[3]) : GetSeriesOrTitle(parts[2])
                );
            })
            .WhereNotNull()
            .ToList();
    }

    private DateTime GetDate(string input)
    {
        var cleanInput = input.Trim().ToLower(GlobalConfig.LanguageCulture);
        string dateString;

        // Check the string length
        if (ExtractionConfig.DateTimeFormat.Length <= cleanInput.Length)
        {
            dateString = cleanInput[..ExtractionConfig.DateTimeFormat.Length];
        }
        else
        {
            var tooShortException = new DateTooShortFormatException(cleanInput);
            _logger.LogError(tooShortException, $"The date string '{cleanInput}' is too short.");

            throw tooShortException;
        }

        // Parse the date
        var extractedDateFromString = _dateTimeForwarder.TryParseExact(dateString, ExtractionConfig.DateTimeFormat,
            GlobalConfig.LanguageCulture, ExtractionConfig.DateTimeStyles, out var date);

        // Ensure the date string matches the configured format
        if (!extractedDateFromString)
        {
            var wrongFormatException = new IncorrectDateFormatException(cleanInput);
            _logger.LogError(wrongFormatException,
                $"The date string '{cleanInput}' does not match the configured date format {ExtractionConfig.DateTimeFormat}.");

            throw wrongFormatException;
        }

        // Indicates the lack of a modifier at the end of the date string
        if (cleanInput.Length <= ExtractionConfig.DateTimeFormat.Length) return date;

        // Look for date modifiers at the end
        var modifier = cleanInput[ExtractionConfig.DateTimeFormat.Length..].Trim();

        if (!ExtractionConfig.SupportedModifiers.ContainsKey(modifier))
        {
            var unsupportedDateModifierException = new UnsupportedDateModifierException(modifier, cleanInput);
            _logger.LogError(unsupportedDateModifierException,
                $"The date string '{cleanInput}' is using an invalid modifier '{modifier}'. Supported modifiers are {string.Join(", ", ExtractionConfig.SupportedModifiers.Keys)}.");

            throw unsupportedDateModifierException;
        }

        var hoursToAdd = ExtractionConfig.SupportedModifiers[modifier];
        date = date.AddHours(hoursToAdd);

        return date;
    }

    private string[] GetPartsOfFileName(FileDomainModel input)
    {
        var parts = input.NameWithoutExtension.Split(ExtractionConfig.MediaFileNameDelimiter);
        var acceptableNumberOfParts = ExtractionConfig.MinNumberOfPartsInMediaFileName <= parts.Length &&
                                      parts.Length <= ExtractionConfig.MaxNumberOfPartsInMediaFileName;

        if (acceptableNumberOfParts) return parts;

        var exception = new IncorrectNumberOfPartsException(input.Name, parts.Length);
        _logger.LogError(exception, $"Could not process file: {input.Name}.");

        throw exception;
    }

    private static List<string> GetSpeakers(string input)
    {
        var cleanInput = input.Trim();
        var speakers = cleanInput
            .Split(ExtractionConfig.SpeakerDelimiter)
            .Select(speaker => speaker.Trim())
            .ToList();

        speakers.Sort();
        return speakers;
    }

    private string GetSeriesOrTitle(string input)
    {
        return _cultureInfoForwarder.TextInfo.ToTitleCase(input.Trim().ToLower(GlobalConfig.LanguageCulture));
    }
}
