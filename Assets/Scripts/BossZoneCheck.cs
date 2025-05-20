using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneCheck : MonoBehaviour
{
    public Boss boss; // Kéo Boss từ Hierarchy vào đây
    public bool triggered = false;
    [SerializeField] GameObject autoWall1;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            boss.isActivated = true; // Boss bắt đầu di chuyển
            autoWall1.SetActive(true);
        }
    }
}
