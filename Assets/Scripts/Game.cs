// using System.Collections;
// using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject
{   
    public ShapeFactory shapeFactory;
    // public PersistableObject prefab;
    public PersistentStorage storage;
    //create slot for prefab
   
     //create keycodes    
     
     public KeyCode createKey = KeyCode.C;
     public KeyCode newGameKey = KeyCode.N; 
     public KeyCode saveKey = KeyCode.S; 
     public KeyCode loadKey = KeyCode.L;
     //create keycodes
     const int saveVersion = 1;
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
            CreateShape();
        }else if(Input.GetKeyDown(newGameKey)){
            BeginNewGame();
            //start game over
        }else if(Input.GetKeyDown(saveKey)){
            // Save();
            storage.Save(this, saveVersion);
        }else if(Input.GetKeyDown(loadKey)){
            // Load();
            BeginNewGame();
            storage.Load(this);
        }
        
    }
    string savePath; 

    List<Shape> shapes;//get list of objects before they're created

    void Awake(){
        
        shapes = new List<Shape>();//create a new list on program awake
        // savePath = Application.persistentDataPath;//name 
    }

    public override void Save(GameDataWriter writer){
        // writer.Write(-saveVersion);
        writer.Write(shapes.Count);
        for(int i = 0; i < shapes.Count; i++){
            writer.Write(shapes[i].ShapeId);
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader){
        int version = reader.Version;
        int count = (int)version <= 0 ? (int)-version : (int)reader.ReadInt();

        if(version > saveVersion){
            Debug.LogError("Unsupported future save version" + version);
            return;
        }
        for(int i = 0; i < count; i++){
            // PersistableObject o = Instantiate(prefab);
            int shapeId = (int)version > 0 ? (int)reader.ReadInt() : 0;
            int materialId = (int)version > 0 ? (int)reader.ReadInt() : 0;
            Shape instance = shapeFactory.Get(shapeId, materialId);
            instance.Load(reader); 
            shapes.Add(instance);
        }
    }

        void BeginNewGame(){
        for(int i = 0; i < shapes.Count; i++){
            Destroy(shapes[i].gameObject);
        }
        shapes.Clear();
    }
    void CreateShape(){
        // PersistableObject o = Instantiate(prefab);
        Shape instance = shapeFactory.GetRandom();//pulls random shape from shapefactory
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * unitsphere;//place prefab in random point inside sphere
        t.localRotation = Random.rotation; //randomize rotation
        t.localScale = Vector3.one * Random.Range(0.5f,1.0f);//random scale
        instance.SetColor(Random.ColorHSV(0f,1f,0.5f,1f,0.25f,1f,1f,1f)); // ColorHSV parameters are as follows(hueMin, hueMax,saturationMin,saturationMax,valueMin, valueMax,alphaMin, alphaMax)
        shapes.Add(instance);
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
 

