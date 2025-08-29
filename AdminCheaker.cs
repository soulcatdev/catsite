using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AdminManager : MonoBehaviour
{
    public TMPro.TMP_InputField inputText;
    public GameObject ourPlayer; // 本地玩家self
    private bool isAdmin = false; // IP匹配状态

    void Start()
    {
        // 5min
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
        // 公网IP（蜂窝网）
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
            Debug.LogError("网站有问题？" + webRequest.error);
            isAdmin = false;
            yield break;
        }

        string siteContent = webRequest.downloadHandler.text;
        int start = siteContent.IndexOf("$");
        int end = siteContent.LastIndexOf("$");
        if (start == -1 || end == -1 || end <= start)
        {
            Debug.LogWarning("没有找到标志$$");
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
        Debug.Log(isAdmin ? "获取管理" : "普通玩家");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string cmd = inputText.text.Trim();
            if (!isAdmin)
            {
                Debug.Log("你没有权限执行管理员命令");
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
            else if (cmd.Contains("///tppl"))
            {
                string targetName = cmd.Remove(0, 8); // 去掉命令前缀
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var player in players)
                {
                    var setup = player.GetComponent<PlayerSetup>();
                    if (setup != null && setup.nickname == targetName)
                    {
                        ourPlayer.transform.position = player.transform.position;
                        break;
                    }
                }
            }

            inputText.text = string.Empty; // 清空输入框
        }
    
