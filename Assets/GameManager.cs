using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public List<GameObject> objects = new List<GameObject>();
    public List<GameObject> objects_Special = new List<GameObject>();
    public Character character;
    private float timer;
    public float releaseTime;
	// Use this for initialization
	void Start () {
        timer = 0;
        releaseTime = 0.5f;
        character = GameObject.Find("Controller/player").GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
        if(character.MyMode==Mode.Alive)
            DoTimer();
	}

    void DoTimer()
    {
        if(timer < releaseTime)
            timer += Time.deltaTime;
        else
        {
            timer = 0;
            CreateRandomObject();
        }
    }

    void CreateRandomObject()
    {
        int value =  Random.Range(0,objects.Count);
        GameObject obj = Instantiate(objects[value]) as GameObject;
        SetObjPosition(obj);
    }

    void CreateSpecialObject()
    {
        GameObject obj = Instantiate(objects_Special[0]) as GameObject;
        SetObjPosition(obj);
    }

    void SetObjPosition(GameObject obj)
    {
        int pos = Random.Range(0,2);
        if(pos==0)
        {
            obj.transform.position = new Vector3(Random.Range(-10,10),6,0);
        }
        else
        {
            obj.transform.position = new Vector3(10,Random.Range(-6,6),0);
        } 
    }
}
