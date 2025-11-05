using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size = 10;
    }

    public static ObjectPool Instance;

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform); // opcional, para mantener la jerarquía limpia
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];

        GameObject obj = null;

        // Buscamos un objeto inactivo disponible
        foreach (var pooledObj in objectPool)
        {
            if (!pooledObj.activeInHierarchy)
            {
                obj = pooledObj;
                break;
            }
        }

        // Si no hay disponibles, instanciamos uno nuevo
        if (obj == null)
        {
            Pool poolData = pools.Find(p => p.tag == tag);
            if (poolData == null)
            {
                Debug.LogWarning($"No pool data found for tag {tag}");
                return null;
            }

            obj = Instantiate(poolData.prefab);
            obj.transform.SetParent(transform);
            objectPool.Enqueue(obj);
        }

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }


    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero; 

        foreach (var pool in pools)
        {
            if (pool.prefab.name == obj.name.Replace("(Clone)", "").Trim())
            {
                poolDictionary[pool.tag].Enqueue(obj);
                break;
            }
        }
    }


}
