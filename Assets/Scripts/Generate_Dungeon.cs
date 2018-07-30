using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Dungeon : MonoBehaviour {
	//reference vars
	public GameObject ref_roomprefab;
	//immutable vars
	public int num_rooms = 100;
	public int rowsandcolumns;
	public GameObject [] rooms;
	int spawnAtX = 0, spawnAtZ = 0;
	// Use this for initialization
	void Start () {
		//initialization
		rooms = new GameObject[num_rooms];
		transform.position = new Vector3 (spawnAtX, 0, spawnAtZ);
		//generate n amount of rooms
		rooms_generate();
		rowsandcolumns = num_rooms;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			rooms_regenerate ();
		}
	}

	void rooms_generate(){
		for (int i = 0; i < num_rooms; i++) {
			//instantiate room prefab
			GameObject room = Instantiate (ref_roomprefab, transform.position, transform.rotation);
			Room room_refScript = room.GetComponent<Room> ();
			room_refScript.minX = spawnAtX;
			room_refScript.minZ = spawnAtZ;
			rooms [i] = room;
			room.name = "Room " + (i + 1).ToString();    

			spawnAtX += 10;

			if (spawnAtX == rowsandcolumns) {
				spawnAtX = 0; 
				spawnAtZ += 10;
			}

		}
	}

	void rooms_destroy(){
		for (int i = 0; i < rooms.Length; i++) {
			
			Destroy (rooms [i].gameObject );
			rooms [i] = null;
		}
	}

	void rooms_regenerate(){
		rooms_destroy ();
		rooms_generate ();
	}




}
