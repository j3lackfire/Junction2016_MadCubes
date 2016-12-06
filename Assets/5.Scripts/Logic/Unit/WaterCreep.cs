using UnityEngine;
using System.Collections;

public class WaterCreep : BaseUnit {

    public override ProjectileType GetProjectileType()
    {
        return ProjectileType.Water_Creep_Projectile;
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.Water_Creep;
    }

    public override CorpseType GetCorpseType()
    {
        return CorpseType.Water_Creep_Corpse;
    }
}
