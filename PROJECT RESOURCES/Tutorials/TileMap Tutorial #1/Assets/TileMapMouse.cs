using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	
	TileMap _tileMap;
	
	Vector3 currentTileCoord;
	
	public Transform selectionCube;
	
	void Start() {
		_tileMap = GetComponent<TileMap>();
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;
		
		if( GetComponent<Collider>().Raycast( ray, out hitInfo, Mathf.Infinity ) ) {
			int x = Mathf.RoundToInt( hitInfo.point.x / _tileMap.tileSize);
			int z = Mathf.RoundToInt( hitInfo.point.z / _tileMap.tileSize);
			//Debug.Log ("Tile: " + x + ", " + z);
			
			currentTileCoord.x = x;
			currentTileCoord.z = z;
			
			selectionCube.transform.position = currentTileCoord*_tileMap.tileSize;
		}
		else {
			// Hide selection cube?
		}
		
		if(Input.GetMouseButtonDown(0)) {
			Debug.Log ("Click!");
		}
	}
}
