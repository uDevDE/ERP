using ERP.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace ERP.Client.Collection
{
    public class ProductionViewCollection
    {
        private static readonly string JsonFile = "ProductionViewCollection.dat";
        public List<PlantOrderModel> PlantOrderCollection { get; private set; }

        public ProductionViewCollection()
        {
            PlantOrderCollection = new List<PlantOrderModel>();
        }

        public async Task<bool> Load()
        {
            var json = await DeserializeFileAsync(JsonFile);
            if (json != null)
            {
                PlantOrderCollection = JsonConvert.DeserializeObject<List<PlantOrderModel>>(json);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(PlantOrderCollection);
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(JsonFile, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        public Task<bool> Add(PlantOrderModel plantOrder)
        {
            if (!PlantOrderCollection.Exists(x => x.Number == plantOrder.Number))
            {
                PlantOrderCollection.Add(plantOrder);
                return Save();
            }

            return Task.FromResult(false);
        }

        public Task<bool> Remove(PlantOrderModel plantOrder)
        {
            if (PlantOrderCollection.Remove(plantOrder))
            {
                return Save();
            }

            return Task.FromResult(false);
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

    }
}
