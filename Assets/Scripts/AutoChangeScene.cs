using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoChangeScene : MonoBehaviour
{
    public string sceneName; // Tên scene bạn muốn chuyển tới
    public float delay = 50f; // Thời gian chờ

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        MusicManager.Instance.PlayMusic("Game");
    }
}
