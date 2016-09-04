using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class UnitMovement : MonoBehaviour {

	// tileX and tileY represent the correct map-tile position
	// for this piece.  Note that this doesn't necessarily mean
	// the world-space coordinates, because our map might be scaled
	// or offset or something of that nature.  Also, during movement
	// animations, we are going to be somewhere in between tiles.
	public int tileX;
	public int tileY;

	public TileMap map;

	// Our pathfinding info.  Null if we have no destination ordered.
	public List<Node> currentPath = null;
    private Animator unitAnimController;
    private LineRenderer unitPathRenderer;
    private GameController gameController;
	// How far this unit can move in one turn. Note that some tiles cost extra.
	public int moveSpeed = 2;
	float remainingMovement;

    void Start()
    {
        unitPathRenderer = GetComponent<LineRenderer>();
        unitPathRenderer.enabled = false;
        map = GameObject.FindGameObjectWithTag("TileMap").GetComponent<TileMap>();
        //Animation Stuff

        unitAnimController = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        remainingMovement = Mathf.FloorToInt(moveSpeed);
        // Correctly set starting position before Lerp takes effect. 

        Vector3 startingPosition = map.TileCoordToWorldCoord(tileX, tileY);
        transform.position = startingPosition = map.TileCoordToWorldCoord(tileX, tileY);
        startingPosition.x = startingPosition.x + (float)(0.5 * transform.localScale.x);

        startingPosition.z = startingPosition.z + (float)(0.5 * transform.localScale.z);
    }

	void Update() {
        
        // Draw our debug line showing the pathfinding!
        // NOTE: This won't appear in the actual game view.
        if (currentPath != null) {
			int currNode = 0;

			while( currNode < currentPath.Count-1 ) {
                unitPathRenderer.enabled = true;

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, 0, -0.5f) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, 0, -0.5f) ;


                unitPathRenderer.SetVertexCount(currentPath.Count);
                // Make sure to offset the line renderer such that the line is centred (assuming 1 unit square size character).
                unitPathRenderer.SetPosition( currNode, start + new Vector3(0.5f,0.3f, 1.5f));
                unitPathRenderer.SetPosition( currNode+1, end + new Vector3(0.5f, 0.3f, 1.5f));

                

				currNode++;
			}
		}

		// Have we moved our visible piece close enough to the target tile that we can
		// advance to the next step in our pathfinding?
		if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord( tileX, tileY )) < 0.1f)
        {
            
            AdvancePathing();
         
        }
			

        // Smoothly animate towards the correct map tile.
        Vector3 destination = map.TileCoordToWorldCoord(tileX, tileY);
        destination.x = destination.x + (float)(0.5 * transform.localScale.x);
        
        destination.z = destination.z + (float)(0.5 * transform.localScale.z);



       

        transform.position = Vector3.Lerp(transform.position, destination, 2f * Time.deltaTime);

        // Adjust units rotation to follow path.

        Vector3 lookDirection = (destination - transform.position).normalized;
        if (Vector3.Distance(destination, transform.position) > 1.1f)
        {

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 8f * Time.deltaTime);
        }







    }

    // Advances our pathfinding progress by one tile.
    void AdvancePathing() {
        if (currentPath == null)
        {
            unitPathRenderer.enabled = false;
            return;
        }


        if (remainingMovement <= 0)
        {
            unitPathRenderer.enabled = false;
            return;
        }


        // Teleport us to our correct "current" position, in case we
        // haven't finished the animation yet.



        // Get cost from current tile to next tile
        remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);




        // Move us to the next tile in the sequence
        tileX = currentPath[1].x;
		tileY = currentPath[1].y;



        
        

        // Remove the old "current" tile from the pathfinding list
        currentPath.RemoveAt(0);
		
		if(currentPath.Count == 1) {
			// We only have one tile left in the path, and that tile MUST be our ultimate
			// destination -- and we are standing on it!
			// So let's just clear our pathfinding info.
			currentPath = null;
            unitPathRenderer.enabled = false;
            unitAnimController.SetBool("IsWalking", false);
        }
	}

	// The "Next Turn" button calls this.
	public void MoveUnit() {
        // Start walking Animation
        unitAnimController.SetBool("IsWalking", true);
        
        // Make sure to wrap-up any outstanding movement left over.
        while (currentPath!=null && remainingMovement > 0) {
            
            
			AdvancePathing();
            
        }

		// Reset our available movement points.
		remainingMovement = moveSpeed;

        

        // Reset Path Line Renderer
        unitPathRenderer.enabled = false;
        
    }



}
