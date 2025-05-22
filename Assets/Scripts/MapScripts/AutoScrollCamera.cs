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

        // Nếu người chơi vượt quá giới hạn
        if (player.position.x > currentX + maxAheadDistance)
        {
            float distance = player.position.x - (currentX + maxAheadDistance);
            targetX += distance * catchUpMultiplier * Time.deltaTime;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
