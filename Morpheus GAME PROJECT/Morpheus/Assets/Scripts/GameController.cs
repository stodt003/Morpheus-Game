using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {

    public static GameController instance = null;
    public gameState currentGameState;
    public Text currentTurnStatusText;

    public List<GameObject> aiUnits;
    public List<GameObject> playerUnits; 

    //private GameObject player;
    private UnitMovement firstPlayerUnitScript;
    private bool haveAnimated;

    public enum gameState
    {
        START,
        PLAYERCHOICE,
        AICHOICE,
        COMBAT,
        ANIMATION,
        END,
        WIN,
        LOSE,
        PAUSE

    }


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        // Cache ALL units for later reference in public list data structures. 
        aiUnits = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        playerUnits = GameObject.FindGameObjectsWithTag("Player").ToList();
        
       
        // Set initial State for the game.
        currentGameState = gameState.START;
        currentTurnStatusText.text = currentGameState.ToString();

    }

    // Use this for initialization
    void Start () {
        haveAnimated = false;
	}
	
	// Update is called once per frame
	void Update () {
        MainGameLoop();
	}

    public void MainGameLoop()
    {
         
        switch (currentGameState)
        {

            case gameState.START:
                {
                    haveAnimated = false;

                }

                break;
            case gameState.PLAYERCHOICE:
                {

                }

                break;
            case gameState.AICHOICE:
                {

                }

                break;
            case gameState.COMBAT:
                {

                }


                break;
            case gameState.ANIMATION:
                {
                    if (!haveAnimated)
                    {
                        // Move player selected movements.
                        foreach (GameObject unit in playerUnits)
                        {
                            unit.GetComponent<UnitMovement>().MoveUnit();
                        }

                        haveAnimated = true; 
                    }
                    
                    
                }


                break;
            case gameState.WIN:
                {
                    Debug.Log("You Win!");
                }


                break;
            case gameState.LOSE:
                {
                    Debug.Log("You Lose!");
                }


                break;
            case gameState.PAUSE:
                {
                    Debug.Log("Game Paused!");
                }


                break;
        }
    }
    
    public void NextTurnPhase()
    {
        switch (currentGameState)
        {
            
            case gameState.START:
                currentGameState = gameState.PLAYERCHOICE;

                break;
            case gameState.PLAYERCHOICE:
                currentGameState = gameState.AICHOICE;

                break;
            case gameState.AICHOICE:
                currentGameState = gameState.COMBAT;

                break;
            case gameState.COMBAT:
                currentGameState = gameState.ANIMATION;

                break;
            case gameState.ANIMATION:                      
                currentGameState = gameState.START;

                break;
            case gameState.WIN:
                

                break;
            case gameState.LOSE:
                

                break;
            case gameState.PAUSE:
                

                break;

                
        }
        currentTurnStatusText.text = currentGameState.ToString();
    }
        
    
}
