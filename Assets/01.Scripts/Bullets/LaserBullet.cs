using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public Action DestroyAction = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 데미지 처리하고
            // 이펙트 정도?
            Agent agent = collision.GetComponentInParent<Agent>();
            if (agent != null)
            {
                agent.Hit();
            }

            DestroyAction?.Invoke(); // 여기서 부모를 삭제하기
            GameManager.Instance.ResourceManager_.Destroy(this.gameObject);
        }
    }

    // 또는 0.1초 정도 후에 삭제
}
