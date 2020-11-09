using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    private Dictionary<string, List<GameObject>> _pool;
    private PooleableSources[] pooleableSources;

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
            new PooleableSources(){ name = "Explosion", resourcePath = "Level1/Particles"},
            new PooleableSources(){ name = "Missile", resourcePath = "Level1/Projectiles"}
        };

        foreach (var pooleableSource in pooleableSources)
            pooleableSource.prefabs = Resources.LoadAll<GameObject>(pooleableSource.resourcePath);
    }

    public GameObject Recicle(string key, GameObject gameObject)
    {
        EnsureInitialized(key);

        gameObject.SetActive(false);
        _pool[key].Add(gameObject);

        return gameObject;
    }

    public GameObject InstantiateFromPool(string key, Vector3 position, Quaternion rotation)
    {
        EnsureInitialized(key);

        GameObject obj = _pool[key].Where(x => !x.activeSelf).Count() > 0 ? _pool[key].Where(x => !x.activeSelf).ToList().PopAt(0) : null;

        if (obj == null)
        {
            var objects = pooleableSources.First(x => x.name == key);
            obj = Instantiate(objects.prefabs.ElementAt(UnityEngine.Random.Range(0, objects.prefabs.Length)));
            obj.SetActive(false);
        }

        IPooledObject pooledObject = obj.GetComponent<IPooledObject>();

        if (pooledObject != null)
            pooledObject.OnInstantiate();

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        obj.SetActive(true);

        return obj;
    }

    private void EnsureInitialized(string key)
    {
        if (_pool == null)
            _pool = new Dictionary<string, List<GameObject>>();
        if (!_pool.ContainsKey(key))
            _pool.Add(key, new List<GameObject>());
    }

    private new void OnDestroy()
    {
        foreach (KeyValuePair<string, List<GameObject>> kv in _pool)
        {
            foreach (GameObject gameObject in kv.Value)
            {
                DestroyImmediate(gameObject);
            }
        }

        _pool = null;
        pooleableSources = null;

        Resources.UnloadUnusedAssets();

        base.OnDestroy();
    }
}
