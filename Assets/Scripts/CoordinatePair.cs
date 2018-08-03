﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct CoordinatePair {
	//defining coordinates
	public int x, z; 

	//constructor
	public CoordinatePair(int x, int z){          
			this.x = x;
			this.z = z;
		}
	//methods

	//addition
	public static CoordinatePair operator + (CoordinatePair  a, CoordinatePair  b) {
		a.x += b.x;
		a.z += b.z;
		return a;
	}

	//subtraction
	public static CoordinatePair operator - (CoordinatePair  a, CoordinatePair  b) {
		a.x -= b.x;
		a.z -= b.z;
		return a;
	}

}
      