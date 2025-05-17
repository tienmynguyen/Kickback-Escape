using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetPosition; // vị trí dịch chuyển đến

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // đảm bảo player có tag "Player"
        {
            SoundManager.Instance.PlaySound2D("teleport");
            other.transform.position = targetPosition.position;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}
