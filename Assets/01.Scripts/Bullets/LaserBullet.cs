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
            // ������ ó���ϰ�
            // ����Ʈ ����?
            Agent agent = collision.GetComponentInParent<Agent>();
            if (agent != null)
            {
                agent.Hit();
            }

            DestroyAction?.Invoke(); // ���⼭ �θ� �����ϱ�
            GameManager.Instance.ResourceManager_.Destroy(this.gameObject);
        }
    }

    // �Ǵ� 0.1�� ���� �Ŀ� ����
}
