using UnityEngine;
using UnityEngine.SceneManagement;

public class DragonFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offsetFromCamera = new Vector3(1f, 0, 0); // khoảng cách theo trục X

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Giữ z của rồng = z hiện tại (không theo camera)
        transform.position = new Vector3(
            cameraTransform.position.x -15f,
            cameraTransform.position.y + offsetFromCamera.y,
            transform.position.z
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player bị bắt bởi rồng!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
