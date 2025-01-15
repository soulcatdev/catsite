using UnityEngine;
using TMPro;

public class SpawnPrefabByName : MonoBehaviour
{
    public TMP_InputField inputField; // 引用TextMeshPro的输入框
    public Transform player;          // 玩家Transform，用于确定生成位置
    public GameObject[] prefabs;      // 可生成的Prefab列表

    void Update()
    {
        // 按下 T 激活输入模式
        if (Input.GetKeyDown(KeyCode.T))
        {
            ActivateInputMode();
        }
    }

    void Start()
    {
        // 设置回车键监听
        inputField.onSubmit.AddListener(HandleInput);
    }

    void ActivateInputMode()
    {
        // 显示光标并激活输入框
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputField.ActivateInputField(); // 激活输入框
        inputField.Select();             // 让输入框获得焦点
    }

    void HandleInput(string inputText)
    {
        if (string.IsNullOrWhiteSpace(inputText)) return;

        // 检测输入是否符合 ///sp + prefabName 的格式
        if (inputText.StartsWith("///sp ", System.StringComparison.OrdinalIgnoreCase))
        {
            string prefabName = inputText.Substring(6).Trim(); // 提取Prefab名称

            // 在Prefab数组中查找匹配的Prefab（忽略大小写）
            GameObject prefabToSpawn = System.Array.Find(prefabs, prefab => prefab.name.Equals(prefabName, System.StringComparison.OrdinalIgnoreCase));

            if (prefabToSpawn != null)
            {
                // 在玩家位置左侧（X-1）生成Prefab
                Vector3 spawnPosition = player.position + Vector3.left;
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                // 更新输入框文字
                inputField.text = $"spawn <color=green>{prefabName}</color> <color=red>complete!</color>";
            }
            else
            {
                inputField.text = $"Prefab <color=red>{prefabName}</color> not found!";
            }
        }
        else
        {
            inputField.text = "<color=red>Invalid command!</color>";
        }

        // 保持输入框激活状态
        inputField.ActivateInputField();
        inputField.Select();
    }
}
