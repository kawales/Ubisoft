using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class playerComboScr : MonoBehaviour
{
    
    public const int UP = 0;
    public const int LEFT = 1;
    public const int DOWN = 2;
    public const int RIGHT = 3;
    public const int MOVECON = 4;
    public const int COMBOMAX = 4; //max combo length

    const string upLane="00";
    const string midLane="02";
    const string downLane="22";
    const string fire="30";
    const string water="12";
    const string earth="13";

    const string skull="00";
    const string golem="22";
    const string mage="02";

    Vector2 pos;

    [SerializeField]KeyCode[] controls; // W A S D CONTROL
    [SerializeField] Sprite[] comboSprites;
    [SerializeField] Sprite[] moveSpr = new Sprite[2];
    int[] currentComb;
    int pressed=-1;
    Transform spawnPointer;
    int moveSpawn=1; 
    public int tick=0;
    float leftTimePenalty=0;
    IEnumerator timeFrame;
    [SerializeField]TextMeshProUGUI jebeniTekst;
    [SerializeField]GameObject[] minions;
    static bool gameOver=false;
    bool transDone=true;
    Animator uiA;
    string lastComb="";
    [SerializeField]AnimationState state;
    // Start is called before the first frame update
    void Start()
    {
        //spawnPointer= this.transform.Find("pointer").transform;
        currentComb=new int[6]{ -1, -1, -1, -1,-1,-1};
        timeFrame=addTick();
        jebeniTekst.text=leftTimePenalty.ToString();
        StartCoroutine(timeFrame);
        pos=Vector2.zero;
        uiA = transform.Find("ui").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver==true)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            lostGame("Player 1 won");
        }
        transDone= !transform.Find("ui").GetComponent<Animator>().IsInTransition(0);
        if(leftTimePenalty>0)
        {
            transform.Find("ui").GetComponent<Animator>().SetBool("charge",true);
            uiA.speed=1/leftTimePenalty+0.1f;
            leftTimePenalty-=Time.deltaTime;
            jebeniTekst.text=leftTimePenalty.ToString();
        }
        else
        {
            transform.Find("ui").GetComponent<Animator>().SetBool("charge",false);
            
            leftTimePenalty=0.0f;
            jebeniTekst.text=0.ToString();
        }
        if(leftTimePenalty==0 && transDone && uiA.GetCurrentAnimatorClipInfo(0)[0].clip.name=="ready")
        {   
            if(Input.GetKeyDown(controls[UP]))
            {
                pressed=UP;
            }
            else if(Input.GetKeyDown(controls[DOWN]))
            {
                pressed=DOWN;
            }
            else if(Input.GetKeyDown(controls[LEFT]))
            {
                pressed=LEFT;
            }
            else if(Input.GetKeyDown(controls[RIGHT]))
            {
                pressed=RIGHT;
            }
            if(pressed!=-1)
            {
                StopCoroutine(timeFrame);
                timeFrame=addTick();
                StartCoroutine(timeFrame);
                if(tick>=6)
                {
                    //Debug.Log("COMBO!");
                    leftTimePenalty=0.5f;
                    currentComb=new int[6]{ -1, -1, -1, -1,-1,-1};
                    tick=0;
                    changeComboSprite();
                    pos=Vector2.zero;
                    pressed=-1;
                    return;
                }
                currentComb[tick]=pressed;
                //Debug.Log(pressed);
                changeComboSprite(tick,pressed);
                pressed=-1;
                
                tick++;
            }
        }
    }

    IEnumerator addTick()
    {
    //Debug.Log("ZAPOCET");
    while(true)
    {
        yield return new WaitForSeconds(0.5f);
        tick++;
        if(pressed==-1)
        {   checkComb(changeToTxt());
            currentComb=new int[6]{ -1, -1, -1, -1,-1,-1};
            tick=0;
            pos=Vector2.zero;
            changeComboSprite();
            //Debug.Log("OBRISAO COMB");
        }
    }
    }

    void changeComboSprite(int tick,int sprNumber)
    {
        this.transform.Find("ui").GetChild(tick).GetComponent<SpriteRenderer>().sprite=comboSprites[sprNumber];
        //GameObject.Find("ui1").transform.GetChild()
    }
    void changeComboSprite()
    {
        this.transform.Find("ui").GetChild(0).GetComponent<SpriteRenderer>().sprite=comboSprites[4];
        this.transform.Find("ui").GetChild(1).GetComponent<SpriteRenderer>().sprite=comboSprites[4];
        this.transform.Find("ui").GetChild(2).GetComponent<SpriteRenderer>().sprite=comboSprites[4];
        this.transform.Find("ui").GetChild(3).GetComponent<SpriteRenderer>().sprite=comboSprites[4];
        this.transform.Find("ui").GetChild(4).GetComponent<SpriteRenderer>().sprite=comboSprites[4];
        this.transform.Find("ui").GetChild(5).GetComponent<SpriteRenderer>().sprite=comboSprites[4];
        //GameObject.Find("ui1").transform.GetChild()
    }

    string changeToTxt()
    {
        string combS="";
        foreach(int c in currentComb)
        {
            if(c==-1)
            {
                combS+="X";
                continue;
            }
            combS+=c.ToString();
        }
        return combS;
    }

    bool checkComb(string combS)
    {
        //0 UP
        //1 LEFT
        //2 DOWN
        //3 RIGHT
        if(upLane==combS.Substring(0,2))
        {
            pos=new Vector2(transform.position.x,transform.position.y+2.5f);
        }
        else if(midLane==combS.Substring(0,2))
        {
            pos=new Vector2(transform.position.x,transform.position.y);
        }
        else if(downLane==combS.Substring(0,2))
        {
            pos=new Vector2(transform.position.x,transform.position.y-3f);
        }
        if(upLane+fire==combS)
        {
            Debug.Log("COMBO 1");
        }
        Debug.Log(pos);
        if(pos!=Vector2.zero)
        {
            if(combS==lastComb)
            {
                leftTimePenalty=1f;
            }
            if(combS.Substring(2,2)==fire && combS.Substring(4,2)==skull)
            {
                GameObject.Instantiate(minions[0],pos,Quaternion.Euler(0,0,0));
                if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==water  && combS.Substring(4,2)==skull)
            {
                GameObject.Instantiate(minions[1],pos,Quaternion.Euler(0,0,0));
                 if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==earth  && combS.Substring(4,2)==skull)
            {
                GameObject.Instantiate(minions[2],pos,Quaternion.Euler(0,0,0));
                 if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==fire && combS.Substring(4,2)==mage)
            {
                GameObject.Instantiate(minions[3],pos,Quaternion.Euler(0,0,0));
                if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==water  && combS.Substring(4,2)==mage)
            {
                GameObject.Instantiate(minions[4],pos,Quaternion.Euler(0,0,0));
                 if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==earth  && combS.Substring(4,2)==mage)
            {
                GameObject.Instantiate(minions[5],pos,Quaternion.Euler(0,0,0));
                 if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==fire && combS.Substring(4,2)==golem)
            {
                GameObject.Instantiate(minions[6],pos,Quaternion.Euler(0,0,0));
                if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==water  && combS.Substring(4,2)==golem)
            {
                GameObject.Instantiate(minions[7],pos,Quaternion.Euler(0,0,0));
                 if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else if(combS.Substring(2,2)==earth  && combS.Substring(4,2)==golem)
            {
                GameObject.Instantiate(minions[8],pos,Quaternion.Euler(0,0,0));
                 if(leftTimePenalty!=1f)leftTimePenalty=0.5f;
                lastComb=combS;
            }
            else
            {
                leftTimePenalty=0.3f;
            }
            return false;
        }
        else
        {
            if(combS=="XXXXXX")
            {
                    return false;
            }
            else
            {
                leftTimePenalty=0.3f;
            }
        } 

        return false;
    }

    static public void lostGame(string t)
    {
        gameOver=true;
        GameObject.Find("win").GetComponent<Animator>().SetBool("end",true);
         GameObject.Find("win").transform.Find("ui").GetChild(0).GetComponent<TextMeshProUGUI>().text=t+"                  won!";    
         //GameObject.Find("win").transform.GetChild(0).transform.GetChild()
    }
}
