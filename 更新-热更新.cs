using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.IO;

public class HotUpdateManager : MonoBehaviour
{
    [Header("UI按钮")]
    public Button button1; // 按钮1！
    public Button button2; // 按钮2！
    public Button hideButton1; // 需要隐藏的按钮1(按钮1！)
    public Button hideButton2; // 需要隐藏的按钮2（按钮2！）//按下按钮1！后就被隐藏防止反复更新

    private string downloadUrl = ""; // 热更新包下载链接

    void Start()
    {
        // 绑定按钮点击事件
        button1.onClick.AddListener(OnButton1Click);
        button2.onClick.AddListener(OnButton2Click);
    }

    /// <summary>
    /// 按钮1点击事件：开始热更新
    /// </summary>
    private void OnButton1Click()
    {
        // 隐藏指定按钮
        hideButton1.gameObject.SetActive(false);
        hideButton2.gameObject.SetActive(false);

        // 开始获取网页内容
        StartCoroutine(FetchWebPage());
    }

    /// <summary>
    /// 获取网页源码并提取下载链接
    /// </summary>
    private IEnumerator FetchWebPage()
    {
        using (HttpClient client = new HttpClient())
        {
            Debug.Log("正在获取网页内容...");

            try
            {
                string content = await client.GetStringAsync("https://www.example.com/");
                Debug.Log("网页内容获取成功");

                // 提取⚃⚃之间的内容（下载链接）
                Match match = Regex.Match(content, "⚃(.*?)⚃");
                if (match.Success)
                {
                    downloadUrl = match.Groups[1].Value;
                    Debug.Log($"解析到下载链接: {downloadUrl}");

                    // 开始下载AB包
                    StartCoroutine(DownloadAssetBundle(downloadUrl));
                }
                else
                {
                    Debug.LogError("未找到有效的下载链接，请检查网页内容。");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"网页获取失败: {ex.Message}");
            }
        }

        yield return null;
    }

    /// <summary>
    /// 下载AB包
    /// </summary>
    private IEnumerator DownloadAssetBundle(string url)
    {
        Debug.Log("开始下载AB包...");

        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                Debug.Log($"下载进度: {request.downloadProgress * 100}%");
                yield return null;
            }

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("AB包下载成功");

                AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
                if (bundle != null)
                {
                    Debug.Log("AB包加载成功");
                    bundle.Unload(false); // 卸载资源包（实际项目中可以根据需求加载资源）
                }

                // 下载完成，退出游戏
                Application.Quit();
            }
            else
            {
                Debug.LogError($"下载失败: {request.error}");
            }
        }
    }

    /// <summary>
    /// 按钮2点击事件：直接退出游戏
    /// </summary>
    private void OnButton2Click()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
