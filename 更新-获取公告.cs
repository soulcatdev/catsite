using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class UpdateTextFromWebsite : MonoBehaviour
{
    public Text textComponent; // 引用 UI 文本组件

    private async void Start()
    {
        await FetchAndUpdateText();
    }

    private async Task FetchAndUpdateText()
    {
        string url = "https://www.examples.com";//灵魂猫咪官网
        string webContent = await GetWebsiteContent(url);

        if (!string.IsNullOrEmpty(webContent))
        {
            string extractedText = ExtractTextBetweenCrowns(webContent);

            if (!string.IsNullOrEmpty(extractedText))
            {
                textComponent.text = extractedText; // 更新 UI 文本内容（覆盖公告）
                Debug.Log($"提取的文本: {extractedText}");
            }
            else
            {
                Debug.LogError("未找到 ♛ 与 ♛ 之间的文本");
            }
        }
    }

    private async Task<string> GetWebsiteContent(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"请求失败: {e.Message}");
                return null;
            }
        }
    }

    private string ExtractTextBetweenCrowns(string content)
    {
        // 使用正则表达式匹配 ♛ 与 ♛ 之间的所有文本
        Match match = Regex.Match(content, "♛(.*?)♛", RegexOptions.Singleline);//更新公告

        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }
        else
        {
            return null;
        }
    }
}
