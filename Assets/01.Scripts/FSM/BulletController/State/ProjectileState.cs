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
        Rain,
    }

    [SerializeField]
    private SpriteRenderer _fieldSr;
    private Vector2 _spawnArea = Vector2.zero;
    private Vector2 _spawnPos = Vector2.zero;


    [SerializeField]
    private bool _isRandomPattern = false;
    [SerializeField]
    private PatternType _patternType;

    protected float _timer = 0f;

    [Header("Random Spawn Parameter")]
    [SerializeField]
    protected float _randomSpawnDelay = 3;

    [Header("Circle Spawn Parameter")]
    [SerializeField]
    private bool _isLeftSpawnDireaction = true;
    [SerializeField]
    private float _circleSpawnDelay = 0.2f;

    [Header("Grid Spawn Parameter")]
    [SerializeField]
    private float _gridSpawnDelay = 0.2f;
    private bool _isGridCheck = false;

    [Header("Rain Spawn Parameter")]
    [SerializeField]
    private float _rainSpawnDelay = 0.2f;

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

        _spawnPos = new Vector2(0, _spawnArea.y);
        switch (_patternType)
        {
            case PatternType.Random:
                _timer = _randomSpawnDelay;
                break;
            case PatternType.Circle:
                _timer = _circleSpawnDelay;
                break;
            case PatternType.Grid:
                _timer = _gridSpawnDelay;
                break;
            case PatternType.Rain:
                _timer = _rainSpawnDelay;
                break;
        }
    }

    public override void OnStateLeave()
    {

    }

    public override void TakeAAction()
    {
        _timer += Time.deltaTime;

        switch (_patternType)
        {
            case PatternType.Random:
                RandomSpawn();
                break;
            case PatternType.Circle:
                CircleSpawn();
                break;
            case PatternType.Grid:
                GridSpawn();
                break;
            case PatternType.Rain:
                RainSpawn();
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
    }

    public virtual void RandomSpawn()
    {
        if (_timer >= _randomSpawnDelay)
        {
            SpawnProjectile(GetRandomSpawnPos(), Vector2.zero);
            _timer = 0f;
        }
    }

    public virtual void CircleSpawn() // 필드 테구리 따라 소환
    {
        if(_timer >= _circleSpawnDelay)
        {
            SpawnProjectile(_spawnPos, Vector2.zero);
            _timer = 0f;

            // _spawnPos = new Vector2(_spawnArea.x, _spawnArea.y)
            if ((_spawnPos.y == _spawnArea.y && Mathf.Abs(_spawnPos.x) < _spawnArea.x)
                || (_isLeftSpawnDireaction
                ? _spawnPos == new Vector2(_spawnArea.x, _spawnArea.y)
                : _spawnPos == new Vector2(-_spawnArea.x, _spawnArea.y))) // 위쪽 || 오른쪽 위
            {
                _spawnPos.x += _isLeftSpawnDireaction ? -1f : 1f;
            }
            else if((_spawnPos.y == -_spawnArea.y && Mathf.Abs(_spawnPos.x) < _spawnArea.x)
                || (_isLeftSpawnDireaction
                ? _spawnPos == new Vector2(-_spawnArea.x, -_spawnArea.y)
                : _spawnPos == new Vector2(_spawnArea.x, -_spawnArea.y))) // 아래쪽 || 왼쪽 아래
            {
                _spawnPos.x += _isLeftSpawnDireaction ? 1f : -1f;
            }
            else if((_spawnPos.x == _spawnArea.x && Mathf.Abs(_spawnPos.y) < _spawnArea.y)
                || (_isLeftSpawnDireaction
                ? _spawnPos == new Vector2(_spawnArea.x, -_spawnArea.y)
                : _spawnPos == new Vector2(_spawnArea.x, _spawnArea.y))) // 오른쪽 || 오른쪽 아래
            {
                _spawnPos.y += _isLeftSpawnDireaction ? 1f : -1f;
            }
            else if((_spawnPos.x == -_spawnArea.x && Mathf.Abs(_spawnPos.y) < _spawnArea.y)
                || (_isLeftSpawnDireaction
                ? _spawnPos == new Vector2(-_spawnArea.x, _spawnArea.y)
                : _spawnPos == new Vector2(-_spawnArea.x, -_spawnArea.y))) // 왼쪽 || 왼쪽 위
            {
                _spawnPos.y += _isLeftSpawnDireaction ? -1f : 1f;
            }
        }
    }

    public virtual void GridSpawn() // 격자로 생성
    {
        if(_timer >= _gridSpawnDelay)
        {
            float y = -_spawnArea.y + (_isGridCheck ? 2 : 1);
            while(y < _spawnArea.y)
            {
                SpawnProjectile(new Vector2(-_spawnArea.x, y), new Vector2(_spawnArea.x, y));
                y += 2;
            }

            float x = -_spawnArea.x + (_isGridCheck ? 1 : 2);
            while (x < _spawnArea.x)
            {
                SpawnProjectile(new Vector2(x, _spawnArea.y), new Vector2(x, -_spawnArea.y));
                x += 2;
            }

            _isGridCheck = !_isGridCheck;
            _timer = 0f;
        }
    }

    public virtual void RainSpawn() // 한곳 만 빼고 생성
    {
        if(_timer >= _rainSpawnDelay)
        {
            // 랜덤으로 위 아래 정하기
            // 스폰할 곳을 모두 뽑아 배열에 넣고 랜덤으로 한 곳 뽑음
            // 뽑은 곳 먼저 발사 후 좀 뒤에 나머지 다 발사
        }
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
