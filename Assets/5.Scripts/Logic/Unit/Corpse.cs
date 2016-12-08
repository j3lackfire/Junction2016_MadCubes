using UnityEngine;
using System.Collections;

public class Corpse : PooledObject
{
    public void Init(Vector3 position)
    {
        transform.position = position + (new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.3f, 0.6f), Random.Range(-0.3f, 0.3f)));
        ScheduleDestroyCorpse(Random.Range(2.5f, 3.5f));
    }

    public void ScheduleDestroyCorpse(float time)
    {
        StartCoroutine(DestroyCorpse(time));
    }

    private IEnumerator DestroyCorpse(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }
}

public enum CorpseType
{
    Invalid,
    Fire_Creep_Corpse,
    Water_Creep_Corpse
}