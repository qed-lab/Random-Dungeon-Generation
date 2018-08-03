using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDungeon : MonoBehaviour
{
    //reference vars
    public GameObject refRoomPrefab;

    //room-oriented variables
    public int numRooms = 100;
    public int rowsAndColumns;
    public GameObject[] rooms;
    int spawnAtX = 0, spawnAtZ = 0;

    // Use this for initialization
    void Start()
    {
        //initialization
        rooms = new GameObject[numRooms];
        transform.position = new Vector3(spawnAtX, 0, spawnAtZ);

        //generate n amount of rooms
        RoomsGenerate(numRooms);
        rowsAndColumns = numRooms;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RoomsRegenerate();
        }
    }

	//generate the rooms
    void RoomsGenerate(int number) 
    {
        for (int i = 0; i < number; i++)
        {
            //instantiate room prefab
            GameObject room = Instantiate(refRoomPrefab, transform.position, transform.rotation);
            Room room_refScript = room.GetComponent<Room>(); //get class ref

			//setting minimum random position to spawn seed for x and z axis
            room_refScript.minX = spawnAtX;
            room_refScript.minZ = spawnAtZ;

			//list of rooms adding
            rooms[i] = room;
            room.name = "Room " + (i + 1).ToString();

			//keep adding minimum distance to spawn
            spawnAtX += 10;

			//start a new row
            if (spawnAtX == rowsAndColumns)
            {
                spawnAtX = 0;
                spawnAtZ += 10;

            }

        }
    }

	//destroy the rooms
    void RoomsDestroy()
    {
        for (int i = 0; i < rooms.Length; i++)
        {

            Destroy(rooms[i].gameObject);
            rooms[i] = null;
        }
    }

	//regenerate the rooms
    void RoomsRegenerate()
    {
        RoomsDestroy();
        RoomsGenerate(numRooms);
    }




}
