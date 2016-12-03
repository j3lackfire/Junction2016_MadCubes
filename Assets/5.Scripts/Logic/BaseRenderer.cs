using UnityEngine;
using System.Collections;

//Use for rendering object. Currently do not have too much use
public class BaseRenderer : MonoBehaviour {
    private BaseObject parentObject;
    
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
    }
}
