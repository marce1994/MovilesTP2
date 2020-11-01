using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    private Dictionary<string, Queue<GameObject>> _pool;
    public PooleableSources[] pooleableSources;

    [Serializable]
    public class PooleableSources
    {
        public string name;
        public string resourcePath;
        public GameObject[] prefabs;
    }

    private void Awake()
    {
        pooleableSources = new PooleableSources[]
        {
            new PooleableSources(){ name = "Asteroid", resourcePath = "Level1/Asteroids"},
            new PooleableSources(){ name = "Explosion", resourcePath = "Level1/Particles"}
        };

        foreach (var pooleableSource in pooleableSources)
            pooleableSource.prefabs = Resources.LoadAll<GameObject>(pooleableSource.resourcePath);
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

        GameObject obj = _pool[key].Count() > 0? _pool[key].Dequeue() : null;

        if (obj == null)
            obj = Instantiate(pooleableSources.First(x => x.name == key).prefabs.First());

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        obj.SetActive(true);

        return obj;
    }

    private void EnsureInitialized(string key)
    {
        if (_pool == null)
            _pool = new Dictionary<string, Queue<GameObject>>();
        if (!_pool.ContainsKey(key))
            _pool.Add(key, new Queue<GameObject>());
    }
}
