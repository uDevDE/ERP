using ERP.Client.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace ERP.Client.Session
{
    public class ElementViewSession
    {
        private static readonly string JsonFile = "ElementViewSession.dat";

        public PlantOrderModel PlantOder { get; set; }
        public DivisionModel Divison { get; set; }

        public async static Task<ElementViewSession> LoadAsync()
        {
            var json = await DeserializeFileAsync(JsonFile);
            if (json != null)
            {
                var session = JsonConvert.DeserializeObject<ElementViewSession>(json);
                return await Task.FromResult(session);
            }

            return await Task.FromResult<ElementViewSession>(null);
        }

        public async static Task<bool> SaveAsync(ElementViewSession session)
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
