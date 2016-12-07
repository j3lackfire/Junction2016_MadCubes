using UnityEngine;
using System.Collections;

//Use for rendering object. Currently do not have too much use
public class ObjectRenderer : MonoBehaviour {
    private BaseObject parentObject;
    public Renderer healthBar;

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
    }

    public void DoUpdateRenderer()
    {
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    public void UpdateHealthBar(float healthPercent)
    {
        healthBar.material.SetFloat("_Cutoff", 1f - healthPercent);
    }
}
