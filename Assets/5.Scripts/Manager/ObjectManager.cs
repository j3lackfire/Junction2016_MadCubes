using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : BaseManager
{
    protected Directors director;

    public override void Init()
    {
        director = Directors.instance;
    }

    public override void DoUpdate() { }

    public virtual void RemoveObject(BaseObject baseObject) { }

    public virtual BaseObject RequestTarget(BaseObject baseObject) { return null; }
}
