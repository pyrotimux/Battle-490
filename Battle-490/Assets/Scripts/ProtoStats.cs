using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class toonDetail
{
    public string toonType; //data, virus or antivirus?
    public int health; //toon HP count
    public int atkpower; //toon attack power
    public int defpower; //toon defense power
}

public class ProtoStats : MonoBehaviour {

    toonDetail toon;

	// Use this for initialization
	void Start () {

        //initializing each toons stats below
        toon = new toonDetail();
        //toon.toonType = "";
        toon.health = 3;
        toon.atkpower = 2;
        toon.defpower = 1;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
