using UnityEngine;
using System.Collections.Generic;

public class PooledObject : MonoBehaviour {

    public long id;
    protected List<PooledObject> myPool;

    public void SetPool(List<PooledObject> pool)
    {
        myPool = pool;
    } 

    protected void ReturnToPool()
    {
        myPool.Add(this);
        gameObject.SetActive(false);
    }
}
