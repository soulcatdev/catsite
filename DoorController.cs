

using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim; // 门的动画组件
    public Transform emptyObject1; // 第一个空对象
    public Transform emptyObject2; // 第二个空对象
    public Transform player;       // 玩家
    public float interactionRadius = 3f; // 半径

    public AudioSource openDoorAudio; // 开门音效
    public AudioSource closeDoorAudio; // 关门音效

    void Start()
    {
        anim = GetComponent<Animator>(); // 获取Animator组件
    }

    void Update()
    {
        // 计算玩家与门的距离
        float distance = Vector3.Distance(player.position, transform.position);

        // 如果玩家在半径范围内且按下E键
        if (distance <= interactionRadius && Input.GetKeyDown(KeyCode.E))
        {
            // 判断两个空对象的坐标是否相同
            if (emptyObject1.position == emptyObject2.position)
            {
                anim.SetTrigger("Open"); // 播放开门动画
                if (openDoorAudio != null) // 检查是否设置了开门音效
                {
                    openDoorAudio.Play(); // 播放开门音效
                }
            }
            else
            {
                anim.SetTrigger("Closed"); // 播放关门动画
                if (closeDoorAudio != null) // 检查是否设置了关门音效
                {
                    closeDoorAudio.Play(); // 播放关门音效
                }
            }
        }
    }
}

