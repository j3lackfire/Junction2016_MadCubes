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

    public override CorpseType GetCorpseType()
    {
        return CorpseType.Water_Creep_Corpse;
    }

    protected override void ObjectAttack()
    {
        base.ObjectAttack();
    }

    Ray attackRay;

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

    public override void ActiveSpecial()
    {
        base.ActiveSpecial();
        StartCoroutine(SpellAttackAllBigCreep());
    }

    //public override void OnHeroRessurect() { base.OnHeroRessurect(); }

    private IEnumerator SpellAttackAllBigCreep()
    {
        SetState(ObjectState.Special);
        List<BaseObject> enemyList = Directors.instance.enemyManager.objectList;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null && (enemyList[i].GetObjectType() == ObjectType.Water_Creep))
            {
                targetObject = enemyList[i];
                DealDamageToTarget();
                yield return new WaitForSeconds(0.1f);
            }
        }
        SetState(ObjectState.Idle);
    }
}
