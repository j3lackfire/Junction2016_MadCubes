using UnityEngine;
using System.Collections;

//Use for rendering object. Currently do not have too much use
public class ObjectRenderer : MonoBehaviour {
    private BaseObject parentObject;
    protected Renderer healthBar;
    [SerializeField]
    protected Color healthBarColor = new Color(1f, 0f, 0f, 0.65f);

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
    }

    public void SetHealthBarColor(Color _color)
    {
        healthBar.material.SetColor("_Color", _color);
    }

    public void DoUpdateRenderer()
    {
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    public void UpdateHealthBar(float healthPercent)
    {
        //For circle health bar
        healthBar.material.SetFloat("_Cutoff",Mathf.Max(0f, 1f - healthPercent));
    }
}
