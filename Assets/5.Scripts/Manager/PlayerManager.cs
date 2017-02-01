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
        //director.StartBattle();
        Debug.Log("<color=green>Battle state  </color>" + director.GetBattleState());
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

    //Get the nearest hero to the object.
    //Can be used for both team, enemy and friendly.
    public BaseHero GetNearestHero(BaseObject baseObject)
    {
        if (heroList.Count ==  0)
        {
            return null;
        }

        BaseHero returnHero = null;
        float distance = 999f;
        for (int i = 0; i < heroList.Count; i ++)
        {
            if (baseObject == heroList[i])
            {
                continue;
            }
            float myDistance = (baseObject.transform.position - heroList[i].transform.position).magnitude;
            if (myDistance < distance)
            {
                returnHero = heroList[i];
                distance = myDistance;
            }
        }
        return returnHero;
    }

    public override BaseObject RequestTarget(BaseObject baseObject)
    {
        BaseObject returnObject = null;
        float distance = 999f;
        for (int i = 0; i < director.enemyManager.objectList.Count; i++)
        {
            if ((director.enemyManager.objectList[i].transform.position
                - baseObject.transform.position).magnitude < distance)
            {
                returnObject = director.enemyManager.objectList[i];
                distance = (director.enemyManager.objectList[i].transform.position - baseObject.transform.position).magnitude;
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
