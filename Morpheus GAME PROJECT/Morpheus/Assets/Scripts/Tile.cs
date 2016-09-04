using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Tile {

	public int tileX;
	public int tileY;
	public TileMap map;
    public TileType type;

    public Tile(int _tileX, int _tileY, TileMap parentMap, TileType tileType)
    {
        tileX = _tileX;
        tileY = _tileY;
        map = parentMap;
        type = tileType;
    }

    
    //void OnMouseUp() {
	//	Debug.Log ("Click!");

	//	if(EventSystem.current.IsPointerOverGameObject())
	//		return;

	//	map.GeneratePathTo(tileX, tileY);
	//}

}
