using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ProjectileState<T> : AIState where T : Bullet
{
    public enum PatternType
    {
        Random,
        Circle,
        Grid,
    }

    [SerializeField]
    private SpriteRenderer _fieldSr;
    private Vector2 _spawnArea = Vector2.zero;
    private Vector2 _spawnPos = Vector2.zero;


    [SerializeField]
    private bool _isRandomPattern = false;
    [SerializeField]
    private PatternType _patternType;

    protected float timer = 0f;

    [Header("Random Spawn Parameter")]
    [SerializeField]
    protected float _randomSpawnDelay = 3;

    [Header("Circle Spawn Parameter")]
    [SerializeField]
    private bool _isLeftSpawnDireaction = true;
    [SerializeField]
    private float _circleSpawnDelay = 0.2f;

    private void Start()
    {
        _spawnArea = _fieldSr.size / 2 + Vector2.one;
    }

    public override void OnStateEnter()
    {
        if (_isRandomPattern)
        {
            _patternType = (PatternType)Random.Range(0, (int)PatternType.Grid + 1);
        }

        _spawnPos = Vector2.zero;
        switch (_patternType)
        {
            case PatternType.Random:
                timer = _randomSpawnDelay;
                break;
            case PatternType.Circle:
                break;
            case PatternType.Grid:
                break;
        }
    }

    public override void OnStateLeave()
    {

    }

    public override void TakeAAction()
    {
        timer += Time.deltaTime;

        switch (_patternType)
        {
            case PatternType.Random:
                RandomSpawn();
                break;
            case PatternType.Circle:
                CircleSpawn();
                break;
            case PatternType.Grid:
                break;
        }
    }

    protected void SpawnProjectile(Vector2 spawnPos, Vector2 direction)
    {
        T bullet = GameManager.Instance.ResourceManager_.Instantiate(typeof(T).Name).GetComponent<T>();
        _spawnPos = spawnPos;
        bullet.transform.position = _spawnPos;

        bullet.Init(direction); // ���߿� Ǯ���̾� ��ġ�� �ٲٱ�
        BulletManager.Instance.AddBullet(bullet);
        timer = 0f;
    }

    public virtual void RandomSpawn()
    {
        if (timer >= _randomSpawnDelay)
        {
            SpawnProjectile(GetRandomSpawnPos(), Vector2.zero);
        }
    }

    public virtual void CircleSpawn() // �ʵ� �ױ��� ���� ��ȯ
    {
        if(timer >= 0.2f)
        {
            if(_spawnPos == Vector2.zero) // ����
            {
                _spawnPos = new Vector2(0, _spawnArea.y);
            }

            SpawnProjectile(_spawnPos, Vector2.zero);

            // _spawnPos = new Vector2(_spawnArea.x, _spawnArea.y)
            if((_spawnPos.y == _spawnArea.y && Mathf.Abs(_spawnPos.x) < _spawnArea.x)
                || _spawnPos == new Vector2(_spawnArea.x, _spawnArea.y)) // ���� || ������ ��
            {
                _spawnPos.x += _isLeftSpawnDireaction ? -1f : 1f;
            }
            else if((_spawnPos.y == -_spawnArea.y && Mathf.Abs(_spawnPos.x) < _spawnArea.x)
                || _spawnPos == new Vector2(-_spawnArea.x, -_spawnArea.y)) // �Ʒ��� || ���� �Ʒ�
            {
                _spawnPos.x += _isLeftSpawnDireaction ? 1f : -1f;
            }
            else if((_spawnPos.x == _spawnArea.x && Mathf.Abs(_spawnPos.y) < _spawnArea.y)
                || _spawnPos == new Vector2(_spawnArea.x, -_spawnArea.y)) // ������ || ������ �Ʒ�
            {
                _spawnPos.y += _isLeftSpawnDireaction ? 1f : -1f;
            }
            else if((_spawnPos.x == -_spawnArea.x && Mathf.Abs(_spawnPos.y) < _spawnArea.y)
                || _spawnPos == new Vector2(-_spawnArea.x, _spawnArea.y)) // ���� || ���� ��
            {
                _spawnPos.y += _isLeftSpawnDireaction ? -1f : 1f;
            }
        }
    }

    public virtual void GridSpawn() // ���ڷ� ����
    {

    }

    public Vector2 GetRandomSpawnPos()
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
