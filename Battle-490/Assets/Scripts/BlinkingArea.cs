using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingArea : MonoBehaviour {
    private int i = 0;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Twinkle", 0.0f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Twinkle() {
        if (i == 0) {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            i++;
        }else {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            i = 0;
        }


    }
}
