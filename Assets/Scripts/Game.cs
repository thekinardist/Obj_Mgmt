using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    List<Transform> objects;//get list of objects before they're created
    //create slot for prefab
     public Transform prefab;
     //create keycodes
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
            Save();
        }else if(Input.GetKeyDown(loadKey)){
            Load();
        }
        
    }
    string savePath; 
    void Awake(){
        objects = new List<Transform>();//create a new list on program awake
        savePath = Application.persistentDataPath;//name 
    }
    
    //create save path and file
    void Save(){
        using( 
            var writer = 
     new BinaryWriter(File.Open(savePath, FileMode.Create))
        ){
            writer.Write(objects.Count);//Save the number of objects in the scene
            for(int i = 0; i < objects.Count; i++){
                //--save the positions of the objects--//
                Transform t = objects[i];
                writer.Write(t.localPosition.x);
                writer.Write(t.localPosition.y);
                writer.Write(t.localPosition.z);
                //--save the position of the objects--//
            }
        }
     
    }

    void Load(){
        BeginNewGame();
        using(
            var reader = new BinaryReader(File.Open(savePath,FileMode.Open))
        ){
            int count = reader.ReadInt32();
            Vector3 p;
            p.x = reader.ReadSingle();
            p.y = reader.ReadSingle();
            p.z = reader.ReadSingle();
            Transform t = Instantiate(prefab);
            t.localPosition = p;
            objects.Add(t);
        }
    }
    void BeginNewGame(){
        for(int i = 0; i < objects.Count; i++){
            Destroy(objects[i].gameObject);
        }
        objects.Clear();
    }
    void CreateObject(){
        Transform t = Instantiate(prefab);
        t.localPosition = Random.insideUnitSphere * unitsphere;//place prefab in random point inside sphere
        t.localRotation = Random.rotation; //randomize rotation
        t.localScale = Vector3.one * Random.Range(0.1f,0.3f);//random scale
        objects.Add(t);
    }
}
//-----Data Writing---//
 

