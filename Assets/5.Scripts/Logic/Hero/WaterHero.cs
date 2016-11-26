using UnityEngine;
using System.Collections;

public class WaterHero : HeroObject {

    protected override void ObjectAttack()
    {
        base.ObjectAttack();
    }

    protected override void DealDamageToTarget()
    {
        if (targetObject != null)
        {
            projectileManager.CreateProjectile(ProjectileType.Water_Hero, false, objectData.objectDamange, transform.position , targetObject, GetObjectElement());
        }
    }

    public override GameElement GetObjectElement()
    {
        return GameElement.Water;
    }
}
