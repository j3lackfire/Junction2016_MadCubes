using UnityEngine;
using System.Collections;

public class WaterHero : HeroObject {

    protected override void ObjectAttack()
    {
        base.ObjectAttack();
    }


    public override GameElement GetObjectElement()
    {
        return GameElement.Water;
    }
}
