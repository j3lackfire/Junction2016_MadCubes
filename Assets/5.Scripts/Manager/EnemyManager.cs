using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : ObjectManager {
    public List<BaseElementObject> objectList = new List<BaseElementObject>();

    public override void Init()
    {
        base.Init();
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].DoUpdate();
        }
    }

}
