using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            //GameObject go = Managers
            //GameObject go = .GetOriginal(name);
            GameObject go = (GameManager.Instance.PoolManager_).GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefab/{path}");
        if (original == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return (GameManager.Instance.PoolManager_).Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            (GameManager.Instance.PoolManager_).Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public void Destroy(GameObject go, float delay)
    {
        GameManager.Instance.StartCoroutine(DestroyCoroutine(go, delay));
    }
    
    public IEnumerator DestroyCoroutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.transform.rotation = Quaternion.identity;
        Destroy(go);
    }
}
