using UnityEngine; 

public class Shape : PersistableObject {
    public int MaterialId {get; private set;}
    static int colorPropertyId = Shader.PropertyToID("_Color");//setting an identifier for the color
    static MaterialPropertyBlock sharedPropertyBlock;

    
    Color color;

    MeshRenderer meshRenderer;
    void Awake(){
        meshRenderer = GetComponent<MeshRenderer>();//Grabs the mesh renderer of each shape
    }
    public void SetColor(Color color){
        this.color = color;
        // meshRenderer.material.color = color; 
        // GetComponent<MeshRenderer>().material.color = color;


        //---Prevents creation of an entirely new material every time a clone is created--//
        //---Checks to see if the property block exists before using it--//
        // var propertyBlock = new MaterialPropertyBlock();
        
        if(sharedPropertyBlock == null){
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
        //---Prevents creation of an entirely new material every time a clone is created--//


    }
    public void SetMaterial(Material material, int materialId){
        // GetComponent<MeshRenderer>().material = material;
        meshRenderer.material = material;
        MaterialId = materialId; 
    }
    public int ShapeId{
        get{
            return shapeId;
        }
        set{
            if(shapeId == int.MinValue && value != int.MinValue){
            shapeId = value;
        }else{
            Debug.LogError("Not allowed to change shapeId.");
        }
            
        }
    }

            int shapeId = int.MinValue;

         	public override void Save (GameDataWriter writer) {
		base.Save(writer);
		writer.Write(color);
	}

	public override void Load (GameDataReader reader) {
		base.Load(reader);
		SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
	}



 
}