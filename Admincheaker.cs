using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Admincheakerh : MonoBehaviour
{
    public TMPro.TMP_InputField inputText;
    public GameObject ourPlayer; // 本地玩家
    private bool isAdmin = false; // IP匹配状态

    void Start()
    {
        // 每5分钟自动验证一次IP
        StartCoroutine(VerifyIPPeriodically());
    }

    IEnumerator VerifyIPPeriodically()
    {
        while (true)
        {
            yield return VerifyIP();      // 验证一次
            yield return new WaitForSeconds(300f); // 5分钟 = 300秒
        }
    }

    IEnumerator VerifyIP()
    {
        // 1. 获取玩家公网IP
        UnityWebRequest ipRequest = UnityWebRequest.Get("https://api.ipify.org");
        yield return ipRequest.SendWebRequest();
        if (ipRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("获取公网IP失败: " + ipRequest.error);
            isAdmin = false;
            yield break;
        }
        string myIP = ipRequest.downloadHandler.text.Trim();

        // 2. 获取网站配置IP
        UnityWebRequest webRequest = UnityWebRequest.Get("http://www.lol.com");
        yield return webRequest.SendWebRequest();
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("访问网站失败: " + webRequest.error);
            isAdmin = false;
            yield break;
        }

        string siteContent = webRequest.downloadHandler.text;
        int start = siteContent.IndexOf("$");
        int end = siteContent.LastIndexOf("$");
        if (start == -1 || end == -1 || end <= start)
        {
            Debug.LogWarning("网站内容没有找到 $...$");
            isAdmin = false;
            yield break;
        }

        string allowedIPs = siteContent.Substring(start + 1, end - start - 1).Trim();
        string[] ipList = allowedIPs.Split(',');

        // 3. 检查IP是否匹配
        isAdmin = false;
        foreach (string ip in ipList)
        {
            if (ip.Trim() == myIP)
            {
                isAdmin = true;
                break;
            }
        }
        Debug.Log(isAdmin ? "IP对不上" : "IP不配😭");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string cmd = inputText.text.Trim();
            if (!isAdmin)
            {
                Debug.Log("没有权限执行管理员命令");
                inputText.text = string.Empty;
                return;
            }

            // 执行管理员命令
            if (cmd.Contains("///playerlist"))
            {
                string text = "";
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var player in players)
                {
                    var setup = player.GetComponent<PlayerSetup>();
                    if (setup != null)
                        text += setup.nickname + " ";
                }
                inputText.text = text;
            }
            
------------其他指令
            inputText.text = string.Empty; // 清空输入框
        }
    }
}
