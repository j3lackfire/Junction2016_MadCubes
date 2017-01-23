using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : ObjectManager {

    public List<BaseHero> heroList = new List<BaseHero>();
    private CargoKart cargoKart;

    public override void Init()
    {
        base.Init();
        CargoKart tempCargoKart = FindObjectOfType<CargoKart>();
        if (cargoKart != null)
        {
            Destroy(cargoKart.gameObject);
        }
        cargoKart = tempCargoKart;
        cargoKart.Init(this, false, 1);

        heroList = new List<BaseHero>();
        heroList.AddRange(FindObjectsOfType<BaseHero>());
        for (int i = 0; i < heroList.Count; i++)
        {
            //default level is 1
            heroList[i].Init(this, false, 1);
        }
        Directors.instance.StartBattle();
        Debug.Log("<color=green>Battle state  </color>" + Directors.instance.GetBattleState());
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

    public CargoKart GetCargoKart() { return cargoKart; }

    public override BaseObject RequestTarget(BaseObject baseObject)
    {
        BaseObject returnObject = null;
        float distance = 999f;
        for (int i = 0; i < Directors.instance.enemyManager.objectList.Count; i++)
        {
            if ((Directors.instance.enemyManager.objectList[i].transform.position
                - baseObject.transform.position).magnitude < distance)
            {
                returnObject = Directors.instance.enemyManager.objectList[i];
                distance = (Directors.instance.enemyManager.objectList[i].transform.position - baseObject.transform.position).magnitude;
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
