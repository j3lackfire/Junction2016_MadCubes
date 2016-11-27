using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterHero : HeroObject {

    protected override void ObjectAttack()
    {
        base.ObjectAttack();
    }

    Ray testRay;

    protected override void DealDamageToTarget()
    {
        if (targetObject != null)
        {
            projectileManager.CreateProjectile(ProjectileType.Water_Hero, false, objectData.objectDamange, transform.position , targetObject, GetObjectElement());
            testRay = new Ray(transform.position, targetObject.transform.position - transform.position);
            RaycastHit[] hitObject = Physics.RaycastAll(testRay, 50f);
            for (int i = 0; i < hitObject.Length; i ++)
            {
                BaseElementObject hit = hitObject[i].transform.GetComponent<BaseElementObject>();
                if (hit != null)
                {
                    if (hit.isEnemy)
                    {
                        hit.ReceiveDamage(objectData.objectDamange, GetObjectElement());
                    }
                }
            }
        }
    }

    public override GameElement GetObjectElement()
    {
        return GameElement.Water;
    }

    //public static List<GameObject> corpseList = new List<GameObject>();
    //public static List<GameObject> avaiableCorpseList = new List<GameObject>();

    //public static GameObject RequestCorpse(Vector3 position, GameElement gameElement, bool isEnemy)
    //{
    //    if (avaiableCorpseList.Count <= 0)
    //    {
    //        GameObject newCoprse = GenerateCoprse(position, gameElement, isEnemy);
    //        corpseList.Add(newCoprse);
    //        return newCoprse;
    //    } else
    //    {
    //        GameObject cc = avaiableCorpseList[0];
    //        corpseList.Add(cc);
    //        avaiableCorpseList.RemoveAt(0);
    //        return cc;
    //    }
    //}

    //public static GameObject GenerateCoprse(Vector3 position, GameElement gameElement, bool isEnemy)
    //{
    //    GameObject corpse = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    corpse.transform.position = position + (new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)));
    //    corpse.transform.localScale = new Vector3(0.3333f, 0.3333f, 0.3333f);
    //    corpse.GetComponent<Renderer>().material = PrefabsManager.GetMaterialColor(gameElement, isEnemy);
    //    corpse.AddComponent<Rigidbody>();
    //    return corpse;
    //}

    //public static GameObject RemoveCorpse()
    //{

    //}
}
