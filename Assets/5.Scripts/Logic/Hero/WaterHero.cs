using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterHero : BaseHero {

    //cached ray for attacking, because this object can attack serveral
    private Ray attackRay;

    public override ProjectileType GetProjectileType()
    {
        return ProjectileType.Water_Hero_Laser;
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.Water_Hero;
    }

    public override CorpseType GetCorpseType()
    {
        return CorpseType.Water_Creep_Corpse;
    }

    protected override void DealDamageToTarget()
    {
        if (targetObject != null)
        {
            //use number of enemy hit for screen shake.
            int numberOfEnemyHit = 0;
            Vector3 bulletEndPos = transform.position + (targetObject.transform.position - transform.position).normalized * 75f;
            projectileManager.CreateProjectile(ProjectileType.Water_Hero_Laser, false, objectData.damange, transform.position , this, bulletEndPos, targetObject);
            attackRay = new Ray(transform.position, targetObject.transform.position - transform.position);
            RaycastHit[] hitObject = Physics.RaycastAll(attackRay, 50f);

            for (int i = 0; i < hitObject.Length; i++)
            {
                BaseObject hit = hitObject[i].transform.GetComponent<BaseObject>();
                if (hit != null)
                {
                    if (hit.isEnemy)
                    {
                        hit.ReceiveDamage(objectData.damange);
                        numberOfEnemyHit++;
                    }
                }
            }
            if (numberOfEnemyHit >= 3)
            {
                cameraController.ScreenShake(ScreenShakeMagnitude.Small);
            }
        }
    }

    public override bool ActiveSpecial()
    {
        if (!base.ActiveSpecial())
        {
            return false;
        }
        StartCoroutine(SpellAttackAllBigCreep());
        return true;
    }

    //Special skill of water hero. Attack on big target.
    private IEnumerator SpellAttackAllBigCreep()
    {
        SetState(ObjectState.Special);
        //cached this list for shorter code
        List<BaseObject> enemyList = enemyManager.objectList;
        //My target = all target that isn't fire creep
        //TODO: change this logic later when create more enemies.
        List<BaseObject> myTargetList = new List<BaseObject>();
        for (int i = 0; i < enemyList.Count; i ++)
        {
            if (enemyList[i].GetObjectType() != ObjectType.Fire_Creep)
            {
                myTargetList.Add(enemyList[i]);
            }
        }

        attackCountUp = 0;
        int waitFramesBetweenAttack = 5;
        int currentTargetIndex = 0;
        //Safety setup to make sure we don't hit the wrong object.
        long[] myTargetID = new long[myTargetList.Count];
        for (int i = 0; i < myTargetList.Count; i++)
        {
            myTargetID[i] = myTargetList[i].id;
        }
        while(attackCountUp < objectData.attackDuration)
        {
            for (int i = 0; i < waitFramesBetweenAttack; i++)
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
