using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class WebTextFetcher : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;  // 指定TextMeshPro组件
    private const string url = "https://github.com/soulcatdev/catsite/";

    // Start is called before the first frame update
    async void Start()
    {
        string fetchedText = await GetTextFromWebsite(url);
        string extractedText = ExtractTextBetweenSymbols(fetchedText, "✦✦");
        textMeshPro.text = extractedText;  // 更新TextMeshPro的文本
    }

    // 获取网页的源码
    private async Task<string> GetTextFromWebsite(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string pageContent = await response.Content.ReadAsStringAsync();
            return pageContent;
        }
    }

    // 提取✦✦之间的文本（更新公告）
    private string ExtractTextBetweenSymbols(string content, string symbol)
    {
        int startIndex = content.IndexOf(symbol);
        int endIndex = content.IndexOf(symbol, startIndex + symbol.Length);

        if (startIndex == -1 || endIndex == -1)
        {
            return "No text found between ✦✦.";
        }

        return content.Substring(startIndex + symbol.Length, endIndex - startIndex - symbol.Length);
    }
}
