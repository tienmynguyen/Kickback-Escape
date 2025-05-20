using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneCheck2 : MonoBehaviour
{
    public Boss boss; // Kéo Boss từ Hierarchy vào đây
    [SerializeField] GameObject autoWall1;
    [SerializeField] BossZoneCheck zoneCheck;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            zoneCheck.triggered = false;
            boss.isActivated = false; // Boss bắt đầu di chuyển
            autoWall1.SetActive(false);
        }
    }
}
