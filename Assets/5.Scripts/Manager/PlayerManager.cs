using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : ObjectManager {

    public List<BaseHero> heroList = new List<BaseHero>();
    public static CargoKart cargoKart;

    public override void Init()
    {
        base.Init();
        if (cargoKart == null)
        {
            cargoKart = FindObjectOfType<CargoKart>();
        }

        cargoKart.Init(this, false, 1);

        heroList.AddRange(FindObjectsOfType<BaseHero>());
        for (int i = 0; i < heroList.Count; i++)
        {
            //default level is 1
            heroList[i].Init(this, false, 1);
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

    public override BaseObject RequestTarget(BaseObject baseObject)
    {
        BaseObject returnObject = null;
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
        if (distance >= baseObject.objectData.attackRange + baseObject.objectData.sight)
        {
            return null;
        } else
        {
            return returnObject;
        }
    }
}
