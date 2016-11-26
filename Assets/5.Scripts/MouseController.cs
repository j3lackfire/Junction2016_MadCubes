using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    public GameObject hightLightCircle;



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
    }

    public void DoUpdate()
    {
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
                    case "Unit":
                        break;
                    case "Hero":
                        currentlySelectedHero = hit.transform.GetComponent<HeroObject>();
                        //hero.SetMovePosition(hit.point);
                        hightLightCircle.SetActive(true);
                        hightLightCircle.transform.parent = currentlySelectedHero.transform;
                        hightLightCircle.transform.localPosition = new Vector3(0f, 0.1f, 0f);
                        break;
                    case "Ground":
                        break;
                    case "Untagged":
                        Debug.Log("<color=red>This object is not tagged  !!!!!  </color>");
                        break;
                    default:
                        Debug.Log("<color=red>default case ??? ????</color>");
                        break;
                }


            }
            else
            {
                Debug.Log("<color=red>Raycast does not hit anything. Might be a problem. </color>");
            }
        }
        else
        {
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
                        case "Unit":
                            break;
                        case "Hero":
                            break;
                        case "Ground":
                            if (currentlySelectedHero != null)
                            {
                                currentlySelectedHero.SetMovePosition(hit.point);
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
}
