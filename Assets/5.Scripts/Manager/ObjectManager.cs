using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : BaseManager
{
    public override void DoUpdate() { }

    public virtual void RemoveObject(BaseObject baseObject) { }

    public virtual BaseObject RequestTarget(BaseObject baseObject) { return null; }
}
