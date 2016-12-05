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
        timeToDealDamage = 0.3f;
        attackCountUp += Time.deltaTime;
        if (attackCountUp >= timeToDealDamage)
        {
            if (targetObject == null)
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
                timeToDealDamage += 0.05f;
            }
        }
        if (attackCountUp >= objectData.attackDuration)
        {
            FinnishAttackTarget();
        }
    }

    public override void OnHeroRessurect()
    {
        base.OnHeroRessurect();
        StartCoroutine(MakeItRain());
    }

    //TODO : Find better solution
    //is there a way to remove ienumerator because it doesn't match my DoUpdate style very good :/
    private IEnumerator MakeItRain()
    {
        List<BaseObject> enemyList = Directors.enemyManager.objectList;
        for (int i = 0; i < enemyList.Count; i ++)
        {
            if (enemyList[i] != null && Ultilities.GetDistanceBetween(gameObject, enemyList[i].gameObject) <= objectData.attackRange * 2f)
            {
                targetObject = enemyList[i];
                DealDamageToTarget();
                DealDamageToTarget();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
