using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class just fake animation of moveable area.
/// </summary>
/// 
public class BlinkingArea : MonoBehaviour {
    private int i = 0;
    public Color matColor = Color.yellow;


	// Use this for initialization
	void Start () {
        InvokeRepeating("Twinkle", 0.0f, 1.0f); // do this forever till object is destroyed.
    }

    void Twinkle() {
        if (i == 0) {
            gameObject.GetComponent<Renderer>().material.color = matColor; // if i is 0 then i am blue
            i++;
        }else {
            gameObject.GetComponent<Renderer>().material.color = Color.white; // else i am whit and turn i back to 0.
            i = 0;
        }


    }
}
