using UnityEngine;



public abstract class SpawnZone : PersistableObject{
    public abstract Vector3 SpawnPoint{ get; }
    public enum SpawnMovementDirection{
        Forward,
        Upward,
        Outward,
        Random
    }

    [SerializeField]
    SpawnMovementDirection spawnMovementDirection;

    // [SerializeField] float spawnSpeedMin, spawnSpeedMax;
    [SerializeField]
    FloatRange spawnSpeed;
    [SerializeField] float angluarVelMin, angularVelMax;


    public virtual void ConfigureSpawn(Shape shape){
        Transform t = shape.transform;
        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation; //randomize rotation
        t.localScale = Vector3.one * Random.Range(0.5f,1.0f);//random scale
        shape.SetColor(Random.ColorHSV(0f,1f,0.5f,1f,0.25f,1f,1f,1f)); // ColorHSV parameters are as follows(hueMin, hueMax,saturationMin,saturationMax,valueMin, valueMax,alphaMin, alphaMax)
        shape.AngularVelocity = Random.onUnitSphere * Random.Range(angluarVelMin, angularVelMax);
        Vector3 direction; 
        if(spawnMovementDirection == SpawnMovementDirection.Upward){
            direction = transform.up;
        }else if(spawnMovementDirection == SpawnMovementDirection.Outward){
            direction = (t.localPosition - transform.position).normalized;
        }else if(spawnMovementDirection == SpawnMovementDirection.Random){
            direction = Random.onUnitSphere;
            }else{
            direction = transform.forward;
        }
        shape.Velocity = direction * spawnSpeed.RandomValueInRange;

    }

}