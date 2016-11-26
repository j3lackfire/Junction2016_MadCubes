using UnityEngine;
using System.Collections;

public class PrefabsManager {

    private static string unitDataPath = "Prefabs/Units/";
    private static string materialDataPath = "Materials/";

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

    public static Material GetMaterialColor(GameElement gameElement)
    {
        Material _mat = (Resources.Load(materialDataPath + gameElement.ToString())) as Material;
        return _mat;
    }
}
