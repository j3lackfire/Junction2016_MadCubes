using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : ObjectManager {

    public List<HeroObject> heroList = new List<HeroObject>();
    public static CargoKart cargoKart;

    public override void Init()
    {
        base.Init();
        if (cargoKart == null)
        {
            cargoKart = FindObjectOfType<CargoKart>();
        }
        cargoKart.Init(this, false);

        heroList.AddRange(FindObjectsOfType<HeroObject>());
        for (int i = 0; i < heroList.Count; i++)
        {
            heroList[i].Init(this, false);
        }

    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        for (int i = 0; i < heroList.Count; i ++)
        {
            heroList[i].DoUpdate();
        }
        cargoKart.DoUpdate();
    }

    public override BaseElementObject RequestTarget(BaseElementObject baseObject)
    {
        BaseElementObject returnObject = null;
        float distance = 999f;
        for (int i = 0; i < Directors.enemyManager.objectList.Count; i++)
        {
            if ((Directors.enemyManager.objectList[i].transform.position
                - baseObject.transform.position).magnitude < distance)
            {
                returnObject = Directors.enemyManager.objectList[i];
                distance = (Directors.enemyManager.objectList[i].transform.position - baseObject.transform.position).magnitude;
            }
        }
        if (distance >= 10f)
        {
            return null;
        } else
        {
            return returnObject;
        }
    }
}
