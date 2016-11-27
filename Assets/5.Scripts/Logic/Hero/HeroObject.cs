using UnityEngine;
using System.Collections;

public class HeroObject : BaseElementObject {
    
    public virtual void ActiveSkill()
    {

    }

    public virtual void OnHeroDie()
    {
        objectState = ObjectState.Die;
    }

    public virtual void OnHeroRessurect()
    {

    }

    public override void ReduceHealth(int damage)
    {
        objectData.objectHealth -= damage;
        if (objectData.objectHealth <= 0)
        {
            //dead effect.
            for (int i = 0; i < 30; i++)
            {
                var corpse = GameObject.CreatePrimitive(PrimitiveType.Cube);
                corpse.transform.position = gameObject.transform.position + (new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)));
                corpse.transform.localScale = new Vector3(0.3333f, 0.3333f, 0.3333f);
                corpse.GetComponent<Renderer>().material = PrefabsManager.GetMaterialColor(GetObjectElement(), !isEnemy);
                corpse.AddComponent<Rigidbody>();
                Destroy(corpse, 3f);
            }
            //end dead effect
            OnHeroDie();
        }
    }



}
