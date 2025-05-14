
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] public float recoilForce = 35f;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] private int maxAirShots = 2;
    [SerializeField] private Image currentBulletImage;
    [SerializeField] float check = 0f;
    private int remainingAirShots;

    private bool isGrounded = false;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        remainingAirShots = maxAirShots;
        currentBulletImage.fillAmount = (float)remainingAirShots / (float)maxAirShots;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && (isGrounded || remainingAirShots > 0))
        {
            Shoot();

            if (!isGrounded)
            {
                remainingAirShots--;
                currentBulletImage.fillAmount = (float)remainingAirShots / (float)maxAirShots;
            }

        }

        if (isGrounded)
        {
            remainingAirShots = maxAirShots;
            currentBulletImage.fillAmount = (float)remainingAirShots / (float)maxAirShots;
        }
    }
    void Shoot()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * 500f);

        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
        Debug.Log(-direction * recoilForce);

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            // Reset vị trí
            transform.position = startPosition;

            // Reset vận tốc (tránh bị trôi tiếp sau khi hồi sinh)
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // (Tùy chọn) Thêm hiệu ứng, âm thanh, animation tại đây
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;

        if (grounded)
        {
            remainingAirShots = maxAirShots;
        }
    }

}
