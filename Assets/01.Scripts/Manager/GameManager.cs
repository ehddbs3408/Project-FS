using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PoolManager PoolManager_ = new PoolManager();
    public ResourceManager ResourceManager_ = new ResourceManager();


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple GameManager!!");
        }
        Instance = this;

        PoolManager_.Awake();
    }


}
