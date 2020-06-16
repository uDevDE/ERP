using ERP.Client.Summaries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace ERP.Client.Session
{
    public class ProjectViewSession
    {
        private static readonly string JsonFile = "ProjectViewSession.dat";

        public List<ProjectSummary> Summaries { get; set; }

        public Task<bool> Add(ProjectSummary summary)
        {
            if (Summaries == null)
            {
                Summaries = new List<ProjectSummary>
                {
                    summary
                };
                return SaveAsync(this);
            }
            else
            {
                if (Exists(summary))
                {
                    return Task.FromResult(false);
                }

                Summaries.Add(summary);
                return SaveAsync(this);
            }
        }

        public Task<bool> Remove(ProjectSummary summary)
        {
            if (Summaries == null)
            {
                return Task.FromResult(false);
            }

            if (Exists(summary))
            {
                var removed = Summaries.Remove(summary);
                if (removed)
                {
                    return SaveAsync(this);
                }
            }

            return Task.FromResult(false);
        }

        public bool Exists(ProjectSummary summary)
        {
            foreach (var item in Summaries)
            {
                if (item.PlantOrder?.Id == summary.PlantOrder?.Id)
                {
                    return true;
                }
            }

            return false;
        }

        public async static Task<ProjectViewSession> LoadAsync()
        {
            var json = await DeserializeFileAsync(JsonFile);
            if (json != null)
            {
                var session = JsonConvert.DeserializeObject<ProjectViewSession>(json);
                return await Task.FromResult(session);
            }

            return await Task.FromResult<ProjectViewSession>(null);
        }

        public async static Task<bool> SaveAsync(ProjectViewSession session)
        {
            try
            {
                var json = JsonConvert.SerializeObject(session, Formatting.Indented);
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(JsonFile, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        private static async Task<string> DeserializeFileAsync(string fileName)
        {
            try
            {
                var localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localFile);
            }
            catch (FileNotFoundException)
            {
                return await Task.FromResult<string>(null);
            }
        }

        public async static Task<bool> FileExistsAsync()
        {
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(JsonFile);
            return file != null ? await Task.FromResult(true) : await Task.FromResult(false);
        }

    }
}
