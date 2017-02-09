using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public Character character;
    public float value;

	// Use this for initialization
	void Start () {
        value = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        if(character.MyMode == Mode.Dead)
            return;

        //OnDrawGizmosSelected();

        if (Input.GetAxis("Vertical")>0)
            transform.localPosition +=  new Vector3(0,value,0);
        if (Input.GetAxis("Vertical")<0)
            transform.localPosition +=  new Vector3(0,-value,0);
        if (Input.GetAxis("Horizontal")<0)
            transform.localPosition +=  new Vector3(-value,0,0);
        if (Input.GetAxis("Horizontal")>0)
            transform.localPosition +=  new Vector3(value,0,0);
	}
}
