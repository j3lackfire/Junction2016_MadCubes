using UnityEngine;
using System.Collections;

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
}
