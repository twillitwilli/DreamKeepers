using System.Collections.Generic;
using UnityEngine;

namespace SoT.AbstractClasses
{
    public abstract class PoolManager<T> : MonoBehaviour where T : PoolManager<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Debug.Log(typeof(T).ToString() + " is NULL");

                return _instance;
            }
        }

        [SerializeField]
        private GameObject _objectPrefab;

        private List<GameObject> _objectPool = new List<GameObject>();

        private int _objectIndex;

        public virtual void Awake()
        {
            _instance = this as T;
        }

        public GameObject GetItem()
        {
            GameObject newItem;

            if (_objectPool.Count < 1)
            {
                newItem = SpawnNewItem(_objectPrefab, _objectPool);

                return newItem;
            }

            else if (!_objectPool[0].activeSelf)
            {
                newItem = _objectPool[0];
                _objectIndex = 0;

                return newItem;
            }

            else
            {
                _objectIndex++;
                _objectIndex = _objectIndex > (_objectPool.Count - 1) ? 0 : _objectIndex;

                newItem = GetItemFromPool(_objectIndex, _objectPool);

                return newItem;
            }
        }

        GameObject GetItemFromPool(int poolIdx, List<GameObject> whichPool)
        {
            GameObject itemFromPool = null;

            bool spawnNewItem = whichPool[poolIdx].activeSelf ? true : false;

            if (spawnNewItem)
                itemFromPool = SpawnNewItem(_objectPrefab, whichPool);

            else
                itemFromPool = whichPool[poolIdx];

            if (!itemFromPool.activeSelf)
                itemFromPool.SetActive(true);

            return itemFromPool;
        }

        GameObject SpawnNewItem(GameObject newItem, List<GameObject> whichPool)
        {
            GameObject item = Instantiate(newItem);
            whichPool.Add(item);
            item.transform.SetParent(transform);

            return item;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;

            if (obj.activeSelf)
                obj.SetActive(false);
        }
    }
}
