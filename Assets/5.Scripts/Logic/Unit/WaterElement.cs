using UnityEngine;
using System.Collections;

public class WaterElement : UnitObject {

    public override GameElement GetObjectElement()
    {
        return GameElement.Water;
    }
}
