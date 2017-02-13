public class GameConstant
{
    //CAMERA
    public static float cameraMoveSpeed = 8f;
    public static float cameraRotateAngle = 45f;

    public static float idleCheckFrequency = 0.25f;
    public static float runningReachingDistance = 0.75f;

    public static int objectChargeCountdownValue = 10;

    //Enemy spawner
    public static int spawnWaterCreepOdds = 6;
    public static float initialSpawnRate = 0.45f;
    public static float increaseSpawnTime = 20;
    public static float spawnRateIncreaseValue = 0.02f;
    public static float maxSpawnRate = 0.25f;

    public static float normalCreepHealthIncreasePerLevel = 0.15f;
    public static float normalCreepDamageIncreasePerLevel = 0.15f;

    public static int enemySpawnCap = 100;
    //number of seconds the regen will stop after being attacked
    public static float attackStopRegenTime = 3f;
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
