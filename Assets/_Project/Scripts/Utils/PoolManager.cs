using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private Dictionary<string, LinkedList<object>> _pools = new Dictionary<string, LinkedList<object>>();

    #region Get
    public T GetPool<T>(T prefab, Vector3 pos, Quaternion quaternion) where T : Component
    {
        string name = prefab.gameObject.name;
        if (!_pools.ContainsKey(name))
            _pools[name] = new LinkedList<object>();

        T result;

        if (_pools[name].Count > 0)
        {
            result = GetPoolFirst<T>(name, pos, quaternion);
            return result;
        }

        result = Object.Instantiate(prefab, pos, quaternion);
        result.gameObject.name = name;

        return result;
    }

    public T GetPool<T>(T prefab, Vector3 pos) where T : Component
    {
        string name = prefab.gameObject.name;
        if (!_pools.ContainsKey(name))
            _pools[name] = new LinkedList<object>();

        T result;
        var quaternion = prefab.transform.rotation;

        if (_pools[name].Count > 0)
        {
            result = GetPoolFirst<T>(name, pos, quaternion);
            return result;
        }

        result = Object.Instantiate(prefab, pos, quaternion);
        result.gameObject.name = name;

        return result;
    }

    private T GetPoolFirst<T>(string name, Vector3 pos, Quaternion quaternion) where T : Component
    {
        T result = _pools[name].First.Value is T
            ? _pools[name].First.Value as T
            : (_pools[name].First.Value as GameObject).GetComponent<T>();

        result.transform.rotation = quaternion;
        result.transform.position = pos;
        result.gameObject.SetActive(true);
        _pools[name].RemoveFirst();

        return result;
    }

    #endregion

    #region Set
    public void SetPool(GameObject target)
    {
        if (!_pools.ContainsKey(target.name))
        {
            Object.Destroy(target);
            return;
        }

        _pools[target.name].AddFirst(target);
        target.SetActive(false);
    }

    public void SetPool(Transform target)
    {
        if (!_pools.ContainsKey(target.name))
        {
            Object.Destroy(target);
            return;
        }

        _pools[target.name].AddFirst(target.gameObject);
        target.gameObject.SetActive(false);
    }

    public void SetPool<T>(T target) where T : Component
    {
        if (!_pools.ContainsKey(target.gameObject.name))
        {
            Object.Destroy(target.gameObject);
            return;
        }

        _pools[target.name].AddFirst(target);
        target.gameObject.SetActive(false);
    }
    #endregion

    public void ClearPool()
    {
        _pools.Clear();
    }
}