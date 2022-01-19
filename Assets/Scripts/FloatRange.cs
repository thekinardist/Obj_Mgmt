using UnityEngine;

[System.Serializable]
public struct FloatRange{
    public float min, max;
    public float RandomValueInRange{
        get{
            return Random.Range(min,max);
        }
    }
}


//Creates a Serialized Field for use by any scripts/variables
