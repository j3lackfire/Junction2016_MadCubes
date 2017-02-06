using UnityEngine;
using System.Collections;

//Use for rendering object. Currently do not have too much use
public class ObjectRenderer : MonoBehaviour {
    private BaseObject parentObject;
    protected Renderer healthBar;
    [SerializeField] //should be green
    protected Color healthBarColor = new Color(1f, 0f, 0f, 0.7f);
    [SerializeField] //should be yellow
    protected Color middleHealthBarColor = new Color(1f, 1f, 0f, 0.7f);
    [SerializeField] //red
    protected Color deadHealthBarColor = new Color(1f, 0f, 0f, 0.7f);

    public bool autoHideHealthBar = false;

    private Color tempColor;
    //Need to be vector 3 since value can be negative
    private Vector3 deltaHealthColorFullToMiddle;
    private Vector3 deltaHealthColorMiddleToDie;

    //private float deltaR;
    //private float deltaG;
    //private float deltaB;
    private float alpha = 0.7f;

    private Vector3 initialLocalPosition;

    //private function
    private bool isHealthBarShow = true;

    public void InitRenderer(BaseObject _parentObject)
    {
        if (_parentObject == null)
        {
            Debug.Log("<color=yellow>There is no parent object  </color>", gameObject);
            parentObject = GetComponentInParent<BaseObject>();
        } else
        {
            parentObject = _parentObject;
        }
        if (healthBar == null)
        {
            healthBar = transform.parent.FindChild("HealthCircle").GetComponent<Renderer>();
        }
        SetHealthBarColor(healthBarColor);

        deltaHealthColorFullToMiddle = new Vector3 (
            middleHealthBarColor.r - healthBarColor.r,
            middleHealthBarColor.g - healthBarColor.g,
            middleHealthBarColor.b - healthBarColor.b);

        deltaHealthColorMiddleToDie = new Vector3(
            deadHealthBarColor.r - middleHealthBarColor.r,
            deadHealthBarColor.g - middleHealthBarColor.g,
            deadHealthBarColor.b - middleHealthBarColor.b);

        //deltaR = deadHealthBarColor.r - healthBarColor.r;
        //deltaG = deadHealthBarColor.g - healthBarColor.g;
        //deltaB = deadHealthBarColor.b - healthBarColor.b;
        //TODO : this might need re structure because this is bad. Child object called function straight from parents:/
        autoHideHealthBar = _parentObject.AutoHideHealthBar();
        if (autoHideHealthBar)
        {
            HideHealthBar();
        }
        initialLocalPosition = transform.localPosition;
    }

    //When the parent object die
    public void OnParentObjectDie()
    {
        ResetRendererPosition();
    }
    //When we respawn it back
    public void OnParentObjectRespawn()
    {

    }

    //Object die while moving, when respawned will have a weird position offset.
    //This is to fix that bug
    private void ResetRendererPosition()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void SetHealthBarColor(Color _color)
    {
        healthBar.material.SetColor("_Color", _color);
    }

    public void DoUpdateRenderer()
    {
        //update the health bar rotation so that it stays in once place.
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    public void UpdateHealthBar()
    {
        UpdateHealthBar((float)parentObject.objectData.health / (float)parentObject.objectData.maxHealth);
    }

    public void UpdateHealthBar(float healthPercent)
    {
        if (healthPercent == 1 && autoHideHealthBar)
        {
            HideHealthBar();
        } else
        {
            CheckShowHealthBar();
        }
        if (healthPercent > 0.5f) //Full health to middle bar
        {
            float deltaColorPercent = (1 - healthPercent) * 2;
            tempColor = new Color(
                healthBarColor.r + deltaHealthColorFullToMiddle.x * deltaColorPercent,
                healthBarColor.g + deltaHealthColorFullToMiddle.y * deltaColorPercent,
                healthBarColor.b + deltaHealthColorFullToMiddle.z * deltaColorPercent, 
                alpha);
        } else // Middle bar to die
        {
            float deltaColorPercent = (0.5f - healthPercent) * 2;
            tempColor = new Color(
                middleHealthBarColor.r + deltaHealthColorMiddleToDie.x * deltaColorPercent,
                middleHealthBarColor.g + deltaHealthColorMiddleToDie.y * deltaColorPercent,
                middleHealthBarColor.b + deltaHealthColorMiddleToDie.z * deltaColorPercent,
                alpha);
        }
        //For circle health bar
        healthBar.material.SetFloat("_Cutoff",Mathf.Max(0f, 1f - healthPercent));
        //tempColor = new Color(
        //    healthBarColor.r + deltaR * (1 - healthPercent),
        //    healthBarColor.g + deltaG * (1 - healthPercent),
        //    healthBarColor.b + deltaB * (1 - healthPercent), 
        //    alpha);
        SetHealthBarColor(tempColor);
    }

    private void HideHealthBar()
    {
        isHealthBarShow = false;
        healthBar.gameObject.SetActive(false);
    }

    private void CheckShowHealthBar ()
    {
        if (isHealthBarShow)
        {
            return;
        }
        isHealthBarShow = true;
        healthBar.gameObject.SetActive(true);
    }
}
