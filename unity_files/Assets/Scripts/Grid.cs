using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Tile tilePrefab;
    public Deck heroDeck;
    

    public PlayerOne hero1;
    public PlayerOne hero2;
    public PlayerOne hero3;
    public PlayerOne hero4;


    public int numberOfTiles = 10;
	public float distanceBetweenTiles = 1f;
	public int tilesPerRow = 4;
	public int numberOfMines = 5;
    public int sunTracker = 15;

	public static Tile[] tilesAll;
	public List<Tile> spawnPits;
	public static List<Tile> tilesUnmined;

	public static string state;

    public static string phase;

    public static int zombiesToSpawn = 0;
    public static int zombiesOnBoard = 0;

    public static bool zMoveYield = false;//Stop inbetween movements

    public static int selectedTile = -1;//Id of tile
    public bool canSpawn = false;
    private System.Random die = new System.Random();
    private int moves;

    public static Tile p1;
    public static Tile p2;
    public static Tile p3;
    public static Tile p4;

    public int heroesKilled = 0;

	// Use this for initialization
	void Start () {
        
        gameSetup();//Roll init Zombies, spawn them, then go to zombie phase
		CreateTiles ();
		state = "inGame";
	}
	

    void gameSetup()
    {
        phase = "Setup";
        //Roll for Zombie Spawn
        zombiesToSpawn = rollDice(12);
        //Spawn Zombies
        
        //Change to Zombie phase
    }

    int rollDice(int d)
    {
        return die.Next(1, d);
    }


	// Update is called once per frame
	void Update () {

		//Check for Finish Game
	}

	void finishGame()
    {
		state = "gameWon";
	}

	void CreateTiles(){

		tilesAll = new Tile[numberOfTiles];

		float xOffset = 0f;
		float zOffset = 0f;

		for (int tilesCreated = 0; tilesCreated < numberOfTiles; tilesCreated++) {
			xOffset += distanceBetweenTiles;

			if(tilesCreated % tilesPerRow == 0){
				zOffset += distanceBetweenTiles;
				xOffset = 0;
			}

			Tile newTile = (Tile)Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), transform.rotation);
			newTile.ID = tilesCreated;
			newTile.tilesPerRow = tilesPerRow;
            newTile.isSpawnPit = false;

            int s1 = rollDice(144);
            int s2 = rollDice(144);
            int s3 = rollDice(144);
            int s4 = rollDice(144);


            if(tilesCreated == s1 || tilesCreated == s2 || tilesCreated == s3 || tilesCreated == s4)
            {
                newTile.isSpawnPit = true;
                spawnPits.Add(newTile);
            }
            if (tilesCreated == 15)
            {
                newTile.player1 = true;
                p1 = newTile;
            }
            if (tilesCreated == 77)
            {
                newTile.player2 = true;
                p2 = newTile;
            }
            if (tilesCreated == 135)
            {
                newTile.player3 = true;
                p3 = newTile;
            }
            if (tilesCreated == 106)
            {
                newTile.player4 = true;
                p4 = newTile;
            }



            tilesAll[tilesCreated] = newTile;
		}

	}

    void checkZFight()
    {
        Debug.Log("CheckZFight called");
        //Need to add checks for dead players
        foreach (Tile currentTile in tilesAll)
        {
            int result;
            int zombiesToKill = 0;
            if(currentTile.player1)
            {
                for(int i = 0;i<currentTile.zombies;i++)
                {
                    result = fight(hero1);
                    if(result == 1)
                    {
                        zombiesToKill += 1;
                    }
                    else if(result ==-1 )
                    {
                        hero1.health -= 1;
                        //need to check for death at health == 0
                    }
                }
                currentTile.zombies -= zombiesToKill;
                Debug.Log("Combat Results: PlayerOne has " + hero1.health.ToString() + " Health and killed " + zombiesToKill.ToString() + " Zombies");
                zombiesToKill = 0;
            }

            if (currentTile.player2)
            {
                for (int i = 0; i < currentTile.zombies; i++)
                {
                    result = fight(hero2);
                    if (result == 1)
                    {
                        zombiesToKill += 1;
                    }
                    else if (result == -1)
                    {
                        hero2.health -= 1;
                        //need to check for death at health == 0
                    }
                }
                currentTile.zombies -= zombiesToKill;
                Debug.Log("Combat Results: PlayerTwo has " + hero2.health.ToString() + " Health and killed " + zombiesToKill.ToString() + " Zombies");
                zombiesToKill = 0;
            }

            if (currentTile.player3)
            {
                for (int i = 0; i < currentTile.zombies; i++)
                {
                    result = fight(hero3);
                    if (result == 1)
                    {
                        zombiesToKill += 1;
                    }
                    else if (result == -1)
                    {
                        hero3.health -= 1;
                        //need to check for death at health == 0
                    }
                }
                currentTile.zombies -= zombiesToKill;
                Debug.Log("Combat Results: PlayerThree has " + hero3.health.ToString() + " Health and killed " + zombiesToKill.ToString() + " Zombies");
                zombiesToKill = 0;
            }

            if (currentTile.player4)
            {
                for (int i = 0; i < currentTile.zombies; i++)
                {
                    result = fight(hero4);
                    if (result == 1)
                    {
                        zombiesToKill += 1;
                    }
                    else if (result == -1)
                    {
                        hero4.health -= 1;
                        //need to check for death at health == 0
                    }
                }
                currentTile.zombies -= zombiesToKill;
                Debug.Log("Combat Results: PlayerFour has " + hero4.health.ToString() + " Health and killed " + zombiesToKill.ToString() + " Zombies");
                zombiesToKill = 0;
            }
            //currentTile.zMoveSelected = false;
        }
    }

    int fight(PlayerOne hero)
    {
        int roll1 = rollDice(6);
        int roll2 = rollDice(6);
        int roll3 = rollDice(6);

        int max = Mathf.Max(roll1, roll2);

        Debug.Log("Hero rolled: " + roll1.ToString() + " and " + roll2.ToString() + " Max: " + max.ToString() + "    Zombie rolled" + roll3.ToString());
        if(max > roll3 && roll1 == roll2)
        {
            Debug.Log("Zombie should die");
            return 1;//Zombie should die
        }
        else if(max > roll3)
        {
            Debug.Log("Fended Off");
            return 0;//Fended off nothing changes
        }
        else
        {
            Debug.Log("Hero should lose 1 health");
            return -1;//Hero lost and should take wound
        }
    }



	void OnGUI(){
		if(state == "inGame"){
			//UnityEngine.GUI thebox = GUI.Box(new Rect(10,10,200,50), "Current Turn ");
            if(phase == "Setup")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Setup Zombies to Place:  "+ zombiesToSpawn.ToString());
            }
            /*
            Zombie Turn phases
            Suntrack Move - Sun
            Draw Cards -ZDraw
            Roll to Spawn - Rollspawn
            Move Zombies -ZMove
            Fight - ZFight
            Spawn Zombies -ZSpawn
        Hero Turn Phases
            Move / Search - move / search
            Exchange - Exchange
            Ranged Attack -Ranged
            Fight - HFight*/

            if(phase == "ZDraw")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Zombie Draw Phase");
                if (GUI.Button(new Rect(10, 70, 200, 50), "Draw (non-functional)"))
                {
                    //Card drawing logic
                    //Allow 1 discard
                    //Draw 4 cards
                    //Change phase
                    Grid.phase = "RollSpawn";
                }
            }

            if (phase == "RollSpawn")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Roll to Spawn Phase");
                if (GUI.Button(new Rect(10, 70, 200, 50), "Roll to Spawn"))
                {
                    //Roll to see if can spawn
                    if(rollDice(12) > zombiesOnBoard)
                    {
                        canSpawn = true;
                        zombiesToSpawn = rollDice(6);
                    }
                    //Change zombiesToSpawn
                    Grid.phase = "ZMove";
                }
            }

            if (phase == "ZMove")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Move Zombies");
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 70, 200, 50), "End Movement"))
                {
                    //Turn of zMoveselected
                    foreach (Tile currentTile in tilesAll)
                    {
                        currentTile.zMoveSelected = false;
                    }
                        Grid.phase = "ZFight";
                }
            }

            if (phase == "ZFight")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Zombie Fights");
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 70, 200, 50), "End Fights"))
                {
                    //Check for ZFights
                    checkZFight();
                    //Check for Dead Players
                    Grid.phase = "ZSpawn";
                }
            }

            if (phase == "ZSpawn")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Zombie Spawns");
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 70, 200, 50), "To Spawn: " + zombiesToSpawn.ToString() + "\n end spawning"))
                {
                    canSpawn = false;
                    moves = rollDice(6);
                    Grid.phase = "move1";

                }
            }

            if (phase == "move1")
            {
                
                GUI.Box(new Rect(10, 10, 200, 50), "P1: Move " + moves.ToString() + "Spaces or Search");
                //Heroes can move d6 spaces
                if (GUI.Button(new Rect(10, 70, 200, 50), "Search"))
                {
                    //Allow movement or they click search
                    if (heroDeck.cards2.Count > 0)
                    {
                        hero1.hand.Add(heroDeck.cards2[0]);
                        heroDeck.cards2.RemoveAt(0);
                    }
                   // hero1.hand.Add("Cool");
                    
                    moves = rollDice(6);
                    Grid.phase = "move2";
                }
                if (GUI.Button(new Rect(10, 130, 200, 50), "End Move"))
                {
                    //Allow movement or they click search
                    moves = rollDice(6);
                    Grid.phase = "move2";
                }
            }
            if (phase == "move2")
            {
                
                GUI.Box(new Rect(10, 10, 200, 50), "P2: Move " + moves.ToString() + "Spaces or Search");
                //Heroes can move d6 spaces
                if (GUI.Button(new Rect(10, 70, 200, 50), "Search"))
                {
                    //Allow movement or they click search
                    if (heroDeck.cards2.Count > 0)
                    {
                        hero2.hand.Add(heroDeck.cards2[0]);
                        heroDeck.cards2.RemoveAt(0);
                    }
                    // hero1.hand.Add("Cool");

                    moves = rollDice(6);
                    Grid.phase = "move3";
                }
                if (GUI.Button(new Rect(10, 130, 200, 50), "End Move"))
                {
                    //Allow movement or they click search
                    moves = rollDice(6);
                    Grid.phase = "move3";
                }
            }
            if (phase == "move3")
            {
                
                GUI.Box(new Rect(10, 10, 200, 50), "P3: Move " + moves.ToString() + "Spaces or Search");
                //Heroes can move d6 spaces
                if (GUI.Button(new Rect(10, 70, 200, 50), "Search"))
                {
                    //Allow movement or they click search
                    if (heroDeck.cards2.Count > 0)
                    {
                        hero3.hand.Add(heroDeck.cards2[0]);
                        heroDeck.cards2.RemoveAt(0);
                    }
                    // hero1.hand.Add("Cool");

                    //Allow movement or they click search
                    moves = rollDice(6);
                    Grid.phase = "move4";
                }
                if (GUI.Button(new Rect(10, 130, 200, 50), "End Move"))
                {
                    //Allow movement or they click search
                    moves = rollDice(6);
                    Grid.phase = "move4";
                }
            }
            if (phase == "move4")
            {
                
                GUI.Box(new Rect(10, 10, 200, 50), "P4: Move " + moves.ToString() + "Spaces or Search");
                //Heroes can move d6 spaces
                if (GUI.Button(new Rect(10, 70, 200, 50), "Search"))
                {
                    //Allow movement or they click search
                    if (heroDeck.cards2.Count > 0)
                    {
                        hero4.hand.Add(heroDeck.cards2[0]);
                        heroDeck.cards2.RemoveAt(0);
                    }
                    // hero1.hand.Add("Cool");

                    //Allow movement or they click search
                    Grid.phase = "Exchange";

                }
                if (GUI.Button(new Rect(10, 130, 200, 50), "End Move"))
                {
                    //Allow movement or they click search
                    moves = rollDice(6);
                    Grid.phase = "Exchange";
                }
            }

            if (phase == "Exchange")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Exchange (Non-Functional)");
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 70, 200, 50), "End Phase"))
                {
                    //Allow Exchange
                    Grid.phase = "Ranged";
                }
            }

            if (phase == "Ranged")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Ranged Attack (Non-Functional)");
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 70, 200, 50), "End Phase"))
                {
                    //Allow Exchange
                    Grid.phase = "HFight";
                }
            }

            if (phase == "HFight")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "Hero Fights");
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 70, 200, 50), "End Phase"))
                {
                    //Force all fights
                    //Needs to be changed into 4 separate fight phases 
                    checkZFight();
                    //Check for dead players
                    Grid.phase = "Sun";
                }
            }

            if (phase == "Sun")
            {
                //All zombies can move 1 space
                if (GUI.Button(new Rect(10, 10, 200, 50), "Move Suntracker"))
                {
                    sunTracker -= 1;
                    if(sunTracker == 0)
                    {
                        state = "gameOver";
                    }
                    else
                    {
                        phase = "ZDraw";
                    }
                }
            }

        }
        else if(state == "gameOver"){
			GUI.Box(new Rect(10,10,200,50), "Zombies Win!");

			if(GUI.Button(new Rect(10,70,200,50), "Restart")){
				restart();
			}
		}
		else if(state == "gameWon"){
			GUI.Box(new Rect(10,10,200,50), "You win!");

			if(GUI.Button(new Rect(10,70,200,50), "Restart")){
				restart();
			}
		}
	}

	void restart(){
		state = "loading";
		Application.LoadLevel (Application.loadedLevel);
     
	}
}
