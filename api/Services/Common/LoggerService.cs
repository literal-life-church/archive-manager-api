﻿using Microsoft.Extensions.Logging;
using Sentry;
using Sentry.Protocol;
using System;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Common
{
    public class LoggerService : ICommonService
    {
        // region Categories

        public static readonly string Bootstrapping = "bootstrapping";
        public static readonly string Crawl = "crawl";

        // endregion

        private static ILogger Logger;

        public static IDisposable Init(ILogger log)
        {
            Logger = log;
            return SentrySdk.Init();
        }

        public static void CaptureException(System.Exception e)
        {
            SentrySdk.CaptureException(e);
            Logger.LogCritical(e.Message);
        }

        public static void Error(string message, string category)
        {
            SentrySdk.AddBreadcrumb(message, category, level: BreadcrumbLevel.Error);
            Logger.LogError(message);
        }

        public static void Info(string message, string category)
        {
            SentrySdk.AddBreadcrumb(message, category, level: BreadcrumbLevel.Info);
            Logger.LogInformation(message);
        }

        public static void Warn(string message, string category)
        {
            SentrySdk.AddBreadcrumb(message, category, level: BreadcrumbLevel.Warning);
            Logger.LogWarning(message);

        }
    }
}
