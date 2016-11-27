using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireHero : HeroObject {

    public override GameElement GetObjectElement()
    {
        return GameElement.Fire;
    }

    protected override void DealDamageToTarget()
    {
        projectileManager.CreateProjectile(ProjectileType.Fire_Hero, false, objectData.objectDamange, transform.position, targetObject, GetObjectElement());
    }

    protected override void ObjectAttack()
    {
        float timeToDealDamage = 0.3f;
        attackCountDown += Time.deltaTime;
        if (attackCountDown >= timeToDealDamage)
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
        if (attackCountDown >= objectData.attackDuration)
        {
            FinnishAttackTarget();
        }
    }

    public override void OnHeroRessurect()
    {
        base.OnHeroRessurect();
        StartCoroutine(MakeItRain());
    }

    IEnumerator MakeItRain()
    {
        List<BaseElementObject> enemyList = Directors.enemyManager.objectList;
        for (int i = 0; i < enemyList.Count; i ++)
        {
            if (enemyList[i] != null && (enemyList[i].transform.position - transform.position).magnitude <= 20f)
            {
                targetObject = enemyList[i];
                DealDamageToTarget();
                DealDamageToTarget();
                yield return new WaitForSeconds(0.05f);
            }
        }

        //float seconds = 1.5f;
        //float countDown = 0.1f;
        //while (true)
        //{
        //    seconds -= Time.deltaTime;
        //    countDown -= Time.deltaTime;
        //    if (countDown <= 0f)
        //    {
        //        if (targetObject == null)
        //        {
        //            targetObject = objectManager.RequestTarget(this);
        //            if (targetObject != null)
        //            {
        //                DealDamageToTarget();
        //            }
        //        }
        //        countDown = 0.1f;
        //    }
        //    if (seconds <= 0)
        //    {
        //        break;
        //    }
        //    yield return null;
        //}
    }
}
