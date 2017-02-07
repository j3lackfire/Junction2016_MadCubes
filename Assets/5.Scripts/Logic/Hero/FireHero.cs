using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireHero : BaseHero {

    //for the fire hero unique super fast shooting
    float timeToDealDamage = 0.3f;

    public override ProjectileType GetProjectileType()
    {
        return ProjectileType.Fire_Hero_Laser;
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.Fire_Hero;
    }

    public override CorpseType GetCorpseType()
    {
        return CorpseType.Fire_Creep_Corpse;
    }

    protected override void PrepareComponent()
    {
        base.PrepareComponent();
    }

    protected override void DealDamageToTarget()
    {
        projectileManager.CreateProjectile(GetProjectileType(), isEnemy, objectData.damange, transform.position, this, 
            targetObject.transform.position + new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), targetObject);
    }

    protected override void StartAttackTarget()
    {
        base.StartAttackTarget();
        timeToDealDamage = 0.3f;
    }

    protected override void ObjectAttack()
    {
        //timeToDealDamage = 0.3f;
        attackCountUp += Time.deltaTime;
        if (attackCountUp >= timeToDealDamage)
        {
            if (targetObject == null || targetObject.objectState == ObjectState.Die || IsTargetChanged())
            {
                targetObject = objectManager.RequestTarget(this);
                if (targetObject == null)
                {
                    //set this value to super high so the hero will not attack again in this turn
                    timeToDealDamage = 999f;
                }
            }
            if (timeToDealDamage >= 1.25f)
            {
                timeToDealDamage = 999f;
            }
            if (timeToDealDamage != 999f)
            {
                DealDamageToTarget();
                timeToDealDamage += 0.075f;
            }
        }
        if (attackCountUp >= objectData.attackDuration)
        {
            FinnishAttackTarget();
        }
    }

    public override bool ActiveSpecial()
    {
        if (!base.ActiveSpecial())
        {
            return false;
        }
        StartCoroutine(SpellAttackAllNearbyCreey());
        return true;
    }

    //public override void OnHeroRessurect() { base.OnHeroRessurect(); }

    //TODO : Find better solution
    //is there a way to remove ienumerator because it doesn't match my DoUpdate style very good :/
    private IEnumerator SpellAttackAllNearbyCreey()
    {
        SetState(ObjectState.Special);
        attackCountUp = 0;
        int waitFramesBetweenAttack = 5;
        int currentTargetIndex = 0;
        List<BaseObject> myTargetList = enemyManager.GetObjectInArea(transform.position, objectData.attackRange * 2f);
        //Safety setup to make sure we don't hit the wrong object.
        long[] myTargetID = new long[myTargetList.Count];
        for (int i = 0; i < myTargetList.Count; i ++)
        {
            myTargetID[i] = myTargetList[i].id;
        }

        while (attackCountUp < objectData.attackDuration)
        {
            for (int i = 0; i < waitFramesBetweenAttack; i ++)
            {
                attackCountUp += Time.deltaTime;
                yield return null;
            }
            if (currentTargetIndex < myTargetList.Count)
            {
                //in case of object die and respawned as another object due to object pooling.
                if (myTargetID[currentTargetIndex] == myTargetList[currentTargetIndex].id)
                {
                    SetTargetObject(myTargetList[currentTargetIndex]);
                    DealDamageToTarget();
                }
                currentTargetIndex++;
            }
        }
        SetState(ObjectState.Idle);
    }
}

