using UnityEngine;



public abstract class SpawnZone : PersistableObject{
    public abstract Vector3 SpawnPoint{ get; }
    // public enum SpawnMovementDirection{
    //     Forward,
    //     Upward,
    //     Outward,
    //     Random
    // }

    // [SerializeField]
    // SpawnMovementDirection spawnMovementDirection;

    // // [SerializeField] float spawnSpeedMin, spawnSpeedMax;
    // [SerializeField]
    // FloatRange spawnSpeed;
    // [SerializeField] float angluarVelMin, angularVelMax;
    [System.Serializable]
    public struct SpawnConfiguration{
        public enum MovementDirection{
            Forward,
            Upward,
            Outward,
            Random
        }
        public MovementDirection movementDirection;
        public FloatRange speed; 
        public FloatRange angularSpeed;
        public FloatRange scale;
        public ColorRangeHSV color;

    }

    [SerializeField]
    SpawnConfiguration spawnConfig;

    public virtual void ConfigureSpawn(Shape shape){
        Transform t = shape.transform;
        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation; //randomize rotation
        t.localScale = Vector3.one * spawnConfig.scale.RandomValueInRange;//random scale
        // shape.SetColor(Random.ColorHSV(0f,1f,0.5f,1f,0.25f,1f,1f,1f)); // ColorHSV parameters are as follows(hueMin, hueMax,saturationMin,saturationMax,valueMin, valueMax,alphaMin, alphaMax)
        shape.SetColor(spawnConfig.color.RandomInRange);
        shape.AngularVelocity = Random.onUnitSphere * spawnConfig.angularSpeed.RandomValueInRange;
        Vector3 direction;
        switch(spawnConfig.movementDirection){
            case SpawnConfiguration.MovementDirection.Upward:
                direction = transform.up;
                break;
            case SpawnConfiguration.MovementDirection.Outward:
                direction = (t.localPosition - transform.position).normalized;
                break;
            case SpawnConfiguration.MovementDirection.Random:
                direction = Random.onUnitSphere;
                break;
            default:
                direction = transform.forward;
                break;      
        } 
 
        shape.Velocity = direction * spawnConfig.speed.RandomValueInRange;

    }

}