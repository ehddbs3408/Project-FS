using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeArea : MonoBehaviour
{
    private List<Life> _lifeList = new List<Life>();

    private void Start()
    {
        for(int i = 0; i < ProjectileManager.Instance.GetAgent().MaxHP; i++)
        {
            Life life = GameManager.Instance.ResourceManager_.Instantiate("Life", this.transform).GetComponent<Life>();
            life.UpdateHeart(true);
            _lifeList.Add(life);
        }
    }

    public void UpdateHeart()
    {
        if (ProjectileManager.Instance.GetAgent().CurrentHP < 0) return;
        _lifeList[ProjectileManager.Instance.GetAgent().CurrentHP].UpdateHeart(false);
    }
}
