using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton
{
    private Dictionary<string, Queue<GameObject>> _pool;

    public class PooleableSources
    {
        public string resourcePath;
        public GameObject[] prefabs;
    }

    public GameObject RecicleGameObject(string key, GameObject gameObject)
    {
        EnsureInitialized(key);

        gameObject.SetActive(false);
        _pool[key].Enqueue(gameObject);

        return gameObject;
    }

    public GameObject InstantiateFromPool(string key, Vector3 position, Quaternion rotation)
    {
        EnsureInitialized(key);

        var obj = _pool[key].Dequeue();
        
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        obj.SetActive(true);

        return _pool[key].Dequeue();
    }

    private void EnsureInitialized(string key)
    {
        if (_pool == null)
            _pool = new Dictionary<string, Queue<GameObject>>();
        if (!_pool.ContainsKey(key))
            _pool.Add(key, new Queue<GameObject>());
    }
}
