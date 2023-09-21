using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowState : BulletState
{
    public override void TakeAAction()
    {
        timer += Time.deltaTime;

        if (timer >= _createDelay)
        {
            Arrow arrow = GameManager.Instance.ResourceManager_.Instantiate("Arrow").GetComponent<Arrow>();
            arrow.transform.position = GetSpawnPos();

            arrow.Init(Vector2.zero); // ���߿� Ǯ���̾� ��ġ�� �ٲٱ�
            BulletManager.Instance.AddBullet(arrow);
            timer = 0f;
        }
    }
}
