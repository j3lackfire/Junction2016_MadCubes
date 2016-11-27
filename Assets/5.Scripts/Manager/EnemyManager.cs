using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : ObjectManager {
    public List<BaseElementObject> objectList = new List<BaseElementObject>();

    public List<GameObject> enemySpawnPos = new List<GameObject>();

    public GameObject spawnPointParent;

    public override void Init()
    {
        base.Init();
    }

    float countDown = 1;

    public override void DoUpdate()
    {
        base.DoUpdate();
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].DoUpdate();
        }
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            SpawnEnemy(GameElement.Fire);
            if(Random.Range(0,4) < 1)
            {
                SpawnEnemy(GameElement.Water);
            }
            countDown += Random.Range(0.35f, 0.65f);
        }
    }

    public override void RemoveObject(BaseElementObject baseObject)
    {
        objectList.Remove(baseObject);
    }

    public void SpawnEnemy(GameElement type)
    {
        BaseElementObject creep = PrefabsManager.SpawnUnit(type, true);
        //set at random position, for now
        creep.transform.position = enemySpawnPos[Random.Range(0 , enemySpawnPos.Count)].transform.position;
        creep.Init(this, true);
        objectList.Add(creep);
        creep.ChargeAtObject(RequestTarget(creep));
    }

    public override BaseElementObject RequestTarget(BaseElementObject baseObject)
    {
        BaseElementObject returnObject = null;
        float distance = 999f;
        for (int i = 0; i < Directors.playerManager.heroList.Count; i ++)
        {
            if ((Directors.playerManager.heroList[i].transform.position
                - baseObject.transform.position).magnitude < distance)
            {
                returnObject = Directors.playerManager.heroList[i];
                distance = (Directors.playerManager.heroList[i].transform.position - baseObject.transform.position).magnitude;
            }
        }
        float distanceToCargo = (PlayerManager.cargoKart.transform.position - baseObject.transform.position).magnitude;
        if (distanceToCargo < distance)
        {
            return PlayerManager.cargoKart;
        } else
        {
            if (distance < 20f)
            {
                return returnObject;
            } else
            {
                return PlayerManager.cargoKart;
            }
        }
    }

}
