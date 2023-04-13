using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ObjectPool : MonoBehaviour
{
    public Transform holder;
    public static ObjectPool instance;
    private List<GameObject> poolObjects = new List<GameObject>();
    private int amountToPool = 0;

    [SerializeField] private GameObject bulletPrefab;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
   void Update()
   {
        
   }
   public GameObject GetPooledObject()
   {
        for(int i =0; i<poolObjects.Count; i++)
        {
            if(!poolObjects[i].activeInHierarchy)
            {
                return poolObjects[i];
            }
        }
        return null;
   }
   public void DeSpawn(GameObject obj)
   {
        poolObjects.Add(obj);
        obj.SetActive(false);
   }
   public GameObject Spawn()
   {
        foreach(GameObject poolObj in poolObjects)
        {
            if (poolObj == null) continue;

            if (poolObj.name == bulletPrefab.name)  
            {
                poolObjects.Remove(poolObj);
                return poolObj;
            }
        }

        GameObject newPrefab = Instantiate(bulletPrefab);
        newPrefab.transform.parent = holder;
        newPrefab.name = bulletPrefab.name;
        return newPrefab;
   }
}
