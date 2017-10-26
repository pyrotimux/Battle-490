using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// this class handles all toon movement and interactions.
/// </summary>
/// 
public class ProtoMove : NetworkBehaviour {
    public bool selected; // if toon is selected
    public int speed = 10; // speed my toon is moving
    private bool delayed = true; // delay so toon doesnt bounce

    [SyncVar]
    public Vector3 moveto; // tell toons where to go 

    [SyncVar]
    public string owner = ""; // which player own me?

    [SyncVar]
    public bool canattack = false; // i can attack

    [SyncVar]
    public bool canmove = false; // i can move

    [SyncVar]
    public Color pcolor = Color.white; // my toon color 

    // Use this for initialization
    void Start () {
        StartCoroutine(DelayStart(2));
    }

    // late init so it can be alive and gravity kicks in first.
    public IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        delayed = false;
        moveto = transform.position;

    }

    // Update is called once per frame
    void LateUpdate () {
        if (delayed) return; // only start after init.
        if (selected) // if i am selected then i am red or else i am set player color.
        {
            Renderer[] rends = gameObject.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.material.color = Color.red;
        }
        else {
            Renderer[] rends = gameObject.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.material.color = pcolor;
        }

        // if i am far from where i am moving to then look at it and keep moving. 
        // once i am close enough then froce set the position. 
        if (Vector3.Distance(transform.position, moveto) > 0.5f) {
            transform.LookAt(moveto);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else {
            transform.position = moveto;
        }
        
    }
}
