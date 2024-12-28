using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerToScene : MonoBehaviour
{
    public VideoClip videoClip; // 要播放的视频剪辑

    private VideoPlayer videoPlayer;

    void Start()
    {
        // 添加 VideoPlayer 组件
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        // 设置 VideoPlayer 属性
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane; // 视频渲染到相机
        videoPlayer.targetCamera = Camera.main; // 使用主相机播放视频
        videoPlayer.isLooping = false; // 不循环播放
        videoPlayer.playOnAwake = true; // 自动播放

        // 注册事件，当视频播放完成时调用方法
        videoPlayer.loopPointReached += OnVideoFinished;

        // 设置全屏播放
        videoPlayer.aspectRatio = VideoAspectRatio.Stretch;

        // 播放视频
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 视频播放完成后，跳转到编号为1的场景
        SceneManager.LoadScene(1);
    }
}
