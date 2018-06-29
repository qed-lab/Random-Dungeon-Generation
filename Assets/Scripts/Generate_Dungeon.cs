using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Dungeon : MonoBehaviour {
	//reference vars
	public GameObject ref_roomprefab;
	//immutable vars
	public int num_rooms;
	public int rowsandcolumns;
	public GameObject [] rooms;
	// Use this for initialization
	void Start () {
		//initialization
		rooms = new GameObject[num_rooms];
		//generate n amount of rooms
		rooms_generate();
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
			GameObject room = Instantiate (ref_roomprefab, this.transform.position, this.transform.rotation);
			rooms [i] = room;
			room.name = "Room " + (i + 1).ToString();    
			       
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
