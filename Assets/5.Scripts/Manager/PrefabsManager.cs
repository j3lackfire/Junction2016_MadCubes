using UnityEngine;
using System.Collections;

public class PrefabsManager {

    private static string unitDataPath = "Prefabs/Units/";
    private static string materialDataPath = "Materials/Units/";
    private static string projectileDataPath = "Prefabs/Projectile/";

    public static BaseElementObject SpawnUnit(GameElement element, bool isEnemy = false)
    {
        BaseElementObject _unit;
        if (isEnemy)
        {
            _unit = (Resources.Load(unitDataPath + element.ToString() + "_Creep") as GameObject).GetComponent<BaseElementObject>();
        }
        else
        {
            _unit = (Resources.Load(unitDataPath + element.ToString() + "_Hero") as GameObject).GetComponent<BaseElementObject>();
        }
        BaseElementObject unit = GameObject.Instantiate(_unit);

        return unit;
    }

    public static BasicProjectile SpawnProjectile(ProjectileType type)
    {
        BasicProjectile projectile = (Resources.Load(projectileDataPath + type.ToString()) as GameObject).GetComponent<BasicProjectile>();
        return projectile;
    }

    public static Material GetMaterialColor(GameElement gameElement, bool isHero = false)
    {
        Material _mat;
        if (!isHero)
        {
            _mat = (Resources.Load(materialDataPath + gameElement.ToString() + "_Creep")) as Material;
        }
        else
        {
            _mat = (Resources.Load(materialDataPath + gameElement.ToString() + "_Hero")) as Material;
        }
        return _mat;
    }
}
