using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	
	TileMap _tileMap;
	
	Vector3 currentTileCoord = new Vector3(-1,-1,-1);
    Vector3 selectionCubeLocation;

	public Transform selectionCube;
    
    private EventSystem _eventSystem;

    void Start() {
		_tileMap = GetComponent<TileMap>();
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;

        if (_eventSystem.IsPointerOverGameObject())
        {
            // Do nothing we clicking a button.
        }
        else
        {
            if (GetComponent<Collider>().Raycast(ray, out hitInfo, 1000))
            {
                selectionCube.GetComponent<Renderer>().enabled = true;

                int x = Mathf.FloorToInt(hitInfo.point.x / _tileMap.tileSize);
                int z = Mathf.FloorToInt(hitInfo.point.z / _tileMap.tileSize);

                if (x <= _tileMap.mapSizeX && x >= 0 && z <= _tileMap.mapSizeY && z >= 0)
                {
                    float scaleX = selectionCube.localScale.x;
                    float scaleZ = selectionCube.localScale.z;

                    currentTileCoord.x = x;
                    currentTileCoord.z = z;

                    selectionCubeLocation.x = x + (0.5f * scaleX);
                    selectionCubeLocation.z = z + (0.5f * scaleZ);



                    selectionCube.transform.position = selectionCubeLocation * _tileMap.tileSize;
                }

                

            }
            else {
                // Hide the selection Cube and set off map tile coords.
                currentTileCoord = new Vector3(-1, 0, -1);
                selectionCube.GetComponent<Renderer>().enabled = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                // MOUSE SELECTION FOR PLAYER
                _tileMap.selectedUnit = SelectUnit();

                if (_tileMap.selectedUnit != null)
                {
                    Debug.Log("Generating Path for Selected Unit to Selected Tile!");
                    if (currentTileCoord.x <= _tileMap.mapSizeX && currentTileCoord.x >= 0 && currentTileCoord.z <= _tileMap.mapSizeY && currentTileCoord.z >= 0)
                    {
                        int tempX = Mathf.FloorToInt(currentTileCoord.x);
                        int tempZ = Mathf.FloorToInt(currentTileCoord.z);
                        _tileMap.GeneratePathTo(tempX, tempZ);
                    }

                }


            }
        }
    }

    public GameObject SelectUnit()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

        Debug.Log("Selecting Player");
      
        if (hitInfo.transform.tag == "Player")
        {
            Debug.Log("Player Selected Sucessfully");
            return hitInfo.transform.gameObject;
        }
        else
        {
            Debug.Log("Mouse not over player");
            return _tileMap.selectedUnit;
        }


    }        

    public Vector3 GetClickedTileCoords()
    {

        if (currentTileCoord != new Vector3(-1f, -1f, -1f))
        {
            return new Vector3(currentTileCoord.x, 0, currentTileCoord.z);
        }
        else return 
                new Vector3(-1,0,-1);
    }
}
