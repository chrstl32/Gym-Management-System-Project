using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace GymDesktopApp
{
    public class ApiResult
    {
        public bool Success;
        public string Message = "";
        public DataTable Data = new DataTable();
    }

    public static class Api
    {
        public static string BaseUrl = "http://localhost/gym_management_system/api/api.php";

        private static readonly HttpClient Client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public static async Task<ApiResult> Get(string action, string table = "")
        {
            try
            {
                string url = BaseUrl + "?action=" + Uri.EscapeDataString(action);

                if (table != "")
                {
                    url += "&table=" + Uri.EscapeDataString(table);
                }

                string raw = await Client.GetStringAsync(url);
                return Parse(raw);
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Success = false,
                    Message = "Connection error: " + ex.Message
                };
            }
        }

        public static async Task<ApiResult> Post(string action, string table, Dictionary<string, string> data)
        {
            try
            {
                data["action"] = action;

                if (table != "")
                {
                    data["table"] = table;
                }

                using (FormUrlEncodedContent content = new FormUrlEncodedContent(data))
                {
                    using (HttpResponseMessage response = await Client.PostAsync(BaseUrl, content))
                    {
                        string raw = await response.Content.ReadAsStringAsync();
                        return Parse(raw);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Success = false,
                    Message = "Connection error: " + ex.Message
                };
            }
        }

        private static ApiResult Parse(string raw)
        {
            raw = ExtractJson(raw);

            ApiResult result = new ApiResult();

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> root = serializer.Deserialize<Dictionary<string, object>>(raw);

                if (root == null)
                {
                    result.Success = false;
                    result.Message = "Invalid API response.";
                    return result;
                }

                if (root.ContainsKey("success"))
                {
                    result.Success = Convert.ToBoolean(root["success"]);
                }

                if (root.ContainsKey("message") && root["message"] != null)
                {
                    result.Message = Convert.ToString(root["message"]);
                }

                if (root.ContainsKey("data") && root["data"] is ArrayList)
                {
                    result.Data = ToTable((ArrayList)root["data"]);
                }

                return result;
            }
            catch
            {
                return new ApiResult
                {
                    Success = false,
                    Message = "Invalid API JSON: " + raw
                };
            }
        }

        private static string ExtractJson(string raw)
        {
            if (raw == null)
            {
                return "";
            }

            int start = raw.IndexOf("{\"success\"", StringComparison.OrdinalIgnoreCase);

            if (start < 0)
            {
                start = raw.IndexOf("{");
            }

            int end = raw.LastIndexOf("}");

            if (start >= 0 && end > start)
            {
                return raw.Substring(start, end - start + 1);
            }

            return raw;
        }

        private static DataTable ToTable(ArrayList list)
        {
            DataTable table = new DataTable();

            foreach (object item in list)
            {
                Dictionary<string, object> rowDictionary = item as Dictionary<string, object>;

                if (rowDictionary == null)
                {
                    continue;
                }

                foreach (KeyValuePair<string, object> pair in rowDictionary)
                {
                    if (!table.Columns.Contains(pair.Key))
                    {
                        table.Columns.Add(pair.Key);
                    }
                }
            }

            foreach (object item in list)
            {
                Dictionary<string, object> rowDictionary = item as Dictionary<string, object>;

                if (rowDictionary == null)
                {
                    continue;
                }

                DataRow row = table.NewRow();

                foreach (KeyValuePair<string, object> pair in rowDictionary)
                {
                    if (table.Columns.Contains(pair.Key))
                    {
                        row[pair.Key] = pair.Value == null ? "" : Convert.ToString(pair.Value);
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
