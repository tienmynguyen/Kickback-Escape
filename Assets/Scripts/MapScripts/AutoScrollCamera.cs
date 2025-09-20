using UnityEngine;

public class AutoScrollCamera : MonoBehaviour
{
    public Transform player;              // Tham chiếu tới người chơi
    public float scrollSpeed = 2f;        // Tốc độ cuộn mặc định
    public float maxAheadDistance = 3f;   // Khoảng cách tối đa player có thể vượt camera
    public float catchUpMultiplier = 2f;  // Hệ số đuổi theo (tăng tốc camera nếu bị tụt)

    void Update()
    {
        float currentX = transform.position.x;
        float targetX = currentX + scrollSpeed * Time.deltaTime;

        // Nếu người chơi vượt quá giới hạn X
        if (player.position.x > currentX + maxAheadDistance)
        {
            float distance = player.position.x - (currentX + maxAheadDistance);
            targetX += distance * catchUpMultiplier * Time.deltaTime;
        }

        // 🚀 Camera luôn follow player theo trục Y
       float targetY = Mathf.Lerp(transform.position.y, player.position.y, 5f * Time.deltaTime);

        // Gán lại vị trí camera
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
