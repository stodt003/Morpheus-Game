using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
	
	public int mapSizeX = 10;
	public int mapSizeY = 10;
	public float tileSize = 1.0f;

    private TileMapMouse mouseController;

    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] rawTiles;
    
    Node[,] mapGraph;

    void Start()
    {
        tileTypes = new TileType[mapSizeX * mapSizeY];
        mouseController = GetComponent<TileMapMouse>();
       

        // Setup the selectedUnit's variable
        selectedUnit.GetComponent<UnitMovement>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<UnitMovement>().tileY = (int)selectedUnit.transform.position.y;
        selectedUnit.GetComponent<UnitMovement>().map = this;

        BuildMesh();

        GenerateMapData();
        GeneratePathfindingGraph();
        
    }

    void GenerateMapData()
    {
        // Allocate our map tiles
        rawTiles = new int[mapSizeX, mapSizeY];

        int x, y;

        // Initialize our map tiles to be grass
        for (x = 0; x < mapSizeX; x++)
        {
            for (y = 0; y < mapSizeX; y++)
            {
                tileTypes[x + y] = new TileType("Grass", true, 1);
                rawTiles[x, y] = 0;
            }
        }
        

    }

    void GeneratePathfindingGraph()
    {
        // Initialize the array
        mapGraph = new Node[mapSizeX, mapSizeY];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                mapGraph[x, y] = new Node();
                mapGraph[x, y].x = x;
                mapGraph[x, y].y = y;
            }
        }

        // Now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {

                // Try Left
                if (x > 0)
                {
                    mapGraph[x, y].neighbours.Add(mapGraph[x - 1, y]);
                    if (y > 0)
                        mapGraph[x, y].neighbours.Add(mapGraph[x - 1, y - 1]);
                    if (y < mapSizeY - 1)
                        mapGraph[x, y].neighbours.Add(mapGraph[x - 1, y + 1]);
                }

                // Try Right
                if (x < mapSizeX - 1)
                {
                    mapGraph[x, y].neighbours.Add(mapGraph[x + 1, y]);
                    if (y > 0)
                        mapGraph[x, y].neighbours.Add(mapGraph[x + 1, y - 1]);
                    if (y < mapSizeY - 1)
                        mapGraph[x, y].neighbours.Add(mapGraph[x + 1, y + 1]);
                }

                // Try straight up and down
                if (y > 0)
                    mapGraph[x, y].neighbours.Add(mapGraph[x, y - 1]);
                if (y < mapSizeY - 1)
                    mapGraph[x, y].neighbours.Add(mapGraph[x, y + 1]);

                
            }
        }
    }

      

    public bool UnitCanEnterTile(int x, int y)
    {

        // We could test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.

        return tileTypes[rawTiles[x, y]].isWalkable;
    }



    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        TileType currentTileType = tileTypes[rawTiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = currentTileType.movementCost;

        if (sourceX != targetX && sourceY != targetY)
        {
            // We are moving diagonally!  Fudge the cost for tie-breaking
            // Purely a cosmetic thing!
            cost += 0.001f;
        }

        return cost;

    }

    public Vector3 TileCoordToWorldCoord(int x, int z)
    {
        return new Vector3(x, 0, z);
    }








    public void GeneratePathTo(int x, int y)
    {
        // Clear out our unit's old path.
        selectedUnit.GetComponent<UnitMovement>().currentPath = null;

        if (UnitCanEnterTile(x, y) == false)
        {
            // We probably clicked on a mountain or something, so just quit out.
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = mapGraph[
                            selectedUnit.GetComponent<UnitMovement>().tileX,
                            selectedUnit.GetComponent<UnitMovement>().tileY
                            ];

        Node target = mapGraph[
                            x,
                            y
                            ];

        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in mapGraph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;  // Exit the while loop!
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // If we get there, the either we found the shortest route
        // to our target, or there is no route at ALL to our target.

        if (prev[target] == null)
        {
            // No route between our target and the source
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        // Step through the "prev" chain and add it to our path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // Right now, currentPath describes a route from out target to our source
        // So we need to invert it!

        currentPath.Reverse();

        selectedUnit.GetComponent<UnitMovement>().currentPath = currentPath;
    }

    
    // MESH BUILDING STARTS HERE!!!!
    public void BuildMesh() {
		
		int numTiles = mapSizeX * mapSizeY;
		int numTris = numTiles * 2;
		
		int vsize_x = mapSizeX + 1;
		int vsize_z = mapSizeY + 1;
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];

		int x, z;
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / mapSizeX, (float)z / mapSizeY );
			}
		}
		
		
		for(z=0; z < mapSizeY; z++) {
			for(x=0; x < mapSizeX; x++) {
				int squareIndex = z * mapSizeX + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = z * vsize_x + x + 		   1;
			}
		}
		
		
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

 
        // Assign our mesh to our filter/renderer/collider
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		
		
	}

  
}
