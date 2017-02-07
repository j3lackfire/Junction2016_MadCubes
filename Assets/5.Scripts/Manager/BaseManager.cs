using UnityEngine;
using System.Collections;

public class BaseManager : MonoBehaviour {
    protected Directors director;

    public virtual void Init()
    {
        director = Directors.instance;
    }

    public virtual void DoUpdate() { }
}
