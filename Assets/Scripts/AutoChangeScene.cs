using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class AutoChangeScene : MonoBehaviour
{
    public string sceneName; // Tên scene bạn muốn chuyển tới
    public float delay = 50f; // Thời gian chờ
    public VideoPlayer videoPlayer;
    void Start()
    {

        StartCoroutine(LoadSceneAfterDelay());
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "intro.webm");
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        MusicManager.Instance.PlayMusic("Game");
    }
}
