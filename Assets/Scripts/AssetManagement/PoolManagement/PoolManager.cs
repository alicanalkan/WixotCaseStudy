using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Wixot.Engine;
using Object = System.Object;

namespace Wixot.AssetManagement.PoolManagement
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] private List<PoolInfo> poolList = new List<PoolInfo>();
        
        private void Start()
        {
            FillPool(poolList);
        }

        /// <summary>
        /// Fills the pool according to the given pool info
        /// </summary>
        /// <param name="info"></param>
        private void FillPool(List<PoolInfo> info)
        {
            AssetManagement.AssetManager.Instance.LoadAllAssets(info.Select(p => p.addressableTag),OnPoolObjectLoaded);
        }

        private void OnPoolObjectLoaded(Dictionary<string,Object> objects)
        {
            for (var i = 0; i < poolList.Count; i++)
            {
                for (int j = 0; j < poolList[i].amount; j++)
                {
                    var poolObj =
                        AssetManager.Instance.Instantiate(poolList[i].addressableTag, transform.position, quaternion.identity,
                            transform);
                    poolObj.name += $" {i}";
                    poolObj.SetActive(false);
                    poolList[i].pooledObjects.Enqueue(poolObj);
                }
                
            }
        }

        /// <summary>
        /// Gets pool object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject GetPoolObject(string type)
        {
            var selectedPool = GetPoolByType(type);

            GameObject poolObj;
            if (selectedPool.pooledObjects.Count > 0)
            {
                poolObj = selectedPool.pooledObjects.Dequeue();
            }
            else
            { 
                poolObj =
                    AssetManager.Instance.Instantiate(selectedPool.addressableTag, transform.position, quaternion.identity,
                        transform);
                poolObj.name += $" {selectedPool.pooledObjects.Count}";
            }

            poolObj.SetActive(true);

            return poolObj;
        }

        /// <summary>
        /// Retrieves the object back in its pool by type.
        /// </summary>
        /// <param name="poolObj"></param>
        /// <param name="type"></param>
        public void DestroyObject(GameObject poolObj, string type)
        {
            poolObj.SetActive(false);

            if (poolObj.transform.parent != transform)
            {
                poolObj.transform.SetParent(transform);
            }
            
            var selectedPool = GetPoolByType(type);
            
            if(!selectedPool.pooledObjects.Contains(poolObj))
                selectedPool.pooledObjects.Enqueue(poolObj);
        }

        /// <summary>
        /// Gets the pool based on the type of object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private PoolInfo GetPoolByType(string type)
        {
            foreach (var poolInfo in poolList)
            {
                if (type == poolInfo.addressableTag)
                    return poolInfo;
            }

            return null;
        }

    }

    [Serializable]
    public class PoolInfo
    {
        public string addressableTag;
        public int amount = 0;
        public readonly Queue<GameObject> pooledObjects = new Queue<GameObject>();
    }
}