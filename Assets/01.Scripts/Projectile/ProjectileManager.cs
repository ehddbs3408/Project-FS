using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    private List<Bullet> _bulletList = new List<Bullet>();

    [SerializeField]
    private Agent _agent;

    private AIBrain _fsm;
    public AIBrain FSM => _fsm;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _fsm = GetComponent<AIBrain>();
    }

    public Agent GetAgent()
    {
        return _agent;
    }

    public void AddBullet(Bullet bullet)
    {
       _bulletList.Add(bullet);
    }

    public void RemoveBullet(Bullet bullet)
    {
        if (_bulletList.Contains(bullet))
        {
            _bulletList.Remove(bullet);
        }
    }

    public void ClearBullet()
    {
        for(int i = 0; i < _bulletList.Count; i++)
        {
            _bulletList[i].Destroy();
        }

        _bulletList.Clear();
    }

    public void StopStage()
    {
        _fsm.ChangeState(null);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MiniGame");
    }
}
