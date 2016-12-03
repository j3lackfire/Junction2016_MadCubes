public class GameConstant {
    //CAMERA
    public static float cameraMoveSpeed = 8f;
    public static float cameraRotateAngle = 45f;

    public static float idleCheckFrequency = 0.25f;
    public static float runningReachingDistance = 0.5f;

    public static int objectChargeCountdownValue = 10;
}

//That's it's for now.
public enum ObjectType
{
    Invalid, //not defined or base class.
    CargoKart,
    Fire_Creep,
    Fire_Hero,
    Water_Creep,
    Water_Hero
}
