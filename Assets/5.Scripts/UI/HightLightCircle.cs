using UnityEngine;
using System.Collections;

public class HightLightCircle : MonoBehaviour {
    [SerializeField]
    [Tooltip("the number of seconds it takes to fully rotate 1 round")]
    private float rotatingSpeed = 6f;
    [SerializeField]
    private GameObject targetGameObject;
    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    private float rotatingRate;
    private float currentYRotation;
    private Vector3 positionOffset = new Vector3(0f, 0.2f, 0f);
    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = false;
        HideCircle();
        rotatingRate = 360f / rotatingSpeed;
        currentYRotation = 0f;
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

    public void FollowTargetObject()
    {
        transform.position = targetGameObject.transform.position + positionOffset;
    }

    public void RotateFunction()
    {
        currentYRotation += rotatingRate * Time.deltaTime;
        if (currentYRotation >= 360f)
        {
            currentYRotation -= 360f;
        }
        transform.localRotation = Quaternion.Euler(new Vector3(-90f, currentYRotation, 0f));
    }

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
