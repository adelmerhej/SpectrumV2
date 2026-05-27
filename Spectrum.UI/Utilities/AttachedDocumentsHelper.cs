using Spectrum.Models.Common.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Spectrum.Utilities
{
    public static class AttachedDocumentsHelper
    {
        public static string ResolveRootFolder(string configuredFolder, string defaultSubFolder)
        {
            return string.IsNullOrWhiteSpace(configuredFolder)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpectrumApp", defaultSubFolder)
                : configuredFolder;
        }

        public static string BuildArchivedFileName(string recordId, string originalFileName)
        {
            if (string.IsNullOrWhiteSpace(recordId))
                return originalFileName;

            return string.Concat(recordId, "_", originalFileName ?? string.Empty);
        }

        public static string GetDisplayDocumentName(string recordId, string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var prefix = (recordId ?? string.Empty) + "_";

            return fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? fileName.Substring(prefix.Length)
                : fileName;
        }

        public static BindingList<DocumentModel> LoadDocumentsByRecordPrefix(string rootFolder, string recordId)
        {
            var documents = new BindingList<DocumentModel>();

            if (string.IsNullOrWhiteSpace(rootFolder) || string.IsNullOrWhiteSpace(recordId) || !Directory.Exists(rootFolder))
                return documents;

            var prefix = recordId + "_";
            IEnumerable<string> files;

            try
            {
                files = Directory.GetFiles(rootFolder)
                    .Where(path => Path.GetFileName(path).StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return documents;
            }

            foreach (var filePath in files)
            {
                try
                {
                    documents.Add(new DocumentModel
                    {
                        DocumentName = GetDisplayDocumentName(recordId, filePath),
                        OriginPath = filePath,
                        DocumentDate = File.GetCreationTime(filePath),
                        StreamedDate = DateTime.Now,
                        DocumentContent = File.ReadAllBytes(filePath)
                    });
                }
                catch
                {
                }
            }

            return documents;
        }

        public static void OpenDocument(string documentPath)
        {
            if (string.IsNullOrWhiteSpace(documentPath) || !File.Exists(documentPath))
                return;

            Process.Start(documentPath);
        }
    }
}
