using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;

    private List<Bullet> _bulletList = new List<Bullet>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
}
