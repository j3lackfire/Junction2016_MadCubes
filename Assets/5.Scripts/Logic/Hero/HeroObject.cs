using UnityEngine;
using System.Collections;

public class HeroObject : BaseElementObject {

    float deadCountDown;

    public override void Init(ObjectManager _objectManager, bool _isEnemy, int level)
    {
        base.Init(_objectManager, _isEnemy, level);
    }

    public virtual void ActiveSkill() { }

    public virtual void OnHeroDie()
    {
        if (Directors.mouseController.currentlySelectedHero == this)
        {
            Directors.mouseController.currentlySelectedHero = null;
            Directors.mouseController.hightLightCircle.transform.gameObject.SetActive(false);
        }
        objectState = ObjectState.Die;
        Directors.cameraController.ScreenShake(ScreenShakeMagnitude.Big);
        childAnimator.gameObject.SetActive(false);
        deadCountDown = objectData.objectRespawnTime;
    }

    public override void WhileHeroDie()
    {
        base.WhileHeroDie();
        deadCountDown -= Time.deltaTime;
        if (deadCountDown <= 0)
        {
            //hero is alive.
            //navMeshAgent.Move(PlayerManager.cargoKart.transform.position);
            transform.position = PlayerManager.cargoKart.transform.position + new Vector3(Random.Range(-2f,2f), 0f, Random.Range(-2f, 2f));
            childAnimator.gameObject.SetActive(true);
            objectState = ObjectState.Idle;
            animatorWrapper.AddTriggerToQueue("EnterIdleAnimation");
            OnHeroRessurect();
        }
    }

    public virtual void OnHeroRessurect() {
        objectState = ObjectState.Idle;
        childAnimator.gameObject.SetActive(true);
        objectData.objectLevel++;
        UpdateStatsByLevel(objectData.objectLevel);
    }

    public override void UpdateStatsByLevel(int level)
    {
        objectData.objectLevel = level;
        objectData.objectMaxHealth = objectData.objectMaxHealth + (int)(objectData.objectMaxHealth * level * 0.22f);
        objectData.objectDamange = objectData.objectDamange + (int)(objectData.objectDamange * level * 0.22f);
        objectData.objectHealth = objectData.objectMaxHealth;
    }

    public override void ReduceHealth(int damage)
    {
        if(objectState == ObjectState.Die)
        {
            return;
        }
        objectData.objectHealth -= damage;
        if (objectData.objectHealth <= 0)
        {
            //dead effect.
            for (int i = 0; i < 30; i++)
            {
                var corpse = GameObject.CreatePrimitive(PrimitiveType.Cube);
                corpse.transform.position = gameObject.transform.position + (new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)));
                corpse.transform.localScale = new Vector3(0.3333f, 0.3333f, 0.3333f);
                corpse.GetComponent<Renderer>().material = PrefabsManager.GetMaterialColor(GetObjectElement(), false);
                corpse.AddComponent<Rigidbody>();
                Destroy(corpse, Random.Range(2.5f, 3.5f));
            }
            //end dead effect
            OnHeroDie();
        }
    }

}
