using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO : Rewrote this entire class.
//Also try to add pools too
public class PrefabsManager {
    
    private static string unitDataPath = "Prefabs/Units/";
    private static string projectileDataPath = "Prefabs/Projectile/";
    private static string corpseDataPath = "Prefabs/Corpses/";

    private static string unitMaterialDataPath = "Materials/Units/";

    //object poolings
    public static Dictionary<string, List<PooledObject>> masterPool = new Dictionary<string, List<PooledObject>>();
    public static long objectCounts;


    //on scene reload
    public static void ClearPool()
    {
        objectCounts = 0;
        masterPool.Clear();
    }

    public static PooledObject GetObjectFromPool(string objectType, string resourcePath)
    {
        PooledObject objectPrefab;
        PooledObject returnObject;
        //create a tempt list
        List<PooledObject> tempList = null;
        //try to get the list of said value from the pool
        masterPool.TryGetValue(objectType, out tempList);
        if (tempList == null)
        {
            //if we don't have that list yet, create it, and load the prefab.
            tempList = new List<PooledObject>();
            masterPool.Add(objectType, tempList);
            objectPrefab = (Resources.Load(resourcePath + objectType) as GameObject).GetComponent<PooledObject>();
            //the first value of a list will always be the value.
            tempList.Add(objectPrefab);
        } else
        {
            objectPrefab = tempList[0];
        }
        if (tempList.Count <= 1)
        {
            //if the pool is empty, we must instantiate a new variable
            returnObject = GameObject.Instantiate<PooledObject>(objectPrefab);
            returnObject.SetPool(tempList);
        } else
        {
            //0 is the prefab value.
            returnObject = tempList[1];
            tempList.RemoveAt(1);
            returnObject.gameObject.SetActive(true);
        }
        returnObject.id = objectCounts;
        objectCounts++;
        returnObject.transform.parent = Directors.mouseController.transform;
        return returnObject;
    }

    public static BaseObject SpawnUnit(ObjectType type)
    {
        if (type == ObjectType.Invalid)
        {
            Debug.Log("<color=red>OBJECT TYPE not assigned. PLEASE CHECK !!!</color>");
            Debug.Break();
            return null;
        }
        return GetObjectFromPool(type.ToString(), unitDataPath) as BaseUnit;
    }
    
    public static Corpse SpawnCorpse(CorpseType type)
    {
        if (type == CorpseType.Invalid)
        {
            Debug.Log("<color=red>CORPSE TYPE not assigned. PLEASE CHECK !!!</color>");
            Debug.Break();
            return null;
        }
        return GetObjectFromPool(type.ToString(), corpseDataPath) as Corpse;
    }

    public static BasicProjectile SpawnProjectile(ProjectileType projectileType)
    {
        if(projectileType == ProjectileType.Invalid)
        {
            Debug.Log("<color=red>PROJECTILE not defined !!!! please check asap !!!  </color>");
            Debug.Break();
            return null;
        }
        return GetObjectFromPool(projectileType.ToString(), projectileDataPath) as BasicProjectile;
    }

    //TODO : recheck this function
    public static Material GetMaterialColor(ObjectType unitType)
    {
        string objectPath = string.Empty;
        switch (unitType)
        {
            case ObjectType.Fire_Hero:
            case ObjectType.Water_Hero:
            case ObjectType.Fire_Creep:
            case ObjectType.Water_Creep:
            case ObjectType.CargoKart:
                objectPath = unitType.ToString();
                break;
            default:
                Debug.Log("<color=red>MATERIAL not defined !!!! please check asap !!!  </color>");
                Debug.Break();
                break;
        }
        Material _mat;
        _mat = (Resources.Load(unitMaterialDataPath + objectPath)) as Material;
        return _mat;
    }

}


