using UnityEngine;
using System.Collections;

//TODO : Rewrote this entire class.
//Also try to add pools too
public class PrefabsManager {

    private static string unitDataPath = "Prefabs/Units/";
    private static string projectileDataPath = "Prefabs/Projectile/";

    private static string unitMaterialDataPath = "Materials/Units/";

    //TODO rewrote this function
    public static BaseObject SpawnUnit(ObjectType type)
    {
        BaseObject _unit;
        _unit = (Resources.Load(unitDataPath + type.ToString()) as GameObject).GetComponent<BaseObject>();
        BaseObject unit = GameObject.Instantiate(_unit);

        return unit;
    }

    //TODO recheck these function
    public static BasicProjectile SpawnProjectile(ProjectileType projectileType)
    {
        string objectPath = string.Empty;
        switch (projectileType)
        {
            case ProjectileType.Fire_Hero_Laser:
            case ProjectileType.Water_Hero_Laser:
            case ProjectileType.Fire_Creep_Projectile:
            case ProjectileType.Water_Creep_Projectile:
                objectPath = projectileType.ToString();
                break;
            case ProjectileType.Invalid:
            default:
                Debug.Log("<color=red>PROJECTILE not defined !!!! please check asap !!!  </color>");
                Debug.Break();
                break;
        }
        //Debug.Log(projectileDataPath + objectPath);
        BasicProjectile projectile = (Resources.Load(projectileDataPath + objectPath) as GameObject).GetComponent<BasicProjectile>();
        return projectile;
    }

    //TODO : recheck this function too
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
