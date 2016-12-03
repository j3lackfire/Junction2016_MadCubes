using UnityEngine;
using System.Collections;

public class Ultilities {

    public static float GetDistanceBetween(GameObject first, GameObject second)
    {
        return ((first.transform.position - second.transform.position).magnitude);
    }
}
