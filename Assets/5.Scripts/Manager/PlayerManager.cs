using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : ObjectManager {

    public List<BaseElementObject> objectList = new List<BaseElementObject>();
    public CargoKart cargoKart;

    public override void Init()
    {
        base.Init();
        if (cargoKart == null)
        {
            cargoKart = FindObjectOfType<CargoKart>();
        }
        cargoKart.Init();

        objectList.AddRange(FindObjectsOfType<HeroObject>());
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].Init(this, false);
        }

    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        for (int i = 0; i < objectList.Count; i ++)
        {
            objectList[i].DoUpdate();
        }
        cargoKart.DoUpdate();
    }
}
