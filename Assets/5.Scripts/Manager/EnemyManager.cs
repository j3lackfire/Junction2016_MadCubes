using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : ObjectManager {
    int currentSpawnLevel = 1;

    public List<BaseObject> objectList = new List<BaseObject>();

    public List<GameObject> enemySpawnPos = new List<GameObject>();

    public GameObject spawnPointParent;

    public float levelCountDown;

    public float summonRate;
    private float spawnCountDown = 1;

    public override void Init()
    {
        base.Init();
        spawnCountDown = 1;
        levelCountDown = GameConstant.increaseSpawnTime;
        summonRate = GameConstant.initialSpawnRate;
        //todo remove later
        objectList.AddRange(FindObjectsOfType<BaseUnit>());
        for (int i = 0; i < objectList.Count; i ++)
        {
            objectList[i].Init(this, true, 1);
        }
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        switch (director.GetBattleState())
        {
            case BattleState.Prepare:
                break;
            case BattleState.Battling:
                CheckSpawnUnit();
                CheckIncreaseLevel();
                break;
            case BattleState.Finish:
                //Debug.Log("<color=1278ba>~~~state </color>" + Directors.instance.GetBattleState());
                break;
            default:
                break;
        }
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].DoUpdate();
        }
    }

    private void CheckSpawnUnit()
    {
        spawnCountDown -= Time.deltaTime;
        if (spawnCountDown <= 0)
        {
            SpawnUnit(ObjectType.Fire_Creep);
            if (Random.Range(0, GameConstant.spawnWaterCreepOdds) < 1)
            {
                SpawnUnit(ObjectType.Water_Creep);
            }
            spawnCountDown += Random.Range(summonRate * 0.75f, summonRate * 1.25f);

            //BaseObject test =SpawnUnit(ObjectType.Water_Creep);
            //test.objectData.maxHealth = 99999;
            //test.objectData.health = test.objectData.maxHealth;
            //spawnCountDown += 999999;
        }
    }

    private void CheckIncreaseLevel()
    {
        levelCountDown -= Time.deltaTime;
        if (levelCountDown <= 0)
        {
            levelCountDown += GameConstant.increaseSpawnTime;
            if (summonRate >= GameConstant.maxSpawnRate)
            {
                summonRate -= GameConstant.spawnRateIncreaseValue;
            }
            //currentSpawnLevel++;

            //need to rework this thing to make it better :/
            UpdateSpawnPosition();
        }
    }

    /// <summary>
    /// Get all enemy in an area. An Area is defined by center with radius
    /// </summary>
    /// <param name="centerPoint"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public List<BaseObject> GetObjectInArea(Vector3 centerPoint, float radius)
    {
        List<BaseObject> returnList = new List<BaseObject>();
        for (int i = 0; i < objectList.Count; i ++)
        {
            BaseObject cachedObject = objectList[i];
            if ((centerPoint - cachedObject.transform.position).magnitude < radius)
            {
                returnList.Add(cachedObject);
            }
        }
        return returnList;
    }

    public BaseObject SpawnUnit(ObjectType objectType)
    {
        if (objectList.Count >= GameConstant.enemySpawnCap)
        {
            return null;
        }
        BaseObject creep = PrefabsManager.SpawnUnit(objectType);
        //set at random position, for now
        creep.transform.position = enemySpawnPos[Random.Range(0, enemySpawnPos.Count)].transform.position;
        creep.Init(this, true, currentSpawnLevel);
        objectList.Add(creep);
        creep.ChargeAtObject(RequestTarget(creep));
        creep.transform.parent = transform;
        return creep;
    }

    public override void RemoveObject(BaseObject baseObject)
    {
        objectList.Remove(baseObject);
    }

    public override BaseObject RequestTarget(BaseObject baseObject)
    {
        BaseObject returnObject = null;
        float distance = 999f;
        List<BaseHero> heroList = Directors.instance.playerManager.heroList;
        for (int i = 0; i < heroList.Count; i ++)
        {
            if (((heroList[i].transform.position
                - baseObject.transform.position).magnitude < distance) 
                && heroList[i].objectState != ObjectState.Die)
            {
                returnObject = heroList[i];
                distance = (heroList[i].transform.position - baseObject.transform.position).magnitude;
            }
        }
        BaseObject cargo = Directors.instance.playerManager.GetCargoKart();
        if (cargo == null || cargo.objectState == ObjectState.Die)
        {
            return returnObject;
        }
        float distanceToCargo = (cargo.transform.position - baseObject.transform.position).magnitude;
        if (distanceToCargo < distance)
        {
            return cargo;
        } else
        {
            if (distance < 20f)
            {
                return returnObject;
            } else
            {
                return cargo;
            }
        }
    }

    public void UpdateSpawnPosition()
    {
        spawnPointParent.transform.position = Directors.instance.playerManager.GetCargoKart().transform.position;
        float sumRandom = 60f;
        float randomX;
        float randomZ;
        for (int i = 0; i < enemySpawnPos.Count; i ++)
        {
            randomX = Random.Range(1f, sumRandom * 0.98f);
            randomZ = sumRandom - randomX;
            randomX = Random.Range(0, 2) == 0 ? -randomX : randomX;
            randomZ = Random.Range(0, 2) == 0 ? -randomZ : randomZ;
            enemySpawnPos[i].transform.localPosition = new Vector3(randomX, 0f, randomZ);
        }
    }

} 
