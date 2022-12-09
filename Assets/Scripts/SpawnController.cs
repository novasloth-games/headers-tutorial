using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Novasloth Games LLC
// Lee Barton
namespace Novasloth {
    public class SpawnController : MonoBehaviour {

        ///////////////////////////////////////////////////////////////////
        // P R I V A T E   V A R I A B L E S
        ///////////////////////////////////////////////////////////////////

        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float spawnDelay = 0.75f;
        [SerializeField] private Vector3 maxPosition;
        [SerializeField] private float error = 0.001f;

        private Vector3 nextPosition = Vector3.zero;
        private bool reachedPosition;

        private Timer spawnTimer;

        ///////////////////////////////////////////////////////////////////
        // U N I T Y   M E T H O D S
        ///////////////////////////////////////////////////////////////////

        private void Awake () {
            spawnTimer = new Timer(spawnDelay, () => {
                GameObject pooledObject;
                if (Random.Range(0, 2) == 0)
                    pooledObject = ObjectPool.Instance.GetPooledObject("sphere");
                else
                    pooledObject = ObjectPool.Instance.GetPooledObject("cube");

                if (pooledObject != null) {
                    pooledObject.transform.position = transform.position;
                    pooledObject.SetActive(true);
                }
            });
        }

        private void Start () {
            GetNextPosition();
            spawnTimer.Start();
        }

        private void Update () {
            spawnTimer.Tick();

            CheckReachedPosition();
            if (reachedPosition) {
                GetNextPosition();
            }

            transform.position = Vector3.Lerp(
                transform.position,
                nextPosition,
                speed * Time.deltaTime
            );
        }

        ///////////////////////////////////////////////////////////////////
        // H E L P E R   M E T H O D S
        ///////////////////////////////////////////////////////////////////

        private void GetNextPosition () {
            nextPosition = new Vector3(
                Random.Range(-1 * maxPosition.x, maxPosition.x),
                maxPosition.y,
                Random.Range(-1 * maxPosition.z, maxPosition.z)
            );
        }

        private void CheckReachedPosition () {
            reachedPosition = Vector3.Distance(transform.position, nextPosition) <= error;
        }
    }
}
