using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseController : BaseManager
{

    public GameObject hightLightCircle;
    public GameObject movementCircle;
    public GameObject attackedCircle;

    [SerializeField]
    public BaseHero currentlySelectedHero;

    private Ray ray;
    private RaycastHit hit;
    private int hitLayer = 0;

    public override void Init()
    {
        //Layers:
        //8 = Ground (for movement)
        //9 = Objects
        hitLayer = 1 << 8 | 1 <<  9;
        hightLightCircle.SetActive(false);
        movementCircle.SetActive(false);
        attackedCircle.SetActive(false);
    }

    public override void DoUpdate()
    {
        
        //Click
        if (Input.GetMouseButtonDown(0))
        {
            //Create a ray
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if the ray cast hit one of my layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayer))
            {
                switch (hit.transform.tag)
                {
                    case "Hero":
                        currentlySelectedHero = hit.transform.GetComponent<BaseHero>();
                        //hero.SetMovePosition(hit.point);
                        hightLightCircle.SetActive(true);
                        hightLightCircle.transform.parent = currentlySelectedHero.transform;
                        hightLightCircle.transform.localPosition = new Vector3(0f, 0.1f, 0f);
                        break;
                    case "Enemy":
                    case "Ground":
                        break;
                    case "Untagged":
                        Debug.Log("<color=red>This object is not tagged  !!!!!  </color>");
                        break;
                    default:
                        Debug.Log("<color=red>defa  5ult case ??? ????</color>");
                        break;
                }
            }
        }
        else
        {
            //Right click
            if (Input.GetMouseButtonDown(1))
            {
                //Create a ray
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //if the ray cast hit one of my layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayer))
                {
                    switch (hit.transform.tag)
                    {
                        case "Enemy":
                            if (currentlySelectedHero != null)
                            {
                                BaseObject baseTarget = hit.transform.gameObject.GetComponent<BaseObject>();
                                currentlySelectedHero.ChargeAtObject(baseTarget);
                                SetAttackMark(baseTarget.gameObject);
                            }
                            break;
                        case "Hero":
                            break;
                        case "Ground":
                            if (currentlySelectedHero != null)
                            {
                                currentlySelectedHero.SetMovePosition(hit.point);
                                SetMovementMark(hit.point);
                            }
                            break;
                        case "Untagged":
                            Debug.Log("<color=red>This object is not tagged  !!!!!  </color>");
                            break;
                        default:
                            Debug.Log("<color=red>default case ??? ????</color>");
                            break;
                    }
                }

            }
        }
        CheckKeyboardInput();
    }

    //TODO : Re check this function
    private void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentlySelectedHero = Directors.playerManager.heroList[0];
            //hero.SetMovePosition(hit.point);
            hightLightCircle.SetActive(true);
            hightLightCircle.transform.parent = currentlySelectedHero.transform;
            hightLightCircle.transform.localPosition = new Vector3(0f, 0.1f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            currentlySelectedHero = Directors.playerManager.heroList[1];
            //hero.SetMovePosition(hit.point);
            hightLightCircle.SetActive(true);
            hightLightCircle.transform.parent = currentlySelectedHero.transform;
            hightLightCircle.transform.localPosition = new Vector3(0f, 0.1f, 0f);
        }
    }

    private void SetMovementMark(Vector3 position)
    {
        movementCircle.SetActive(true);
        movementCircle.transform.position = position;
        StartCoroutine(MovementMarkEnum());
    }

    IEnumerator MovementMarkEnum()
    {
        float circleSize = 2f;
        movementCircle.transform.localScale = new Vector3(circleSize, 0.1f, circleSize);
        yield return null;
        while (true)
        {
            circleSize -= Time.deltaTime * 6f;
            movementCircle.transform.localScale = new Vector3(circleSize, 0.1f, circleSize);
            if (circleSize <= 0.1f)
            {
                movementCircle.SetActive(false);
                break;
            }
            yield return null;
        }
    }

    private void SetAttackMark(GameObject target)
    {
        attackedCircle.SetActive(true);
        attackedCircle.transform.position = target.transform.position - new Vector3(0f,-0.05f,0f);

        StartCoroutine(AttackMarkEnum(target));
    }

    IEnumerator AttackMarkEnum(GameObject target)
    {
        float circleSize = 3f;
        attackedCircle.transform.localScale = new Vector3(circleSize, 0.1f, circleSize);
        yield return null;
        while (true)
        {
            if (target == null || circleSize <= 0.1f)
            {
                attackedCircle.SetActive(false);
                break;
            }
            circleSize -= Time.deltaTime * 6f;
            attackedCircle.transform.position = target.transform.position - new Vector3(0f, -0.05f, 0f);
            attackedCircle.transform.localScale = new Vector3(circleSize, 0.1f, circleSize);
            yield return null;
        }
    }
}
