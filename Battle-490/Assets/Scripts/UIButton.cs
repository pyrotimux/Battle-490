using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

    //all the buttons on the UI
    GameObject buttonAll, buttonMove, buttonAttack, buttonDone, buttonEndTurn;

    //initialize all button
    public void InitButton()
    {
        buttonAll = GameObject.Find("UIButtonCtrl");
        buttonMove = buttonAll.transform.GetChild(1).gameObject;
        buttonAttack = buttonAll.transform.GetChild(2).gameObject;
        buttonDone = buttonAll.transform.GetChild(3).gameObject;
        buttonEndTurn = buttonAll.transform.GetChild(4).gameObject;
    }

    public void End_Turn_Button_Down()
    {
        Input.GetKeyDown("e");
        Debug.Log("End Turn pressed!");
        
    }

    /*
	// Use this for initialization
	void Start () {
		
	}
	*/
     /*
	// Update is called once per frame
	void Update () {
		
	}
    */
    
}
