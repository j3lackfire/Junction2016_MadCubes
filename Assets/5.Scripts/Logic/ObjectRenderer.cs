using UnityEngine;
using System.Collections;

//Use for rendering object. Currently do not have too much use
public class ObjectRenderer : MonoBehaviour {
    private BaseObject parentObject;
    protected Renderer healthBar;
    [SerializeField]
    protected Color healthBarColor = new Color(1f, 0f, 0f, 0.7f);
    [SerializeField]
    protected Color deadHealthBarColor = new Color(1f, 0f, 0f, 0.7f);

    public bool autoHideHealthBar = false;

    private Color tempColor;
    private float deltaR;
    private float deltaG;
    private float deltaB;
    private float alpha = 0.7f;

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
        deltaR = deadHealthBarColor.r - healthBarColor.r;
        deltaG = deadHealthBarColor.g - healthBarColor.g;
        deltaB = deadHealthBarColor.b - healthBarColor.b;
        //TODO : this might need re structure because this is bad. Child object called function straight from parents:/
        autoHideHealthBar = _parentObject.AutoHideHealthBar();
        if (autoHideHealthBar)
        {
            HideHealthBar();
        }
    }

    public void SetHealthBarColor(Color _color)
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
        //For circle health bar
        healthBar.material.SetFloat("_Cutoff",Mathf.Max(0f, 1f - healthPercent));
        tempColor = new Color(
            healthBarColor.r + deltaR * (1 - healthPercent),
            healthBarColor.g + deltaG * (1 - healthPercent),
            healthBarColor.b + deltaB * (1 - healthPercent), 
            alpha);
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
