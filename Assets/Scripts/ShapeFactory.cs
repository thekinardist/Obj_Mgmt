using UnityEngine; 
[CreateAssetMenu] //Adds entry for it to Unity's Menu allowing you to create it using the dropdown
public class ShapeFactory : ScriptableObject{
    [SerializeField]// makes it so that the array will be visible in the inspector
    Shape[] prefabs; //creates an array in which to put objects
    public Shape Get(int shapeId){
        return Instantiate(prefabs[shapeId]);//requests specific shape from array
    }

    public Shape GetRandom(){
        return Get(Random.Range(0,prefabs.Length));
    }

}