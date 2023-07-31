using System;
using System.Collections.Generic;
using System.Linq;
using LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Mapper;

internal class FileToMediaMetadataMapper : IFileToMediaMetadataMapper
{
    private readonly IDateTimeForwarder _dateTimeForwarder;
    private readonly ILogger<FileToMediaMetadataMapper> _logger;

    public FileToMediaMetadataMapper(IDateTimeForwarder dateTimeForwarder,
        ILogger<FileToMediaMetadataMapper> logger)
    {
        _dateTimeForwarder = dateTimeForwarder;
        _logger = logger;
    }

    public List<MediaMetadataDomainModel> Map(List<FileDomainModel> input)
    {
        if (input.IsNullOrEmpty()) return new List<MediaMetadataDomainModel>();

        return input.Select(file =>
            {
                var parts = GetPartsOfFileName(file);
                var isPartOfNamedSeries = parts.Length == ExtractionConfig.MaxNumberOfPartsInMediaFileName;

                return new MediaMetadataDomainModel
                {
                    Date = GetDate(parts[0]),
                    Speakers = GetSpeakers(parts[1]),
                    Title = isPartOfNamedSeries ? GetTitle(parts[3]) : GetTitle(parts[2])
                };
            })
            .ToList();
    }

    private string[] GetPartsOfFileName(FileDomainModel input)
    {
        var parts = input.NameWithoutExtension.Split(ExtractionConfig.MediaFileNameDelimiter);
        var acceptableNumberOfParts = ExtractionConfig.MinNumberOfPartsInMediaFileName <= parts.Length &&
                                      parts.Length <= ExtractionConfig.MaxNumberOfPartsInMediaFileName;

        if (acceptableNumberOfParts) return parts;

        var exception = new IncorrectNumberOfPartsException(input.Name, parts.Length);
        _logger.LogError(exception, $"Could not process {input.Name}.");

        throw exception;
    }

    private DateTime GetDate(string input)
    {
        var cleanInput = input.Trim().ToLower(ExtractionConfig.LanguageCulture);
        string dateString;

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

        var extractedDateFromString = _dateTimeForwarder.TryParseExact(dateString, ExtractionConfig.DateTimeFormat,
            ExtractionConfig.LanguageCulture, ExtractionConfig.DateTimeStyles, out var date);

        if (extractedDateFromString) return date;

        var wrongFormatException = new IncorrectDateFormatException(cleanInput);
        _logger.LogError(wrongFormatException,
            $"The date string '{cleanInput}' does not match the configured date format {ExtractionConfig.DateTimeFormat}.");

        throw wrongFormatException;
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

    private static string GetTitle(string input)
    {
        return input.Trim();
    }
}
