// using System.Collections;
// using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

[DisallowMultipleComponent]

public class Game : PersistableObject
{   
    // public ShapeFactory shapeFactory;
    [SerializeField] ShapeFactory shapeFactory;
    [SerializeField] bool reseedOnLoad;//creates the option to reseed the random range chosen at the beginning
    [SerializeField] Slider creationSpeedSlider;
    [SerializeField] Slider destructionSpeedSlider;

    // public PersistableObject prefab;
    public PersistentStorage storage;
    
    Random.State mainRandomState;
    //create slot for prefab
   
     //create keycodes    
     
     public KeyCode createKey = KeyCode.C;
     public KeyCode newGameKey = KeyCode.N; 
     public KeyCode saveKey = KeyCode.S; 
     public KeyCode loadKey = KeyCode.L;
     //create keycodes
     public KeyCode destroyKey = KeyCode.X; 
     const int saveVersion = 3;
     //public float unitsphere;
     public float CreationSpeed { get; set; }
     public float DestructionSpeed { get; set; }
     public int levelCount;

    //  public SpawnZone spawnZone;
    //  public SpawnZone SpawnZoneOfLevel{ get; set; }
     public static Game Instance { get; private set; }

     float creationProgress, destructionProgress;
     int loadedLevelBuildIndex;  
   
    // void Awake(){}


