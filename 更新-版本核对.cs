using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class CompareWebsiteNumber : MonoBehaviour
{
    public Text textComponent; // 引用UI文本组件
    public GameObject plane;   // 引用要显示的Plane对象

    private async void Start()
    {
        plane.SetActive(false); // 确保Plane在开始时隐藏
        await FetchAndCompareNumber();
    }

    private async Task FetchAndCompareNumber()
    {
        string url = "https://www.examples.com";
        string webContent = await GetWebsiteContent(url);

        if (!string.IsNullOrEmpty(webContent))
        {
            int websiteNumber = ExtractNumberBetweenDice(webContent);
            int textNumber = int.Parse(textComponent.text);

            Debug.Log($"网站数字: {websiteNumber}, 文本数字: {textNumber}");

            if (websiteNumber > textNumber)
            {
                plane.SetActive(true); // 显示Plane面板
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

    private int ExtractNumberBetweenDice(string content)
    {
        // 使用正则表达式匹配⚅与⚅之间的数字
        Match match = Regex.Match(content, "⚅(\\d+)⚅");

        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
            return number;
        }
        else
        {
            Debug.LogError("未找到⚅与⚅之间的数字");
            return 0;
        }
    }
}
