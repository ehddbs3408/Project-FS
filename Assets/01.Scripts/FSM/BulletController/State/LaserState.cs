using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserState : BulletState
{
    public override void TakeAAction()
    {
        timer += Time.deltaTime;

        if (timer >= _createDelay)
        {
            Laser bullet = GameManager.Instance.ResourceManager_.Instantiate("Laser").GetComponent<Laser>();
            bullet.transform.position = GetSpawnPos();

            bullet.Init(Vector2.zero); // ���߿� Ǯ���̾� ��ġ�� �ٲٱ�
            BulletManager.Instance.AddBullet(bullet);
            timer = 0f;
        }
    }
}
