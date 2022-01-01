// using System.Collections;
// using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject
{   
      public PersistableObject prefab;
    public PersistentStorage storage;
    //create slot for prefab
   
     //create keycodes    
     List<PersistableObject> objects;//get list of objects before they're created

     public KeyCode createKey = KeyCode.C;
     public KeyCode newGameKey = KeyCode.N; 
     public KeyCode saveKey = KeyCode.S; 
     public KeyCode loadKey = KeyCode.L;
     //create keycodes
     public float unitsphere;

   
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(createKey)){
            //Instantiate(prefab);
            CreateObject();
        }else if(Input.GetKeyDown(newGameKey)){
            BeginNewGame();
        }else if(Input.GetKeyDown(saveKey)){
            // Save();
            storage.Save(this);
        }else if(Input.GetKeyDown(loadKey)){
            // Load();
            BeginNewGame();
            storage.Load(this);
        }
        
    }
    string savePath; 
    void Awake(){
        objects = new List<PersistableObject>();//create a new list on program awake
        // savePath = Application.persistentDataPath;//name 
    }

    public override void Save(GameDataWriter writer){
        writer.Write(objects.Count);
        for(int i = 0; i < objects.Count; i++){
            objects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader){
        var count = reader.ReadInt();
        for(int i = 0; i < count; i++){
            PersistableObject o = Instantiate(prefab);
            o.Load(reader); 
            objects.Add(o);
        }
    }

        void BeginNewGame(){
        for(int i = 0; i < objects.Count; i++){
            Destroy(objects[i].gameObject);
        }
        objects.Clear();
    }
    void CreateObject(){
        PersistableObject o = Instantiate(prefab);
        Transform t = o.transform;
        t.localPosition = Random.insideUnitSphere * unitsphere;//place prefab in random point inside sphere
        t.localRotation = Random.rotation; //randomize rotation
        t.localScale = Vector3.one * Random.Range(0.1f,0.5f);//random scale
        objects.Add(o);
    }
    
    //create save path and file
    // void Save(){
    //     using( 
    //         var writer = 
    //  new BinaryWriter(File.Open(savePath, FileMode.Create))
    //     ){
    //         writer.Write(objects.Count);//Save the number of objects in the scene
    //         for(int i = 0; i < objects.Count; i++){
    //             //--save the positions of the objects--//
    //             Transform t = objects[i];
    //             writer.Write(t.localPosition.x);
    //             writer.Write(t.localPosition.y);
    //             writer.Write(t.localPosition.z);
    //             //--save the position of the objects--//
    //         }
    //     }
     
    // }

    // void Load(){
    //     BeginNewGame();
    //     using(
    //         var reader = new BinaryReader(File.Open(savePath,FileMode.Open))
    //     ){
    //         int count = reader.ReadInt32();
    //         Vector3 p;
    //         p.x = reader.ReadSingle();
    //         p.y = reader.ReadSingle();
    //         p.z = reader.ReadSingle();
    //         Transform t = Instantiate(prefab);
    //         t.localPosition = p;
    //         objects.Add(t);
    //     }
    // }

}
//-----Data Writing---//
 

