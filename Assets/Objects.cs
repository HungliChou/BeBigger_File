using UnityEngine;
using System.Collections;

public enum ObjectState
{
    moving, bouncing, 
}

public class Objects : MonoBehaviour {

    public Vector2[] startDir;
    public Transform target;
    private Vector3 move;
    public ObjectState state;
    private bool canDestroy;
    private float canDestroyTimer;

    public Vector3 MoveDir{get{return move;} set{move = value;}}
	// Use this for initialization
	void Start () {
        canDestroy = false;
        canDestroyTimer = 0;
        state = ObjectState.moving;
        target = GameObject.Find("Controller/player").transform;
        facingPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += move*0.1f;
        CheckIsOut();
	}
    void facingPlayer()
    {
        Vector3 dif = target.position - transform.position;
        move = dif.normalized;

        if(tag=="bomb")
        {
            float angle = Vector3.Angle(dif,transform.up);
            if(transform.position.x<0)
                transform.Rotate(0,0,-angle);
            else
                transform.Rotate(0,0,angle);
        }

    }

    public void ChangeState(ObjectState _state)
    {
        state = _state;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject!=gameObject && col.tag!= "bound")
        {
            if(col.tag=="Player")
            {
                col.GetComponent<Character>().DoAction(gameObject);
            }
            else
            {
                if(tag=="triangle")
                {
                    Vector3 dif = col.transform.position - transform.position;
                    col.gameObject.GetComponent<Objects>().ChangeState(ObjectState.bouncing);
                    col.gameObject.GetComponent<Objects>().MoveDir = dif.normalized;
                    //print("bounce");
                }
            }
        }
    }

    void CheckIsOut()
    {
        if(!canDestroy)
        {
            if(canDestroyTimer>1)
            {
                canDestroy = true;
            }
            else
                canDestroyTimer += Time.deltaTime;
        }
        else
        {
            if(transform.position.x>10||transform.position.x<-10||transform.position.y>10||transform.position.y<-10)
                Destroy(gameObject);
        }
    }
            

}
