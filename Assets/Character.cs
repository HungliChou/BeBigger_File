using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum Mode{Alive, Dead, Win}
public enum Buffname {Invincible,}

[System.Serializable]
public class Buff
{
    public Buffname name;
    public float duration;

    public Buff(Buffname _name, float _duration)
    {
        name = _name;
        duration = _duration;
    }
}

public class Character : MonoBehaviour {

    public float myScale;
    public float speed;
    public float value;
    public float value_bomb;
    public Transform goal;
    public Mode mode;
    public Mode MyMode{get{return mode;}}
    public List<Buff> buffs = new List<Buff>();

    void Awake()
    {
        mode = Mode.Alive;
    }
	// Use this for initialization
	void Start () {
        myScale = transform.localScale.x;
        value = 0.5f;
        value_bomb = 0.8f;
        speed = 0.03f;
	}
	
    void OnGUI()
    {
        if(mode!=Mode.Alive)
        {
            if(mode==Mode.Dead) 
                GUI.Label(new Rect(Screen.width/2-150, Screen.height/2, 150, 100), "Gameover");
            else if(mode==Mode.Win) 
                GUI.Label(new Rect(Screen.width/2-150, Screen.height/2, 150, 100), "Win"); 
            if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 80, 30), "Restart"))
            {
                RestartGame();
            }
        }

    }

	// Update is called once per frame
	void Update () {
        if(mode == Mode.Dead)
            return;

        //OnDrawGizmosSelected();

        if (Input.GetAxis("Vertical")>0)
            transform.localPosition +=  new Vector3(0,speed,0);
        if (Input.GetAxis("Vertical")<0)
            transform.localPosition +=  new Vector3(0,-speed,0);
        if (Input.GetAxis("Horizontal")<0)
            transform.localPosition +=  new Vector3(-speed,0,0);
        if (Input.GetAxis("Horizontal")>0)
            transform.localPosition +=  new Vector3(speed,0,0);

        if(myScale<= 0)
        {
            transform.localScale = Vector3.zero;
            mode = Mode.Dead;
            Time.timeScale = 0;
        }
        else if(myScale>= 6)
        {
            mode = Mode.Win;
        }

        UpdateBuffs();
	}
        
    public void UpdateBuffs()
    {
        if(buffs.Count>0)
        {
            foreach(Buff buff in buffs)
            {
                if(buff.duration>0)
                {
                   buff.duration -= Time.deltaTime;
                }
                else
                    buffs.Remove(buff);
            }
        }
    }

    public bool CkeckIfhasBuff(Buffname name)
    {
        bool returnValue = false;
        if(buffs.Count>0)
        {
            foreach(Buff buff in buffs)
            {
                if(buff.name == name)
                {
                    returnValue = true;
                    break;
                }
            }
        }
        return returnValue;
    }

    public void DoAction(GameObject coll)
    {
        if(mode!= Mode.Alive)
            return;
        float ratio = 0;
        switch(coll.tag)
        {
            case "absorb":
                ratio = (myScale+value) / myScale;
                transform.localScale += new Vector3(value,value,1);
                goal.localScale /= ratio;
                Destroy(coll.gameObject);
                myScale += value;
                print("ab");
                break;
            case "triangle":
                Vector3 dif = coll.transform.position - transform.position;
                coll.gameObject.GetComponent<Objects>().ChangeState(ObjectState.bouncing);
                coll.gameObject.GetComponent<Objects>().MoveDir = dif.normalized;
                print("bounce");
                break;
            case "bomb":
                if(!CkeckIfhasBuff(Buffname.Invincible))
                {
                    ratio = (myScale-value_bomb) / myScale;
                    transform.localScale -= new Vector3(value_bomb,value_bomb,1);
                    goal.localScale /= ratio;
                    myScale -= value_bomb;
                    print("bomb");
                }
                Destroy(coll.gameObject);
                break;
            case "special1":
                Buff invincible = new Buff(Buffname.Invincible, 5);
                buffs.Add(invincible);
                Destroy(coll.gameObject);
                break;
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.FindChild("circle_hollow").localScale = Vector3.one;
        myScale = 1;
        mode = Mode.Alive;
    }

    IEnumerator GlowingGoalCircle()
    {
        
    }
}
