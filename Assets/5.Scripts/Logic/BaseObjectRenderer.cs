using UnityEngine;
using System.Collections;

public class BaseObjectRenderer : MonoBehaviour {
    private BaseElementObject parentObject;
    
    public void InitRenderer(BaseElementObject _parentObject)
    {
        if (_parentObject == null)
        {
            Debug.Log("<color=yellow>There is no parent object  </color>", this.gameObject);
            parentObject = GetComponentInParent<BaseElementObject>();
        } else
        {
            parentObject = _parentObject;
        }
    }

    //public void SetRendererColor(ObjectTeam materialColor)
    //{
    //    GetComponent<Renderer>().material = PrefabsManager.GetMaterialColor(materialColor);
    //}
}
