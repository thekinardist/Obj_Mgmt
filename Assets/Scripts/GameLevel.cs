using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : PersistableObject
{
    [SerializeField]
    SpawnZone spawnZone;

    [SerializeField]
    PersistableObject[] persistentObjects;
    public static GameLevel Current { get; private set; }

    void OnEnable(){
        Current = this;
        if(persistentObjects == null){
            persistentObjects = new PersistableObject[0]; 
        }
    }

    // void Start(){
    //     Game.Instance.SpawnZoneOfLevel = spawnZone;
    // }

    // public Vector3 SpawnPoint{
    //     get{
    //         return spawnZone.SpawnPoint;
    //     }
    // }
    public void ConfigureSpawn(Shape shape){
        spawnZone.ConfigureSpawn(shape);
    }

    public override void Save(GameDataWriter writer){
        writer.Write(persistentObjects.Length);
        for(int i = 0; i < persistentObjects.Length; i++){
            persistentObjects[i].Save(writer); 
        }

    }
    public override void Load(GameDataReader reader){
        int saveCount = (int)reader.ReadInt(); 
        for(int i = 0; i < saveCount; i++){
            persistentObjects[i].Load(reader); 
        }
    }
}
