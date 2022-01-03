using UnityEngine; 

[CreateAssetMenu] //Adds entry for it to Unity's Menu allowing you to create it using the dropdown
public class ShapeFactory : ScriptableObject{
    
    [SerializeField]
    Material[] materials;

    [SerializeField]// makes it so that the array will be visible in the inspector
    Shape[] prefabs; //creates an array in which to put objects
    public Shape Get(int shapeId = 0, int materialId = 0){
        //return Instantiate(prefabs[shapeId]);//requests specific shape from array
        Shape instance = Instantiate(prefabs[shapeId]);
        instance.ShapeId = shapeId;
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandom(){
        return Get(
        Random.Range(0,prefabs.Length),
        Random.Range(0,materials.Length));
    }

}