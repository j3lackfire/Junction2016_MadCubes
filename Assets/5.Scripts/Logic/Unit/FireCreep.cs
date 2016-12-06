using UnityEngine;
using System.Collections.Generic;

public class FireCreep : BaseUnit {
    public override ProjectileType GetProjectileType()
    {
        return ProjectileType.Fire_Creep_Projectile;
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.Fire_Creep;
    }

    public override CorpseType GetCorpseType()
    {
        return CorpseType.Fire_Creep_Corpse;
    }
}
