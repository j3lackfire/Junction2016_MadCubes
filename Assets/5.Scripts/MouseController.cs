using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    public GameObject hightLightCircle;
    public GameObject movementCircle;
    public GameObject attackedCircle;

    [SerializeField]
    public HeroObject currentlySelectedHero;

    private Ray ray;
    private RaycastHit hit;
    private int hitLayer = 0;

    public void Init()
    {
        //Layers:
        //8 = Ground (for movement)
        //9 = Objects
        hitLayer = 1 << 8 | 1 <<  9;
        hightLightCircle.SetActive(false);
        movementCircle.SetActive(false);
        attackedCircle.SetActive(false);
    }

    public void DoUpdate()
    {
        //Click
        if (Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject()) //not clicking on UI
        {
            //Create a ray
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if the ray cast hit one of my layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayer))
            {
                switch (hit.transform.tag)
                {
                    case "Hero":
                        currentlySelectedHero = hit.transform.GetComponent<HeroObject>();
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
            if (Input.GetMouseButtonDown(1)
                && !EventSystem.current.IsPointerOverGameObject()) //not clicking on UI
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
                                BaseElementObject baseTarget = hit.transform.gameObject.GetComponent<BaseElementObject>();
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
