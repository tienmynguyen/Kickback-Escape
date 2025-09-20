using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private bool isSkillRoutineRunning = false;
    private bool isLowHpSkillTriggered = false;

    [Header("Circle Idle Zone")]
    [SerializeField] private LineRenderer lineRenderer;
    public int segments = 100;          // độ mịn của vòng tròn
    public float idleRadius = 20f;       // bán kính vòng tròn

    [SerializeField] private Image bossHPImage;
    public int bossHP = 100;
    public int currentHP = 50;
    public Transform player;
    public bool isActivated = false;
    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public bool isFlipped = false;
    private Vector3 startPosition;
    [SerializeField] GameObject bloodprefabs;
    public Vector2 attackSize1 = new Vector2(2f, 1f);
    public Vector2 attackSize2 = new Vector2(2f, 1f);
    public Vector2 SkillSize = new Vector2(2f, 1f);
    [SerializeField] GameObject autoWall2;

    // ===== Skill mới =====
    [Header("Bullet Skill")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;

    [Header("Laser Skill")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserDuration = 0.5f;
    [SerializeField] private float warningDuration = 0.5f;

    void Start()
    {
        currentHP = bossHP;
        startPosition = transform.position;

        SetupLineRenderer();
        DrawCircle();
    }

    void SetupLineRenderer()
    {
        if (lineRenderer == null) return;

        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.widthMultiplier = 0.1f; // độ dày line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    void DrawCircle()
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = segments + 1;
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * idleRadius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * idleRadius;
            lineRenderer.SetPosition(i, new Vector3(transform.position.x + x, transform.position.y + y, 0));
            angle += 360f / segments;
        }
    }

    IEnumerator SkillRoutine()
    {
        isSkillRoutineRunning = true;

        while (isActivated && currentHP > 0)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));

            // Random skill
            List<System.Action> skills = new List<System.Action>();
            skills.Add(ShootSkill);
            skills.Add(() => StartCoroutine(LaserSkill()));

            if ((float)currentHP / bossHP <= 0.5f)
            {
                skills.Add(() => StartCoroutine(CircleShootSkill()));
                skills.Add(() => StartCoroutine(CircleLaserSkill()));
            }

            int randomIndex = Random.Range(0, skills.Count);
            skills[randomIndex].Invoke();
        }

        isSkillRoutineRunning = false;
    }

    void Update()
    {
        bossHPImage.fillAmount = (float)currentHP / bossHP;

        if (isActivated && !isSkillRoutineRunning)
        {
            StartCoroutine(SkillRoutine());
        }

        // Skill đặc biệt khi dưới 10% HP, chỉ gọi 1 lần
        if ((float)currentHP / bossHP <= 0.1f && !isLowHpSkillTriggered)
        {
            isLowHpSkillTriggered = true;
            StartCoroutine(CircleShootSkill());
            StartCoroutine(CircleLaserSkill());
        }

        if (isActivated)
        {
            LookAtPlayer();
        }

        // luôn update vòng tròn quanh boss
        DrawCircle();
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 pos = transform.TransformPoint(attackOffset);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos, SkillSize);
    }

    // ========== Đòn đánh cũ ==========
    public void Attack1()
    {
        Vector3 pos = transform.TransformPoint(new Vector3(3.5f, 0.5f, 0f));
        Collider2D colInfo = Physics2D.OverlapBox(pos, attackSize1, transform.eulerAngles.z, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage();
        }
    }

    public void Attack2()
    {
        Vector3 pos = transform.position;
        pos += transform.right * 3.5f;
        pos += transform.up * 1f;

        Collider2D colInfo = Physics2D.OverlapBox(pos, attackSize2, 0f, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage();
        }
    }

    public void Skill()
    {
        Vector3 pos = transform.position;
        pos += transform.right * 5f;
        pos += transform.up * 1f;

        Collider2D colInfo = Physics2D.OverlapBox(pos, SkillSize, 0f, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage();
        }
    }

    // ========== Skill mới ==========
    // 1. Bắn đạn thẳng
    public void ShootSkill()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
        Destroy(bullet, 5f);
    }

    // 2. Laser
    public IEnumerator LaserSkill()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float distance = Vector2.Distance(transform.position, player.position);

        // Spawn cảnh báo
        GameObject warning = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        Vector3 warningScale = warning.transform.localScale;
        warningScale.x = distance;
        warning.transform.localScale = warningScale;
        warning.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(warning, warningDuration);

        yield return new WaitForSeconds(warningDuration);

        // Spawn laser thật
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        Vector3 laserScale = laser.transform.localScale;
        laserScale.x = distance;
        laser.transform.localScale = laserScale;

        Destroy(laser, laserDuration);

        // Damage check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, attackMask);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<PlayerController>().TakeDamage();
        }
    }

    // 3. Bắn đạn vòng tròn
    public IEnumerator CircleShootSkill()
    {
        yield return new WaitForSeconds(2f);

        int bulletCount = 12;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;
            Destroy(bullet, 5f);
        }
    }

    public IEnumerator CircleLaserSkill()
    {
        int laserCount = 8;
        float angleStep = 360f / laserCount;

        yield return new WaitForSeconds(warningDuration);

        for (int i = 0; i < laserCount; i++)
        {
            float angle = i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, angle));

            Vector3 scale = laser.transform.localScale;
            scale.x = 20f;
            laser.transform.localScale = scale;

            Destroy(laser, laserDuration);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 10f, attackMask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }

    // ========== Logic cơ bản ==========
    public void LookAtPlayer()
    {
        Vector3 scale = transform.localScale;

        if (transform.position.x < player.position.x && isFlipped)
        {
            scale.x *= -1;
            transform.localScale = scale;
            isFlipped = false;
        }
        else if (transform.position.x > player.position.x && !isFlipped)
        {
            scale.x *= -1;
            transform.localScale = scale;
            isFlipped = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        currentHP--;
        bossHPImage.fillAmount = (float)currentHP / (float)bossHP;
        GameObject blood = Instantiate(bloodprefabs, transform.position, Quaternion.identity);
        Destroy(blood, 1f);

        if (currentHP <= 0)
        {
            autoWall2.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void ResetBoss()
    {
        transform.position = startPosition;
        currentHP = bossHP;
    }
}
