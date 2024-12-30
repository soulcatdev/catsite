using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class CompareWebsiteNumber : MonoBehaviour
{
    public TMP_Text textComponent; // 引用 TextMeshPro 文本组件
    public GameObject plane;       // 引用 Plane 对象

    private async void Start()
    {
        plane.SetActive(false); // 确保 Plane 在开始时隐藏
        await FetchAndCompareNumber();
    }

    private async Task FetchAndCompareNumber()
    {
        string url = "https://www.examples.com";  // 网站 URL
        string webContent = await GetWebsiteContent(url); // 获取网站内容

        if (!string.IsNullOrEmpty(webContent)) // 确保网站内容有效
        {
            // 提取网站中的数字（获取最新版本号，不支持小数！）
            int websiteNumber = ExtractNumberBetweenDice(webContent); 

            if (int.TryParse(textComponent.text, out int textNumber)) // 获取 TextMeshPro 中的数字
            {
                Debug.Log($"网站中的数字: {websiteNumber}, 文本中的数字: {textNumber}");

                // 比较两个数字（版本校对）
                if (websiteNumber > textNumber)
                {
                    plane.SetActive(true); // 如果网站中的数字大于文本数字，则显示 Plane
                }
            }
            else
            {
                Debug.LogError("文本组件中没有有效的数字！");
            }
        }
        else
        {
            Debug.LogError("无法获取网站内容！");
        }
    }

    // 获取网站源码
    private async Task<string> GetWebsiteContent(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            try
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text; // 返回网站源码
                }
                else
                {
                    Debug.LogError($"请求失败: {request.error}");
                    return null;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"请求异常: {e.Message}");
                return null;
            }
        }
    }

    // 提取 ⚅ 和 ⚅ 之间的数字
    private int ExtractNumberBetweenDice(string content)
    {
        // 使用正则表达式匹配 ⚅ 与 ⚅ 之间的数字
        Match match = Regex.Match(content, @"⚅\s*(\d+)\s*⚅");

        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
            return number; // 返回匹配到的数字
        }
        else
        {
            Debug.LogError("未找到 ⚅ 和 ⚅ 之间的数字");
            return 0; // 返回 0 代表没有匹配到数字
        }
    }
}
