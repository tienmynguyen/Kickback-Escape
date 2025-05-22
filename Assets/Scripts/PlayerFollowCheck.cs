using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFollowCheck : MonoBehaviour
{
    public float distanceBehindCamera = 20f; // khoảng cách cho phép tụt lại
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (transform.position.x < mainCamera.position.x - distanceBehindCamera)
        {
            // Player bị tụt lại -> game over / restart
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
