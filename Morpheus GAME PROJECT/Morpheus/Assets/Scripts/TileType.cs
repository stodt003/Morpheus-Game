using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

	public string name;
	

	public bool isWalkable = true;
	public float movementCost = 1;

    public TileType(string typeName, bool walkable, float cost)
    {
        name = typeName;
        isWalkable = walkable;
        movementCost = cost;
    }

}
