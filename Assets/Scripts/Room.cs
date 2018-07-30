using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //ref vars
    public int size;
    public int mod;
    public GameObject ref_cellprefab;
    public int minX, minZ;
    public CoordinatePair[] dirs = {

        new CoordinatePair(1, 0),
        new CoordinatePair(-1, 0),
        new CoordinatePair(0, 1),
        new CoordinatePair(0, -1)

    };

    //other vars    
    public int currentIndex;
    public CoordinatePair cloneAt;

    //Use this for initialization
    public GameObject[,] CellsInRoom = new GameObject[10, 10];
    public List<GameObject> activeCellsInRoom = new List<GameObject>(); //abbreviated for active cells in room


    void Start()
    {
        transform.position = new Vector3(minX, 0, minZ);
        GenerateSeed();

        print(cloneAt.x + " , " + cloneAt.z);

        StartCoroutine(randomWalk());
    }

    // Update is called once per frame
    void Update()
    {

    }
    //generates the seed to start the room
    void GenerateSeed()
    {
        //generating random coordinates to place seed
        cloneAt = new CoordinatePair(Random.Range(minX, minX + 9), Random.Range(minZ, minZ + 9)); // sets up pointer to where to clone curnt cell
        //uses generatecell function
        activeCellsInRoom.Add(GenerateCell());
    }
    private GameObject GenerateCell()
    {

        //references 
        GameObject ref_newcell = Instantiate(ref_cellprefab);
        Ground_Cell groundCellScript = ref_newcell.GetComponent<Ground_Cell>();

        //putting reference inside the list
        CellsInRoom[cloneAt.x - minX, cloneAt.z - minZ] = ref_newcell;

        //initializing the object instance
        ref_newcell.name = "Cell at gridposition " + cloneAt.x + "," + cloneAt.z;
        ref_newcell.transform.position = new Vector3((float)cloneAt.x + 0.5f, 0f, (float)cloneAt.z + 0.5f);
        ref_newcell.transform.parent = transform;
        groundCellScript.coords = cloneAt;
        return ref_newcell;
    }

    void GenerateStep()
    {
        currentIndex = activeCellsInRoom.Count - 1;
        mod = Random.Range(0, 4);
        cloneAt += dirs[mod];

        cloneAt.x = Mathf.Abs(cloneAt.x); //prevents runtime error 
        cloneAt.z = Mathf.Abs(cloneAt.z);

        if (verifyBounds(cloneAt))
        {
            activeCellsInRoom.Add(GenerateCell());

        }
    }

    // Change signature to accept inputs (float x, float z)
    bool verifyBounds(CoordinatePair cur_cellPos)
    {
        if (CellsInRoom[cur_cellPos.x - minX, cur_cellPos.z - minZ] == null && cur_cellPos.x < minX + 10 && cur_cellPos.x > minX - 1 && cur_cellPos.z < minZ + 10 && cur_cellPos.z > minZ - 1)
        {
            return true;

        }
        else
            return false;

        // Everything below this line is "dead code":

        activeCellsInRoom.RemoveAt(currentIndex);
        currentIndex = activeCellsInRoom.Count - 1;
        cur_cellPos = activeCellsInRoom[currentIndex].GetComponent<Ground_Cell>().coords;
    }

    IEnumerator randomWalk()
    {
        for (int i = 0; i < 100; i++)
        {
            GenerateStep();
            yield return new WaitForSeconds(0.01f);

        }

    }
}

