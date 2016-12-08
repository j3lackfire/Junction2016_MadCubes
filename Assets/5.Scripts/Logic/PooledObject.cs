using UnityEngine;
using System.Collections.Generic;

public class PooledObject : MonoBehaviour {
    [HideInInspector]
    public long id;
    protected List<PooledObject> myPool;
    protected bool isFirstInit = true;

    protected virtual void OnFirstInit()
    {
        if (isFirstInit)
        {
            isFirstInit = false;
        }
    }

    public void SetFirstInit()
    {
        isFirstInit = true;
    }

    public void SetPool(List<PooledObject> pool)
    {
        myPool = pool;
    } 

    //should this function be called internally or externally ?
    //It makes a little more sense to call it externally.
    protected void ReturnToPool()
    {
        if (myPool == null)
        {
            Destroy(this.gameObject);
            return;
        }
        myPool.Add(this);
        gameObject.SetActive(false);
    }
}
