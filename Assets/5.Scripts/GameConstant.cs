public class GameConstant {
    //CAMERA
    public static float cameraMoveSpeed = 8f;
    public static float cameraRotateAngle = 45f;

    public static float idleCheckFrequency = 0.25f;
    public static float runningReachingDistance = 0.5f;

    public static int objectChargeCountdownValue = 10;

    //Enemy spawner
    public static int spawnWaterCreepOdds = 8;
    public static float initialSpawnRate = 0.35f;
    public static float increaseSpawnTime = 10f;
    public static float spawnRateIncreaseValue = 0.02f;
    public static float maxSpawnRate = 0.1f;

    public static float normalCreepHealthIncreasePerLevel = 0.35f;
    public static float normalCreepDamageIncreasePerLevel = 0.35f;
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
