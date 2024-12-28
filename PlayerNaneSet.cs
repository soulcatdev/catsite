using UnityEngine;
using UnityEngine.UI; // 如果使用 Text
using TMPro; // 如果使用 TextMeshPro
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    public TMP_Text inputText; // 输入的文本（TextMeshPro）
    public TMP_Text feedbackText; // 用于显示反馈的文本
    public Button myButton; // 按钮对象

    private void Start()
    {
        // 绑定按钮点击事件
        myButton.onClick.AddListener(OnButtonClick);
        feedbackText.text = ""; // 初始化为隐藏
    }

    private void OnButtonClick()
    {
        string text = inputText.text;

        // 检查字数是否大于15
        if (text.Length > 15)
        {
            feedbackText.text = "文本字数超过15！";
            Invoke(nameof(ClearFeedbackText), 5f); // 5秒后清除提示
        }
        else
        {
            // 检查是否包含特殊字符
            if (text.Contains("\"") || text.Contains("<") || text.Contains(">"))
            {
                feedbackText.text = "文本包含特殊字符！";
                Invoke(nameof(ClearFeedbackText), 5f); // 显示 5 秒后清除
            }
            else
            {
                // 跳转到场景 2
                SceneManager.LoadScene(2);
            }
        }
    }

    private void ClearFeedbackText()
    {
        feedbackText.text = ""; // 清除反馈文本
    }
}
