using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image healthSlider; // ⚡ Dùng Image chứ không phải Slider
    private Animator animator;
    public int maxHp = 5;
    public int playerHp = 5;
    [SerializeField] public float recoilForce = 35f;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] private int maxAirShots = 2;
    [SerializeField] private Image currentBulletImage;
    [SerializeField] GameObject bloodprefabs;
    private int remainingAirShots;
    private bool isDead = false;
    private bool isGrounded = false;
    private Vector3 startPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        playerHp = maxHp;

        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        remainingAirShots = maxAirShots;
        currentBulletImage.fillAmount = (float)remainingAirShots / (float)maxAirShots;

        // ✅ Chỉ hiển thị máu ở Level 6
        if (SceneManager.GetActiveScene().name == "Level 6" && healthSlider != null)
        {
            healthSlider.fillAmount = 1f; // đầy máu
        }
        else if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false); // Ẩn máu ở màn khác
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (isGrounded || remainingAirShots > 0))
        {
            Shoot();

            if (!isGrounded)
            {
                if (remainingAirShots < 99) remainingAirShots--;
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
        animator.SetTrigger("Shoot");
        SoundManager.Instance.PlaySound2D("bang");

        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * 1000f);

        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") && !isDead)
        {
            isDead = true;
            SoundManager.Instance.PlaySound2D("takedmg");
            SaveManager.Instance.AddDeath();
            GameObject blood = Instantiate(bloodprefabs, transform.position, Quaternion.identity);
            Destroy(blood, 1f);

            if (SceneManager.GetActiveScene().name == "Level 8")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            transform.position = startPosition;

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        if (other.CompareTag("Apple"))
        {
            SoundManager.Instance.PlaySound2D("collect");
            maxAirShots = -1;
            Destroy(other.gameObject);
            currentBulletImage.color = Color.yellow;
        }

        Invoke(nameof(ResetDeathState), 0.5f);

        if (other.CompareTag("Boss"))
        {
            TakeDamage();
        }
    }

    void ResetDeathState()
    {
        isDead = false;
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
        if (grounded)
        {
            remainingAirShots = maxAirShots;
        }
    }

    public void TakeDamage()
    {
        playerHp--;

        // ✅ Cập nhật thanh máu (chỉ ở Level 6)
        if (SceneManager.GetActiveScene().name == "Level 6" && healthSlider != null)
        {
            healthSlider.fillAmount = (float)playerHp / (float)maxHp;
        }

        GameObject blood = Instantiate(bloodprefabs, transform.position, Quaternion.identity);
        Destroy(blood, 1f);

        if (playerHp <= 0)
        {
            GameObject bosses = GameObject.FindGameObjectWithTag("Boss");
            Boss boss = bosses.GetComponent<Boss>();
            if (boss != null)
            {
                boss.ResetBoss();
            }

            SaveManager.Instance.AddDeath();

            transform.position = startPosition;

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            playerHp = maxHp;

            // ✅ Reset máu đầy
            if (SceneManager.GetActiveScene().name == "Level 6" && healthSlider != null)
            {
                healthSlider.fillAmount = 1f;
            }
        }
    }
}
