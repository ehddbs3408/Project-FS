using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState : AIState
{
    [SerializeField]
    protected float _createDelay = 3;

    [SerializeField]
    private SpriteRenderer _fieldSr;
    private Vector2 _spawnArea = Vector2.zero;

    protected float timer = 0f;

    private void Start()
    {
        _spawnArea = _fieldSr.size / 2 + Vector2.one;
    }

    public override void OnStateEnter()
    {
        timer = _createDelay;
    }

    public override void OnStateLeave()
    {

    }

    public override void TakeAAction()
    {
        timer += Time.deltaTime;

        if (timer >= _createDelay)
        {
            Bullet bullet = GameManager.Instance.ResourceManager_.Instantiate("Bullet").GetComponent<Bullet>();
            bullet.transform.position = GetSpawnPos();

            bullet.Init(Vector2.zero); // ���߿� Ǯ���̾� ��ġ�� �ٲٱ�
            BulletManager.Instance.AddBullet(bullet);
            timer = 0f;
        }
    }

    public Vector2 GetSpawnPos()
    {
        Vector2 pos = Vector2.zero;
        if (Random.value > 0.5f) // ����&�Ʒ���
        {
            // x�� ����
            pos.x = Random.Range(-_spawnArea.x, _spawnArea.x);
            // y�� �� �ƴϸ� �Ʒ�
            pos.y = Random.value > 0.5f ? -_spawnArea.y : _spawnArea.y;
        }
        else // ����&������
        {
            // x�� �� �ƴϸ� �Ʒ�
            pos.x = Random.value > 0.5f ? -_spawnArea.x : _spawnArea.x;
            // y�� ����
            pos.y = Random.Range(-_spawnArea.y, _spawnArea.y);
        }

        return pos;
    }
}
