using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
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
    void Start()
    {
        currentHP = bossHP;
        startPosition = transform.position;
    }
    void Update()
    {
        bossHPImage.fillAmount = (float)currentHP / (float)bossHP;

        if (isActivated)
        {
            LookAtPlayer();
          
        }
    }
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos, SkillSize);
    }
    public void Attack1()
    {
        Vector3 pos = transform.position;
        pos += transform.right * 3.5f;
        pos += transform.up * 0.5f;

        Collider2D colInfo = Physics2D.OverlapBox(pos, attackSize1, 0f, attackMask);
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
        // Reset vị trí

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