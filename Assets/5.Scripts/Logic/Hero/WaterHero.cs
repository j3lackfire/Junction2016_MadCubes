using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterHero : BaseHero {

    public override ProjectileType GetProjectileType()
    {
        return ProjectileType.Water_Hero_Laser;
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.Water_Hero;
    }

    protected override void ObjectAttack()
    {
        base.ObjectAttack();
    }

    Ray testRay;

    protected override void DealDamageToTarget()
    {
        if (targetObject != null)
        {
            Vector3 bulletEndPos = transform.position + (targetObject.transform.position - transform.position).normalized * 75f;
            projectileManager.CreateProjectile(ProjectileType.Water_Hero_Laser, false, objectData.damange, transform.position , this, bulletEndPos, targetObject);
            testRay = new Ray(transform.position, targetObject.transform.position - transform.position);
            RaycastHit[] hitObject = Physics.RaycastAll(testRay, 50f);
            for (int i = 0; i < hitObject.Length; i++)
            {
                BaseObject hit = hitObject[i].transform.GetComponent<BaseObject>();
                if (hit != null)
                {
                    if (hit.isEnemy)
                    {
                        hit.ReceiveDamage(objectData.damange);
                    }
                }
            }
            if (hitObject.Length >= 4)
            {
                Directors.cameraController.ScreenShake(ScreenShakeMagnitude.Small);
            }
        }
    }

    public override void OnHeroRessurect()
    {
        base.OnHeroRessurect();
        StartCoroutine(MakeItRain());
    }

    IEnumerator MakeItRain()
    {
        List<BaseObject> enemyList = Directors.enemyManager.objectList;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null && (enemyList[i].objectData.attackRange >= 10))
            {
                targetObject = enemyList[i];
                DealDamageToTarget();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
