using UnityEngine;
using System.Collections;

public class HighLightCircle : MonoBehaviour {
    [SerializeField]
    [Tooltip("the number of seconds it takes to fully rotate 1 round")]
    private float rotatingSpeed = 6f;
    [SerializeField]
    private GameObject targetGameObject;

    //[SerializeField]
    //private Color color;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    private float rotatingRate;
    private float currentYRotation;
    private Vector3 positionOffset;

    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = false;
        HideCircle();
        rotatingRate = 360f / rotatingSpeed;
        currentYRotation = 0f;

        positionOffset = new Vector3(0, transform.localPosition.y, 0);

        //if (color.a != 0)
        //{
        //    spriteRenderer.color = color;
        //}
        transform.parent = Directors.instance.mouseController.transform;
    }

    public void DoUpdate()
    {
        if (isActive)
        {
            FollowTargetObject();
            RotateFunction();
        }
    }

    private void ShowCircle()
    {
        spriteRenderer.enabled = true;
    }

    private void HideCircle()
    {
        spriteRenderer.enabled = false;
    }

    private void FollowTargetObject()
    {
        transform.position = targetGameObject.transform.position + positionOffset;
    }

    private void RotateFunction()
    {
        currentYRotation += rotatingRate * Time.deltaTime;
        if (currentYRotation >= 360f)
        {
            currentYRotation -= 360f;
        }
        transform.localRotation = Quaternion.Euler(new Vector3(-90f, currentYRotation, 0f));
    }

    //Deactive the circle so it does not follow anything.
    public void DeactiveCircle()
    {
        isActive = false;
        HideCircle();
    }

    //Set a target for this circle to follow
    public void SetTargetGameObject(GameObject _gameObject) {
        targetGameObject = _gameObject;
        if (targetGameObject == null)
        {
            isActive = false;
            HideCircle();
        } else
        {
            isActive = true;
            ShowCircle();
        }
    }
}
