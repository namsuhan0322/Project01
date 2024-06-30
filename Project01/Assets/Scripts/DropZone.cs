using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    private bool isPlayerInZone = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("플레이어가 상점 구역에 들어왔습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInZone = false;
            Debug.Log("플레이어가 상점 구역을 나갔습니다.");
        }
    }

    public bool IsPlayerInZone()
    {
        return isPlayerInZone;
    }
}