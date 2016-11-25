using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private CargoKart targetCargoKart;

    [SerializeField]
    public Vector3 cargoPos;


    void Awake()
    {
        if (targetCargoKart == null)
        {
            targetCargoKart = FindObjectOfType<CargoKart>();
        }
        cargoPos = targetCargoKart.transform.position;
    }	

    public void FollowCargoKart()
    {
        Vector3 deltaPos = targetCargoKart.transform.position - cargoPos;
        transform.position += deltaPos;
        cargoPos = targetCargoKart.transform.position;
    }

}
