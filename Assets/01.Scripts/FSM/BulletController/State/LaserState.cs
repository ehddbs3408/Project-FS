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
            Laser laser = GameManager.Instance.ResourceManager_.Instantiate("Laser").GetComponent<Laser>();
            laser.transform.position = GetSpawnPos();

            laser.Init(Vector2.zero); // ���߿� Ǯ���̾� ��ġ�� �ٲٱ�
            BulletManager.Instance.AddBullet(laser);
            timer = 0f;
        }
    }
}
