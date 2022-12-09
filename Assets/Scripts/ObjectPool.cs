using System.Collections.Generic;
using UnityEngine;

// Novasloth Games LLC
// Lee Barton
namespace Novasloth {
    public sealed class ObjectPool : MonoBehaviour {

        ///////////////////////////////////////////////////////////////////
        // S I N G L E T O N
        ///////////////////////////////////////////////////////////////////

        private static ObjectPool instance = null;
        public static ObjectPool Instance {
            get {
                if (instance == null) {
                    Debug.LogWarning("ObjectPool :: Instance::get TrafficSystem not found in scene!");
                    return null;
                }

                return instance;
            }
        }

        ///////////////////////////////////////////////////////////////////
        // P U B L I C   V A R I A B L E S
        ///////////////////////////////////////////////////////////////////

        public int TotalActive { get; private set; } = 0;

        ///////////////////////////////////////////////////////////////////
        // P R I V A T E   V A R I A B L E S
        ///////////////////////////////////////////////////////////////////

        [Header("Object Pooling")]
        [SerializeField] private bool pooling = true;
        [SerializeField] private List<Pool> poolList;
        private Dictionary<string, Queue<GameObject>> poolsQueueLookup;
        private Dictionary<string, Pool> poolLookup;

        ///////////////////////////////////////////////////////////////////
        // U N I T Y   M E T H O D S
        ///////////////////////////////////////////////////////////////////

        private void Awake () {
            instance = this;
        }

        private void Start () {
            if (pooling) {
                GeneratePool();
            }
        }

        private void OnDestroy () {
            instance = null;
        }

        ///////////////////////////////////////////////////////////////////
        // H E L P E R   M E T H O D S   ( P O O L I N G )
        ///////////////////////////////////////////////////////////////////

        private void GeneratePool () {
            Debug.Log("Generating object pool...");

            poolsQueueLookup = new Dictionary<string, Queue<GameObject>>();
            poolLookup = new Dictionary<string, Pool>();

            GameObject objInstance;
            foreach (Pool pool in poolList) {
                Queue<GameObject> poolQueue = new Queue<GameObject>();

                for (int i = 0; i < pool.ObjectTotal; i++) {
                    objInstance = Instantiate(pool.RandomPrefab, (pool.Parent == null ? null : pool.Parent));
                    objInstance.SetActive(false);
                    poolQueue.Enqueue(objInstance);
                }

                pool.InactiveCount = poolQueue.Count;
                poolLookup.Add(pool.Tag, pool);
                poolsQueueLookup.Add(pool.Tag, poolQueue);
            }

            Debug.Log("Object pool generated!");
        }

        public GameObject GetPooledObject (string poolTag) {
            if (pooling) {
                Queue<GameObject> poolQueue = poolsQueueLookup[poolTag];
                Pool pool = poolLookup[poolTag];

                if (poolQueue.Count > 0) {
                    GameObject pooledObject = poolQueue.Dequeue();
                    pool.InactiveCount = poolQueue.Count;
                    pool.ActiveCount++;
                    TotalActive++;

                    float scale = Random.Range(1.0f, 2.5f);
                    pooledObject.transform.localScale = new Vector3(scale, scale, scale);
                    return pooledObject;
                }
            }

            return null;
        }

        public void ReturnObjectToPool (string poolTag, GameObject activeObject) {
            Queue<GameObject> poolQueue = poolsQueueLookup[poolTag];
            Pool pool = poolLookup[poolTag];

            activeObject.SetActive(false);
            poolQueue.Enqueue(activeObject);

            pool.InactiveCount = poolQueue.Count;
            pool.ActiveCount--;
            TotalActive--;
        }

        ///////////////////////////////////////////////////////////////////
        // P O O L   O B J E C T
        ///////////////////////////////////////////////////////////////////

        [System.Serializable]
        private class Pool {

            // Public variables
            public string Tag { get { return tag; } }
            public GameObject RandomPrefab { get { return prefabs[Random.Range(0, prefabs.Count)]; } }
            public Transform Parent { get { return parentTransform; } }
            public int ObjectTotal { get { return poolCount; } }
            public int ActiveCount { get { return activeCount; } set { activeCount = value; } }
            public int InactiveCount { get { return inactiveCount; } set { inactiveCount = value; } }

            // Private variables
            [SerializeField] private string tag;
            [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
            [SerializeField] private Transform parentTransform;
            [SerializeField] private int poolCount = 0;
            [SerializeField] private int activeCount = 0;
            [SerializeField] private int inactiveCount = 0;
        }
    }
}
