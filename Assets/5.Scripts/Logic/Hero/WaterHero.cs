using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterHero : HeroObject {

    protected override void ObjectAttack()
    {
        base.ObjectAttack();
    }

    Ray testRay;

    protected override void DealDamageToTarget()
    {
        if (targetObject != null)
        {
            projectileManager.CreateProjectile(ProjectileType.Water_Hero, false, objectData.objectDamange, transform.position , targetObject, GetObjectElement());
            testRay = new Ray(transform.position, targetObject.transform.position - transform.position);
            RaycastHit[] hitObject = Physics.RaycastAll(testRay, 50f);            
            for (int i = 0; i < hitObject.Length; i ++)
            {
                BaseElementObject hit = hitObject[i].transform.GetComponent<BaseElementObject>();
                if (hit != null)
                {
                    if (hit.isEnemy)
                    {
                        hit.ReceiveDamage(objectData.objectDamange, GetObjectElement());
                    }
                }
            }
            if (hitObject.Length >= 4)
            {
                Directors.cameraController.ScreenShake(ScreenShakeMagnitude.Small);
            }
        }
    }

    public override GameElement GetObjectElement()
    {
        return GameElement.Water;
    }

    public override void OnHeroRessurect()
    {
        base.OnHeroRessurect();
        StartCoroutine(MakeItRain());
    }

    IEnumerator MakeItRain()
    {
        List<BaseElementObject> enemyList = Directors.enemyManager.objectList;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null && (enemyList[i].objectData.objectAttackRange >= 10))
            {
                targetObject = enemyList[i];
                DealDamageToTarget();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
