using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Tile : MonoBehaviour {

	public Material materialIdle;
	public Material materialLightup;
	public Material materialUncovered;
	public Material materialDetonated;
	public int ID;
	public int tilesPerRow;

	public Tile tileUpper;
	public Tile tileLower;
	public Tile tileLeft;
	public Tile tileRight;

	public Tile tileUpperLeft;
	public Tile tileUpperRight;
	public Tile tileLowerLeft;
	public Tile tileLowerRight;
		
	public List<Tile> adjacentTiles = new List<Tile>();

	public string state = "idle";

	public TextMesh displayText;
	public GameObject displayFlag;

    public bool isSpawnPit;
    public bool zMoveSelected;

    //Hold players
    public bool player1 = false;
    public bool player2 = false;
    public bool player3 = false;
    public bool player4 = false;


    //Keep track of zombies on a tile
    public int zombies;



	// Use this for initialization
	void Start () {
		//name tiles in heirachy to help with debugging
		gameObject.name = "Tile " + ID.ToString();

		if(inBounds(Grid.tilesAll, ID + tilesPerRow)) 						{ tileUpper = Grid.tilesAll[ID + tilesPerRow]; }
		if(inBounds(Grid.tilesAll, ID - tilesPerRow)) 						{ tileLower = Grid.tilesAll[ID - tilesPerRow]; }
		if(inBounds(Grid.tilesAll, ID + 1) && (ID+1) % tilesPerRow != 0)	{ tileRight = Grid.tilesAll[ID + 1]; }
		if(inBounds(Grid.tilesAll, ID - 1) && ID % tilesPerRow != 0) 		{ tileLeft = Grid.tilesAll[ID - 1]; }

		if(inBounds(Grid.tilesAll, ID + tilesPerRow + 1) && (ID + tilesPerRow + 1) % tilesPerRow != 0) { tileUpperRight = Grid.tilesAll[ID + tilesPerRow + 1]; }
		if(inBounds(Grid.tilesAll, ID + tilesPerRow - 1) &&     ID % tilesPerRow != 0) { tileUpperLeft  = Grid.tilesAll[ID + tilesPerRow - 1]; }
		if(inBounds(Grid.tilesAll, ID - tilesPerRow + 1) && (ID+1) % tilesPerRow != 0) { tileLowerRight = Grid.tilesAll[ID - tilesPerRow + 1]; }
		if(inBounds(Grid.tilesAll, ID - tilesPerRow - 1) &&     ID % tilesPerRow != 0) { tileLowerLeft  = Grid.tilesAll[ID - tilesPerRow - 1]; }

		if(tileUpper)	{ adjacentTiles.Add(tileUpper); }
		if(tileLower)	{ adjacentTiles.Add(tileLower); }
		if(tileLeft)	{ adjacentTiles.Add(tileLeft); }
		if(tileRight)	{ adjacentTiles.Add(tileRight); }

		if(tileUpperLeft)	{ adjacentTiles.Add(tileUpperLeft); }
		if(tileUpperRight)	{ adjacentTiles.Add(tileUpperRight); }
		if(tileLowerLeft)	{ adjacentTiles.Add(tileLowerLeft); }
		if(tileLowerRight)	{ adjacentTiles.Add(tileLowerRight); }

		//countMines ();

		displayText.GetComponent<Renderer>().enabled = false;
		displayFlag.GetComponent<Renderer>().enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (zMoveSelected)
        {
        GetComponent<Renderer>().material = materialLightup;
        }
    }

	private bool inBounds(Tile[] inputArray, int targetID){
		if (targetID < 0 || targetID >= inputArray.Length) {
			return false;
		} else {
			return true;
		}
	}

	public void setFlag(){
		if (state == "idle") {
			state = "flagged";
			displayFlag.GetComponent<Renderer>().enabled = true;


		} else if (state == "flagged") {
			state = "idle";
			displayFlag.GetComponent<Renderer>().enabled = false;
		}
	}


	void OnMouseOver(){
		if(Grid.state == "inGame"){
			if (state == "idle") {
				GetComponent<Renderer>().material = materialLightup;
				if (Input.GetMouseButtonDown (0)) {
                    //uncoverTile();;

                    //Every grid.phase may have different uses for clicking

                    if (Grid.phase == "Setup")
                    {
                        if(isSpawnPit)
                        {
                            zombies += 1;
                            Grid.zombiesToSpawn -= 1;
                            Grid.zombiesOnBoard += 1;
                            if(Grid.zombiesToSpawn == 0)
                            {
                                Grid.phase = "ZDraw";
                            }
                        }
                    }

                    if (Grid.phase == "ZSpawn")
                    {
                        if (isSpawnPit && Grid.zombiesToSpawn != 0)
                        {
                            zombies += 1;
                            Grid.zombiesToSpawn -= 1;
                            Grid.zombiesOnBoard += 1;
                        }
                    }

                    if(Grid.phase == "ZMove")
                    {

                        if (Grid.selectedTile != -1)
                        {
                            Debug.Log("zmove not selected");
                            Debug.Log("Tile to move from is" + Grid.selectedTile.ToString());
                            Debug.Log("this is" + ID.ToString());

                            zombies += 1;
                            Grid.tilesAll[Grid.selectedTile].zombies -= 1;
                            Grid.tilesAll[Grid.selectedTile].zMoveSelected = false;
                            Grid.selectedTile = -1;
                            Grid.zMoveYield = true;
                        }

                            if (Grid.selectedTile == -1 && Grid.zMoveYield !=true && zombies > 0)
                        {
                            Debug.Log("Selecting tile to move from");
                            Debug.Log("Tile ID " + ID.ToString());
                            Grid.selectedTile = ID;
                            zMoveSelected = true;

                        }
                        Grid.zMoveYield = false;                        
                    }

                    if(Grid.phase == "move1")
                    {
                        if(Grid.selectedTile != -1)
                        {
                            Debug.Log("Tile to move from is" + Grid.selectedTile.ToString());
                            Debug.Log("this is" + ID.ToString());
                            player1 = true;
                            Grid.tilesAll[Grid.selectedTile].player1 = false;
                            Grid.tilesAll[Grid.selectedTile].zMoveSelected = false;
                            //Tell Grid p1 to changeGrid.tilesAll[Grid.selectedTile]
                            Grid.p1 = Grid.tilesAll[Grid.selectedTile];
                            Grid.selectedTile = -1;
                            Grid.zMoveYield = true;

                            //Change phase
                            
                        }

                        if(Grid.selectedTile == -1 && Grid.zMoveYield !=true && player1)
                        {
                            Debug.Log("Selecting tile to move from");
                            Debug.Log("Tile ID " + ID.ToString());
                            Grid.selectedTile = ID;
                            zMoveSelected = true;
                        }
                        Grid.zMoveYield = false;
                    }

                    if (Grid.phase == "move2")
                    {
                        if (Grid.selectedTile != -1)
                        {
                            Debug.Log("Tile to move from is" + Grid.selectedTile.ToString());
                            Debug.Log("this is" + ID.ToString());
                            player2 = true;
                            Grid.tilesAll[Grid.selectedTile].player2 = false;
                            Grid.tilesAll[Grid.selectedTile].zMoveSelected = false;
                            //Tell Grid p1 to changeGrid.tilesAll[Grid.selectedTile]
                            Grid.p2 = Grid.tilesAll[Grid.selectedTile];
                            Grid.selectedTile = -1;
                            Grid.zMoveYield = true;

                            //Change phase

                        }

                        if (Grid.selectedTile == -1 && Grid.zMoveYield != true && player2)
                        {
                            Debug.Log("Selecting tile to move from");
                            Debug.Log("Tile ID " + ID.ToString());
                            Grid.selectedTile = ID;
                            zMoveSelected = true;
                        }
                        Grid.zMoveYield = false;
                    }

                    if (Grid.phase == "move3")
                    {
                        if (Grid.selectedTile != -1)
                        {
                            Debug.Log("Tile to move from is" + Grid.selectedTile.ToString());
                            Debug.Log("this is" + ID.ToString());
                            player3 = true;
                            Grid.tilesAll[Grid.selectedTile].player3 = false;
                            Grid.tilesAll[Grid.selectedTile].zMoveSelected = false;
                            //Tell Grid p1 to changeGrid.tilesAll[Grid.selectedTile]
                            Grid.p3 = Grid.tilesAll[Grid.selectedTile];
                            Grid.selectedTile = -1;
                            Grid.zMoveYield = true;

                            //Change phase

                        }

                        if (Grid.selectedTile == -1 && Grid.zMoveYield != true && player3)
                        {
                            Debug.Log("Selecting tile to move from");
                            Debug.Log("Tile ID " + ID.ToString());
                            Grid.selectedTile = ID;
                            zMoveSelected = true;
                        }
                        Grid.zMoveYield = false;
                    }

                    if (Grid.phase == "move4")
                    {
                        if (Grid.selectedTile != -1)
                        {
                            Debug.Log("Tile to move from is" + Grid.selectedTile.ToString());
                            Debug.Log("this is" + ID.ToString());
                            player4 = true;
                            Grid.tilesAll[Grid.selectedTile].player4 = false;
                            Grid.tilesAll[Grid.selectedTile].zMoveSelected = false;
                            //Tell Grid p1 to changeGrid.tilesAll[Grid.selectedTile]
                            Grid.p4 = Grid.tilesAll[Grid.selectedTile];
                            Grid.selectedTile = -1;
                            Grid.zMoveYield = true;

                            //Change phase

                        }

                        if (Grid.selectedTile == -1 && Grid.zMoveYield != true && player4)
                        {
                            Debug.Log("Selecting tile to move from");
                            Debug.Log("Tile ID " + ID.ToString());
                            Grid.selectedTile = ID;
                            zMoveSelected = true;
                        }
                        Grid.zMoveYield = false;
                    }
                }

                if (Input.GetMouseButtonDown (1)) {
					setFlag();
                    Debug.Log("P1: " + Grid.p1.ToString() + "P2: " + Grid.p2.ToString() + "P3: " + Grid.p3.ToString() + "P4: " + Grid.p4.ToString());

                }
			}

			else if (state == "flagged") {
				GetComponent<Renderer>().material = materialLightup;
				if (Input.GetMouseButtonDown (1)) {
					setFlag();
				}
			}
		}
	}

	void OnMouseExit()
    {
		if(Grid.state == "inGame"){
			if (state == "idle" || state == "flagged") {
				GetComponent<Renderer>().material = materialIdle;
			}
		}
	}

	void OnMouseUp(){
		//Clunky Debugger

		StringBuilder sb = new StringBuilder ();
        sb.Append("Z = " + zombies.ToString() + "\n");
        if(isSpawnPit) { sb.Append("Pit \n"); }
        if (player1) { sb.Append("P1    "); }
        if(player2) { sb.Append("P2      "); }
        if(player3) { sb.Append("P3      "); }
        if(player4) { sb.Append("P4      "); }
        if (tileUpperLeft) { sb.Append(tileUpperLeft.ID); } else { sb.Append("-"); }
		sb.Append (",");
		if(tileUpper) { sb.Append(tileUpper.ID); } else { sb.Append("-"); }
		sb.Append (",");
		if(tileUpperRight) { sb.Append(tileUpperRight.ID); } else { sb.Append("-"); }
		sb.Append ("\n");
		if(tileLeft) { sb.Append(tileLeft.ID); } else { sb.Append("-"); }
		sb.Append (",");
		sb.Append(ID);
		sb.Append (",");
		if(tileRight) { sb.Append(tileRight.ID); } else { sb.Append("-"); }
		sb.Append ("\n");
		if(tileLowerLeft) { sb.Append(tileLowerLeft.ID); } else { sb.Append("-"); }
		sb.Append (",");
		if(tileLower) { sb.Append(tileLower.ID); } else { sb.Append("-"); }
		sb.Append (",");
		if(tileLowerRight) { sb.Append(tileLowerRight.ID); } else { sb.Append("-"); }

		print (sb);
	}
}
