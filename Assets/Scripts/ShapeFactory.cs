using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;

[CreateAssetMenu] //Adds entry for it to Unity's Menu allowing you to create it using the dropdown
public class ShapeFactory : ScriptableObject{
    
    [SerializeField]
    bool recycle;

    [SerializeField]
    Material[] materials;

    [SerializeField]// makes it so that the array will be visible in the inspector
    Shape[] prefabs; //creates an array in which to put objects

    List<Shape>[] pools;

    Scene poolScene;

    void CreatePools(){
        pools = new List<Shape>[prefabs.Length];
        for(int i = 0; i < pools.Length; i++){
            pools[i] = new List<Shape>();
        }
        if(Application.isEditor){
        poolScene = SceneManager.GetSceneByName(name);
    
        if(poolScene.isLoaded){
            GameObject[] rootObjects = poolScene.GetRootGameObjects();
            for(int i = 0; i < rootObjects.Length; i++){
                Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                if(!pooledShape.gameObject.activeSelf){
                    pools[pooledShape.ShapeId].Add(pooledShape);
                }//checks whether the shape is active
            }
            return;
         }
        poolScene = SceneManager.CreateScene(name);//creates a scene when we go into play mode
    }
    }

    public Shape Get(int shapeId = 0, int materialId = 0){
        //return Instantiate(prefabs[shapeId]);//requests specific shape from array

        Shape instance; 
        if(recycle){
            if(pools == null){
                CreatePools();
            }
            List<Shape> pool = pools[shapeId];
            int lastIndex = pool.Count - 1;
            if(lastIndex >= 0){
            instance = pool[lastIndex];
            instance.gameObject.SetActive(true);
            pool.RemoveAt(lastIndex);
            }else{
                instance = Instantiate(prefabs[shapeId]);
                instance.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(
                    instance.gameObject, poolScene
                );//moves created objects into the created scene
            }
        }
        else{
            instance = Instantiate(prefabs[shapeId]);
        }
       
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public void Reclaim(Shape shapeToRecycle){
        if(recycle){
            if(pools == null){
                CreatePools();
            }
            pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }else{
            Destroy(shapeToRecycle.gameObject);
        }
    }
    public Shape GetRandom(){
        return Get(
        Random.Range(0,prefabs.Length),
        Random.Range(0,materials.Length));
    }

}