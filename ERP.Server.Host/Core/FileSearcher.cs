using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using ERP.Contracts.Domain;
using ERP.Contracts.Domain.Core.Enums;

namespace ERP.Server.Host.Core
{
    public class FileSearcher
    {
        public string BaseDirectory { get; private set; }
        public string Pattern { get; private set; }
        public SearchOption Option { get; private set; }

        public FileSearcher(string baseDirectory, string pattern, SearchOption option)
        {
            BaseDirectory = baseDirectory;
            Pattern = pattern;
            Option = option;
        }

        public Task<FolderDTO> SearchAsync()
        {
            return Task.Run(async () =>
            {
                var name = new DirectoryInfo(BaseDirectory).Name;
                var rootFolder = new FolderDTO(name, name, false, false, true);
                var directories = Directory.GetDirectories(BaseDirectory);
                var basePath = Path.GetDirectoryName(BaseDirectory);

                foreach (var directory in directories)
                {
                    var rel = MakeRelative(basePath, directory);
                    var subDirectoryInfo = new DirectoryInfo(directory);

                    var folder = new FolderDTO(subDirectoryInfo.Name, rel, true);
                    var subFolders = await GetFolder(basePath, subDirectoryInfo);                    
                    foreach (var subFolder in subFolders)
                    {
                        try
                        {
                            folder.SubFolders.Add(Path.Combine(rel, subFolder.Name), subFolder);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    rootFolder.SubFolders.Add(Path.Combine(rel, folder.Name), folder);
                }

                return rootFolder;
            });
        }

        private Task<List<FolderDTO>> GetFolder(string basePath, DirectoryInfo directoryInfo)
        {
            return Task.Run( () =>
            {
                var directories = directoryInfo.GetDirectories();
                var result = new List<FolderDTO>();
                foreach (var directory in directories)
                {
                    var rel = MakeRelative(basePath, directory.FullName);
                    var folder = new FolderDTO(directory.Name, rel, false, true);
                    var files = GetFiles(directory, rel);

                    foreach (var file in files)
                    {                     
                        try
                        {
                            folder.Files.Add(file.FilePath, file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message} - File: {file.FilePath}");
                        }
                    }

                    result.Add(folder);
                }

                return result;
            });
        }

        private List<FileEntryDTO> GetFiles(DirectoryInfo directoryInfo, string relativePath)
        {
            var files = directoryInfo.GetFiles(Pattern, SearchOption.TopDirectoryOnly);
            var result = new List<FileEntryDTO>();

            foreach (var file in files)
            {
                var fileEntryInfo = GetFileEntryInfo(file);
                var fileEntry = new FileEntryDTO(file.Name, relativePath, fileEntryInfo);
                result.Add(fileEntry);
            }

            return result;
        }

        private static FileEntryInfoDTO GetFileEntryInfo(FileInfo fileInfo)
        {
            const int COUNT = 15;

            var match = ValidateJobFile(fileInfo.Name);
            if (match.Success && match.Groups.Count == COUNT)
            {
                if (int.TryParse(match.Groups[1].Value, out int number))
                {
                    string projectIdentifier;
                    if (!string.IsNullOrEmpty(match.Groups[8].Value.Trim()))
                        projectIdentifier = string.Format("{0:s}-{1:s}-{2:s}{3:s}", match.Groups[3].Value, match.Groups[5].Value, match.Groups[7].Value, match.Groups[8].Value);
                    else
                        projectIdentifier = string.Format("{0:s}-{1:s}-{2:s}", match.Groups[3].Value, match.Groups[5].Value, match.Groups[7].Value);

                    return new FileEntryInfoDTO()
                    {
                        ProjectNumber = number,
                        Direction = match.Groups[2].Value.Replace('-', ' ').Trim(),
                        ProjectIdentifier = projectIdentifier,
                        Section = string.Format("{0:s}{1:s}{2:s}", match.Groups[9].Value, match.Groups[10].Value, match.Groups[11].Value),
                        Contraction = match.Groups[12].Value.Trim(),
                        Description = match.Groups[13].Value,
                        Extension = GetExtensionType(match.Groups[14].Value)
                    };
                }
            }

            return null;
        }

        private static FileEntryExtensionType GetExtensionType(string extension)
        {
            switch (extension.ToLower())
            {
                case "pdf":  default: return FileEntryExtensionType.Pdf;
                case "srw": return FileEntryExtensionType.Srw;
                case "enc": return FileEntryExtensionType.Enc;
            }
        }

        private static string MakeRelative(string basePath, string fullPath)
        {
            var root = new Uri(basePath);
            return root.MakeRelativeUri(new Uri(fullPath)).ToString();
        }

        private static Match ValidateJobFile(string filenameWithExtension) => 
            Regex.Match(filenameWithExtension, @"([0-9]{4})(-[\w]{1})?___(\d+)(-|.)(\d+)(-|.)(\d+)(-\d+)?-(MP|MPX)(\d+)(-\d+)?-([\w]{2,3})(.*?)\.([A-Z]{3})", RegexOptions.IgnoreCase);

    }
}
