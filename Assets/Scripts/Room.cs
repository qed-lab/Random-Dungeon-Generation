using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	//ref vars
	public int size;
	public int mod;
	public GameObject ref_cellprefab;
	public CoordinatePair[] dirs = {
		
		new CoordinatePair(1, 0), 
		new CoordinatePair(-1, 0), 
		new CoordinatePair(0, 1), 
		new CoordinatePair(0, -1)

	};
	//other vars
	public int cur_index;
	public CoordinatePair cloneAt;
	//Use this for initialization
	public GameObject[,] CellsInRoom = new GameObject[100,100];
	public List<GameObject> acir = new List<GameObject>(); //abbreviated for active cells in room
	void Awake() {

		GenerateSeed ();	

		print (cloneAt.x + " , " + cloneAt.z);

		StartCoroutine (randomWalk());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//generates the seed to start the room
	void GenerateSeed(){
		//generating random coordinates to place seed
		cloneAt = new CoordinatePair (Random.Range (0, 9), Random.Range (0, 9));
		//uses generatecell function
		acir.Add(GenerateCell ());
	}
	private GameObject  GenerateCell(){
		
		//references 
		GameObject  ref_newcell = Instantiate (ref_cellprefab);
		Ground_Cell ref_cellclass = ref_newcell.GetComponent <Ground_Cell> ();
		//putting reference inside the list
		CellsInRoom [cloneAt.x, cloneAt.z] = ref_newcell;
		//initializing the object instance
		ref_newcell.name = "Cell at gridposition " + cloneAt.x + "," + cloneAt.z; 
		ref_newcell.transform.position = new Vector3 ((float)cloneAt.x + 0.5f, 0f, (float)cloneAt.z + 0.5f);
		ref_newcell.transform.parent = transform;
		ref_cellclass.coords = cloneAt;
		return ref_newcell;
	}

	void GenerateStep(){
		
		cur_index = acir.Count - 1;
		mod = Random.Range (0, 4);
		cloneAt += dirs [mod];

		cloneAt.x = Mathf.Abs (cloneAt.x); //prevents runtime error 
		cloneAt.z = Mathf.Abs (cloneAt.z);

		if (verifyBounds ()) {
			acir.Add(GenerateCell ());

		}
	}
	bool verifyBounds(){
		if (CellsInRoom [cloneAt.x, cloneAt.z] == null && cloneAt.x < 10 && cloneAt.x > -1 && cloneAt.z < 10 && cloneAt.z > -1) {
			return true;

		} else
			return false;
			acir.RemoveAt (cur_index);
			cur_index = acir.Count - 1;
			cloneAt = acir [cur_index].GetComponent<Ground_Cell>().coords;
	}
	IEnumerator randomWalk(){
		for (int i = 0; i < 100; i++) {
			GenerateStep ();
			yield return new WaitForSeconds (0.01f);
		
		}

	}
}

           