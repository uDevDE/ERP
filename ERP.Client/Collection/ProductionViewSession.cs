using ERP.Client.Model;
using ERP.Client.Session;
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
        public List<ProjectPlantOrderSession> PlantOrderSession { get; private set; }

        public ProductionViewCollection()
        {
            PlantOrderSession = new List<ProjectPlantOrderSession>();
        }

        public async Task<bool> Load()
        {
            var json = await DeserializeFileAsync(JsonFile);
            if (json != null)
            {
                PlantOrderSession = JsonConvert.DeserializeObject<List<ProjectPlantOrderSession>>(json);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(PlantOrderSession);
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(JsonFile, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        public Task<bool> Add(ProjectPlantOrderSession session)
        {
            if (!PlantOrderSession.Exists(x => x.PlantOrder.Number == session.PlantOrder.Number))
            {
                PlantOrderSession.Add(session);
                return Save();
            }

            return Task.FromResult(false);
        }

        public Task<bool> Remove(ProjectPlantOrderSession session)
        {
            if (PlantOrderSession.Remove(session))
            {
                return Save();
            }

            return Task.FromResult(false);
        }

        public Task<bool> Remove(PlantOrderModel plantOrder)
        {
            bool result = false;
            for (int i = PlantOrderSession.Count - 1; i >= 0; i--)
            {
                if (PlantOrderSession[i].PlantOrder == plantOrder)
                {
                    PlantOrderSession.RemoveAt(i);
                    result = true;
                    break;
                }
            }

            if (result)
            {
                return Save();
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public ProjectPlantOrderSession Find(PlantOrderModel plantOrder)
        {
            foreach (var item in PlantOrderSession)
            {
                if (item.PlantOrder == plantOrder)
                {
                    return item;
                }
            }

            return null;
        }

        public Task<bool> Update(PlantOrderModel plantOrder, DivisionModel division)
        {
            if (PlantOrderSession != null)
            {
                foreach (var item in PlantOrderSession)
                {
                    if (item.PlantOrder.Id == plantOrder.Id)
                    {
                        item.Division = division;
                    }
                }
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