    void Start(){

        mainRandomState = Random.state;

        // Instance = this; 
        shapes = new List<Shape>();//create a new list on program awake
        // savePath = Application.persistentDataPath;//name
        // LoadLevel();
        if(Application.isEditor){
        // Scene loadedLevel = SceneManager.GetSceneByName("Level_1");
        // if(loadedLevel.isLoaded){
        //     SceneManager.SetActiveScene(loadedLevel);
        //     return;
        //     }
        for(int i = 0; i < SceneManager.sceneCount; i++){
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if(loadedScene.name.Contains("Level")){
                SceneManager.SetActiveScene(loadedScene);
                loadedLevelBuildIndex = loadedScene.buildIndex;
                return;
                }
            }
        }
        BeginNewGame();
        StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int levelBuildIndex){
        // SceneManager.LoadScene("Level_1", LoadSceneMode.Additive);
        // yield return null;
        enabled = false;
        if(loadedLevelBuildIndex > 0){
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
        }
        yield return SceneManager.LoadSceneAsync(
            levelBuildIndex, LoadSceneMode.Additive
        );//loads scene asyncronously preventing the game fSrom freezing while loading
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        loadedLevelBuildIndex = levelBuildIndex;
        enabled = true;
    }//this is prohibiting user input while loading is happening; Normally there would be a loading screen

    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(createKey)){
            //Instantiate(prefab);
            CreateShape();
        }else if(Input.GetKeyDown(newGameKey)){
            BeginNewGame();
            StartCoroutine(LoadLevel(loadedLevelBuildIndex));
            //start game over
        }else if(Input.GetKeyDown(saveKey)){
            // Save();
            storage.Save(this, saveVersion);
        }else if(Input.GetKeyDown(loadKey)){
            // Load();
            BeginNewGame();
            storage.Load(this);
        } else{
            for(int i = 1; i <= levelCount; i++){
                if(Input.GetKeyDown(KeyCode.Alpha0 + i)){
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }           
            }
        }
    }//allows for loading level using alpa numeric keypad
        void  FixedUpdate(){
        creationProgress += Time.deltaTime * CreationSpeed;
        while(creationProgress >= 1f){
            creationProgress -= 1f;
            CreateShape();
        }
        destructionProgress += Time.deltaTime * DestructionSpeed;
        while(destructionProgress >= 1f){
            destructionProgress -= 1f;
            DestroyShape();
        }
    }
    
    
    


    string savePath; 

    List<Shape> shapes;//get list of objects before they're created

    public override void Save(GameDataWriter writer){
        // writer.Write(-saveVersion);
        writer.Write(shapes.Count);
        writer.Write(Random.state);
        writer.Write(CreationSpeed); 
        writer.Write(creationProgress);
        writer.Write(DestructionSpeed);  
        writer.Write(destructionProgress);
        writer.Write(loadedLevelBuildIndex);
        GameLevel.Current.Save(writer); 
        for(int i = 0; i < shapes.Count; i++){
            writer.Write(shapes[i].ShapeId);
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }

    public override void Load (GameDataReader reader) {
		int version = reader.Version;
		if (version > saveVersion) {
			Debug.LogError("Unsupported future save version " + version);
			return;
		}
		StartCoroutine(LoadGame(reader));
	}

	IEnumerator LoadGame (GameDataReader reader) {
		int version = reader.Version;
		int count = (int)version <= 0 ? (int)-version : (int)reader.ReadInt();

		if (version >= 3) {
			Random.State state = reader.ReadRandomState();
			if (!reseedOnLoad) {
				Random.state = state;
			}
            creationSpeedSlider.value = CreationSpeed = reader.ReadFloat();
            creationProgress = reader.ReadFloat();
            destructionSpeedSlider.value = DestructionSpeed = reader.ReadFloat();  
            destructionProgress = reader.ReadFloat(); 
		}

                // StartCoroutine(LoadLevel((int)version < 2 ? 1 : (int)reader.ReadInt()));
        yield return LoadLevel((int)version < 2 ? 1 : (int)reader.ReadInt());
        if(version >= 3){
            GameLevel.Current.Load(reader);
        }
		for (int i = 0; i < count; i++) {
			int shapeId = (int)version > 0 ? (int)reader.ReadInt() : 0;
			int materialId = (int)version > 0 ? (int)reader.ReadInt() : 0;
			Shape instance = shapeFactory.Get(shapeId, materialId);
			instance.Load(reader);
			shapes.Add(instance);
		}
	}
    // public override void Load(GameDataReader reader){
    //     int version = reader.Version;
    //     int count = (int)version <= 0 ? (int)-version : (int)reader.ReadInt();
    //     if(version >= 3){
    //         Random.State state = reader.ReadRandomState();
    //         if(!reseedOnLoad){
    //             Random.state = state;
    //         }
    //     }
    //     StartCoroutine(LoadLevel((int)version < 2 ? 1 : (int)reader.ReadInt()));
    //     if(version > saveVersion){
    //         Debug.LogError("Unsupported future save version" + version);
    //         return;
    //     }
    //     for(int i = 0; i < count; i++){
    //         // PersistableObject o = Instantiate(prefab);
    //         int shapeId = (int)version > 0 ? (int)reader.ReadInt() : 0;
    //         int materialId = (int)version > 0 ? (int)reader.ReadInt() : 0;
    //         Shape instance = shapeFactory.Get(shapeId, materialId);
    //         instance.Load(reader); 
    //         shapes.Add(instance);
    //     }
    // }

    void BeginNewGame(){
        Random.state = mainRandomState; 
        int seed = Random.Range(0,int.MaxValue) ^ (int)Time.unscaledTime; 
        mainRandomState = Random.state;
        Random.InitState(seed);

    
        creationSpeedSlider.value = CreationSpeed = 0;
        destructionSpeedSlider.value = DestructionSpeed = 0;;

        for(int i = 0; i < shapes.Count; i++){
            Destroy(shapes[i].gameObject);
        }
        shapes.Clear();
    }

    void DestroyShape(){
        if(shapes.Count> 0){
        int index = Random.Range(0, shapes.Count);
        // Destroy(shapes[index].gameObject);
        shapeFactory.Reclaim(shapes[index]);
        int lastIndex = shapes.Count - 1;
        shapes[index] = shapes[lastIndex];
        shapes.RemoveAt(lastIndex);
        }
    }
    void CreateShape(){
        // PersistableObject o = Instantiate(prefab);
        Shape instance = shapeFactory.GetRandom();//pulls random shape from shapefactory
        Transform t = instance.transform;
        // t.localPosition = Random.insideUnitSphere * unitsphere;//place prefab in random point inside sphere
        t.localPosition = GameLevel.Current.SpawnPoint;
        t.localRotation = Random.rotation; //randomize rotation
        t.localScale = Vector3.one * Random.Range(0.5f,1.0f);//random scale
        instance.SetColor(Random.ColorHSV(0f,1f,0.5f,1f,0.25f,1f,1f,1f)); // ColorHSV parameters are as follows(hueMin, hueMax,saturationMin,saturationMax,valueMin, valueMax,alphaMin, alphaMax)
        shapes.Add(instance);
    }

    // void OnEnable (){
    //     Instance = this;
    // }
    
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
 

