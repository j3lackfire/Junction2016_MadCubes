using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireHero : BaseHero {

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
        //projectileManager.CreateProjectile(ProjectileType.Fire_Hero, false, objectData.damange, transform.position, targetObject, GetObjectElement());
        targetObject.ReceiveDamage(objectData.damange);
    }

    protected override void ObjectAttack()
    {
        float timeToDealDamage = 0.3f;
        attackCountUp += Time.deltaTime;
        if (attackCountUp >= timeToDealDamage)
        {
            if (targetObject == null)
            {
                targetObject = objectManager.RequestTarget(this);
                if (targetObject == null)
                {
                    timeToDealDamage = 999f;
                }
            }
            if (timeToDealDamage >= 1.15f)
            {
                timeToDealDamage = 999f;
            }
            if (timeToDealDamage != 999f)
            {
                DealDamageToTarget();
                timeToDealDamage += 0.2f;
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
