using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //ref vars
	public GameObject refCellPrefab;
	public GameObject generator;
	//public GenerateDungeon generatorScript; 
	//room-generation oriented vars
    public int mod;
	public int minX, minZ; //publicly set minimum x and z axis coordinates for random seed and boundary generation
	public List<CoordinatePair> UnreadDirs; //stores directions to check
	public int currentIndex;//current cell to look at in list of active cells(cells which are active have directions available to them)
	public CoordinatePair cloneAt;//where to clone the current cell   
  
    //storage
    public GameObject[,] CellsInRoom = new GameObject[10, 10];
    public List<GameObject> activeCellsInRoom = new List<GameObject>(); //abbreviated for active cells in room


    void Start()
    {
		
		//initialize list
		UnreadDirs = new List<CoordinatePair> ();

		//initialize room position
        transform.position = new Vector3(minX, 0, minZ);

		//generate the starting cell
        GenerateSeed();

        //run the coroutine for full room generation
        StartCoroutine(randomWalk());


    }

    //generates the seed to start the room
    void GenerateSeed()
    {
        //generating random coordinates to place seed
        cloneAt = new CoordinatePair(Random.Range(minX, minX + 9), Random.Range(minZ, minZ + 9)); // sets up pointer to where to clone curnt cell

        //uses generatecell function
        activeCellsInRoom.Add(GenerateCell());
    }

	//generates a cell with relevant information
    private GameObject GenerateCell()
    {

        //references 
        GameObject ref_newcell = Instantiate(refCellPrefab);
        Ground_Cell groundCellScript = ref_newcell.GetComponent<Ground_Cell>();


        //putting reference inside the list
        CellsInRoom[cloneAt.x - minX, cloneAt.z - minZ] = ref_newcell;

        //initializing the object instance
        ref_newcell.name = "Cell at gridposition " + cloneAt.x + "," + cloneAt.z;
        ref_newcell.transform.position = new Vector3((float)cloneAt.x + 0.5f, 0f, (float)cloneAt.z + 0.5f);
        ref_newcell.transform.parent = transform;
        groundCellScript.coords = cloneAt;

		//putting cell in allcells list for wall
		generator.GetComponent<GenerateDungeon>().allCells[cloneAt.x, cloneAt.z] = ref_newcell;
		//print (generator.GetComponent<GenerateDungeon>().allCells [cloneAt.x, cloneAt.z]);
		//returns the object instance
        return ref_newcell;

    }  


	//explores dirs and adds cells
    void GenerateStep()
    {
		//init dirs to check
		UnreadDirs.Clear();
		UnreadDirs.Add( new CoordinatePair(1, 0));//right
		UnreadDirs.Add(	new CoordinatePair(-1, 0));//left
		UnreadDirs.Add(	new CoordinatePair(0, 1));//up
		UnreadDirs.Add(	new CoordinatePair(0, -1));//down

		//sets current index to last item in acir
		currentIndex = activeCellsInRoom.Count - 1;
		if (currentIndex < 0) {
			currentIndex = 0;
		}
		//sets mod to which index of UnreadDirs to look at
		mod = Random.Range(0, 4);

		//modifies cloneat based on mod
        cloneAt += UnreadDirs [mod];  

		//prevents runtime error 
        cloneAt.x = Mathf.Abs(cloneAt.x); 
        cloneAt.z = Mathf.Abs(cloneAt.z);

		//while I still have directions to check
		while (!(UnreadDirs.Count == 0)) 
		{
			//if my direction to check (unreadDirs[mod] is invalid
			if (!VerifyBounds (cloneAt)) {
				
				//undo the direction's modifiers
				cloneAt -= UnreadDirs [mod];

				//remove the direction from the list
				UnreadDirs.RemoveAt (mod); 

				//pick a new random direction from the list of available dirs
				mod = Random.Range (0, UnreadDirs.Count);

				//prevents runtime error (again)
				cloneAt.x = Mathf.Abs(cloneAt.x); 
				cloneAt.z = Mathf.Abs(cloneAt.z);

			} else {
				
				//else if dir is valid then exit loop
				break;
			}
		}

		//if I exited the loop because I found a valid direction
		if (!(UnreadDirs.Count == 0)) {
			//add the cell in that direction
			activeCellsInRoom.Add (GenerateCell ());
			//reset current index to new length (index wise)
			currentIndex = activeCellsInRoom.Count - 1; 
		} else {
			
			//I exited the loop because I have no directions left to check

			//check if I can remove cells
			if (!(activeCellsInRoom.Count == 0)) {
				activeCellsInRoom.RemoveAt(currentIndex);
			}
			//change curnt cell dir to explore
			currentIndex -= 1;  

			//abs? inefficient code
			if (currentIndex < 0) {
				currentIndex = 0;
			}

			//push new coords from curnt index to cloneAt
			if (!(activeCellsInRoom.Count == 0)) {
				cloneAt = activeCellsInRoom [currentIndex].GetComponent<Ground_Cell> ().coords;
			}
		}
    }

    // Change signature to accept inputs (float x, float z)
    bool VerifyBounds(CoordinatePair cur_cellPos)
    {
		//if within range of grid and cell is not occupied
		if (cur_cellPos.x < minX + 10 && cur_cellPos.x > minX - 1 && cur_cellPos.z < minZ + 10 && cur_cellPos.z > minZ - 1)
        {
			if(CellsInRoom[cur_cellPos.x - minX,  cur_cellPos.z - minZ] == null){
            	return true;
				 //valid
			}
			else
				return false;
        }
        else
            return false;
		
        

        
    }

	//coroutine which generates room        
    IEnumerator randomWalk()
    {
        for (int i = 0; i < 100; i++)
        {
            GenerateStep();
            yield return new WaitForSeconds(0.01f);

        }


    }



}

