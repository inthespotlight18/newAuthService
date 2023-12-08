using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthModelLib
{
    public static class Helper
    {
        public static DataTable Tabulate(string json)
        {
            try
            {
                var jsonLinq = JObject.Parse(json);
                // Find the first array using Linq
                var srcArray = jsonLinq.Descendants().Where(d => d is JArray).FirstOrDefault();

                if (srcArray == null)
                    srcArray = new JArray();

                

                var trgArray = new JArray();
                foreach (JObject row in srcArray.Children<JObject>())
                {
                    var cleanRow = new JObject();
                    foreach (JProperty column in row.Properties())
                    {
                        // Only include JValue types
                        if (column.Value is JValue)
                        {
                            cleanRow.Add(column.Name, column.Value);
                        }
                    }

                    trgArray.Add(cleanRow);
                }
                return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}, Tabulate() FAILED.");
                return null;
            }

        }


    }
}
