using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

    public virtual void Init() { }

    public virtual void DoUpdate() { }

    public virtual void RemoveObject(BaseElementObject baseObject) { }

    public virtual BaseElementObject RequestTarget(BaseElementObject baseObject) { return null; }

}
