using UnityEngine;
using UnityEngine.SceneManagement;
public class DragonFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offsetFromCamera = new Vector3(-2f, 0, 0); // Rồng cách camera 2 đơn vị phía sau
    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    void Update()
    {
        // Luôn cập nhật vị trí rồng theo camera
        transform.position = cameraTransform.position + offsetFromCamera;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player bị bắt bởi rồng!");
            // Tải lại màn chơi hoặc game over
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
