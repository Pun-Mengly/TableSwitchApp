using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TableSwitchWebApplication.Data.ChatGPT
{
    public class ChatGPT
    {
        public async Task<string> SendRequestToChatGPTApi(string query)
        {
            string apiKey = "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";
            string endpoint = "https://api.openai.com/v1/engines/davinci-codex/completions";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestData = new { prompt = query, max_tokens = 60, n = 1, stop = "\n" };
            string requestDataJson = JsonConvert.SerializeObject(requestData);

            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}

