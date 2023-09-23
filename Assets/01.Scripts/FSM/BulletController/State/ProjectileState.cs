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

        bullet.Init(direction); // 나중에 풀레이어 위치로 바꾸기
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

    public virtual void CircleSpawn() // 필드 테구리 따라 소환
    {
        if(timer >= 0.2f)
        {
            if(_spawnPos == Vector2.zero) // 시작
            {
                _spawnPos = new Vector2(0, _spawnArea.y);
            }

            SpawnProjectile(_spawnPos, Vector2.zero);

            // _spawnPos = new Vector2(_spawnArea.x, _spawnArea.y)
            if((_spawnPos.y == _spawnArea.y && Mathf.Abs(_spawnPos.x) < _spawnArea.x)
                || _spawnPos == new Vector2(_spawnArea.x, _spawnArea.y)) // 위쪽 || 오른쪽 위
            {
                _spawnPos.x += _isLeftSpawnDireaction ? -1f : 1f;
            }
            else if((_spawnPos.y == -_spawnArea.y && Mathf.Abs(_spawnPos.x) < _spawnArea.x)
                || _spawnPos == new Vector2(-_spawnArea.x, -_spawnArea.y)) // 아래쪽 || 왼쪽 아래
            {
                _spawnPos.x += _isLeftSpawnDireaction ? 1f : -1f;
            }
            else if((_spawnPos.x == _spawnArea.x && Mathf.Abs(_spawnPos.y) < _spawnArea.y)
                || _spawnPos == new Vector2(_spawnArea.x, -_spawnArea.y)) // 오른쪽 || 오른쪽 아래
            {
                _spawnPos.y += _isLeftSpawnDireaction ? 1f : -1f;
            }
            else if((_spawnPos.x == -_spawnArea.x && Mathf.Abs(_spawnPos.y) < _spawnArea.y)
                || _spawnPos == new Vector2(-_spawnArea.x, _spawnArea.y)) // 왼쪽 || 왼쪽 위
            {
                _spawnPos.y += _isLeftSpawnDireaction ? -1f : 1f;
            }
        }
    }

    public virtual void GridSpawn() // 격자로 생성
    {

    }

    public Vector2 GetRandomSpawnPos()
    {
        Vector2 pos = Vector2.zero;
        if (Random.value > 0.5f) // 위쪽&아래쪽
        {
            // x는 랜덤
            pos.x = Random.Range(-_spawnArea.x, _spawnArea.x);
            // y는 위 아니면 아래
            pos.y = Random.value > 0.5f ? -_spawnArea.y : _spawnArea.y;
        }
        else // 왼쪽&오른쪽
        {
            // x는 위 아니면 아래
            pos.x = Random.value > 0.5f ? -_spawnArea.x : _spawnArea.x;
            // y는 랜덤
            pos.y = Random.Range(-_spawnArea.y, _spawnArea.y);
        }

        return pos;
    }
}
