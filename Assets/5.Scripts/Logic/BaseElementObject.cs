using UnityEngine;
using System.Collections;

public class BaseElementObject : MonoBehaviour {

    //Components
    private NavMeshAgent navMeshAgent;
    protected BaseObjectRenderer objectRenderer;
    protected AnimatorWrapper animatorWrapper;
    [SerializeField]
    protected ObjectManager objectManager;

    public bool isEnemy;

    [SerializeField]
    protected float objectMoveSpeed;

    [SerializeField]
    protected int objectHealth;

    [SerializeField]
    protected int objectDamange;

    private bool isMoving;
    private Vector3 targetPosition;

    public virtual void Init(ObjectManager _objectManager, bool _isEnemy)
    {
        isEnemy = _isEnemy;
        objectManager = _objectManager;
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        navMeshAgent.speed = objectMoveSpeed;
        isMoving = false;
        animatorWrapper = new AnimatorWrapper(GetComponentInChildren<Animator>());
    }

    public virtual void DoUpdate()
    {
        if(isMoving)
        {
            CheckStopMoving();
        }

        animatorWrapper.DoUpdate();
    }

    public void SetMovePosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        navMeshAgent.Resume();
        navMeshAgent.SetDestination(targetPosition);
        if (!isMoving)
        {
            animatorWrapper.AddTriggerToQueue("EnterMovementAnimation");
        }
        isMoving = true;
    }

    private void CheckStopMoving()
    {
        if( (transform.position - targetPosition).magnitude <= 0.5f)
        {
            isMoving = false;
            navMeshAgent.Stop();
            animatorWrapper.AddTriggerToQueue("EnterIdleAnimation");
        }
    }

    protected virtual void DealDamageToTarget()
    {

    }

    /// <summary>
    /// Element rule
    /// </summary>
    public void ReceiveDamage (int damage, GameElement attackedElement)
    {
        float damageMultiplier = 1f;
        switch (GetObjectElement())
        {
            case GameElement.Invalid:
            default:
                Debug.Log("<color=red>Invalid element, please check again.!!!! </color>");
                break;
            case GameElement.Fire:
                break;
            case GameElement.Water:
                break;
            case GameElement.Rock:
                break;
            case GameElement.Tree:
                break;
            case GameElement.Electricity:
                break;
            case GameElement.Ice:
                break;
        }
        ReduceHealth((int)(damageMultiplier * damage));
    }

    public void ReduceHealth(int damage)
    {

    }

    public virtual GameElement GetObjectElement ()
    {
        return GameElement.Invalid;
    }

    public int GetObjectDamage() { return objectDamange; }
    public int GetObjectHealth() { return objectHealth; }

}

public enum GameElement
{
    Invalid,
    Fire,
    Water,
    Rock,
    Tree,
    Electricity,
    Ice
}