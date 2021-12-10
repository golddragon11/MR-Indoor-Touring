using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Pathfinding;
using static Drop;
using static DropAR;
using static modeParameter;
using System;
using UnityEngine.XR.ARFoundation;
using TMPro;
using Fungus;


public class Nav : MonoBehaviour
{
    Pathfinding p;
    //public static Deque<int> xxxx=new Deque<int>();
    public static Vector3 nextpoint ;
    private Animator anim;
    public int floor;
    public int target;
    public int mode;
    public Image targetInfo;
    public Image userInfo;
    public Image floorBG;
    public Canvas canvas;
    private int currentFloor;
    private float moveSpeed = 3f, turnSpeed = 1f;
    public GameObject Camera;
    public GameObject MainCam;
    public GameObject ARSO;
    public GameObject ARsession;
    public Text ButtonText;
    public Text positionText;
    public Text userInfoText;
    public GameObject arrow;
    public Joystick joystick;
    public Joystick RotJoystick;
    public ARSessionOrigin aRSessionOrigin;
    public List<int> LocList1F = new List<int>();
    public List<int> LocList2F = new List<int>();
    public List<int> LocList3F = new List<int>();
    public List<int> LocList4F = new List<int>();
    public List<int> LocList5F = new List<int>();
    public List<int> LocList6F = new List<int>();
    public float isPressed = 0f;
    public float timer;
    public Flowchart myFlowchart;
    public Vector3 move;
    private IEnumerator enu;
    //public Text tarLocText;
    // Start is called before the first frame update
    void Start()
    {
        enu = positioning();
        StartCoroutine(enu);
        userInfo.enabled = false;
        floorBG.enabled = true;
        targetInfo.enabled = false;
        mode = modeParameter.mode;
        timer = 1f;
        //mode = myFlowchart.GetIntegerVariable("mode");
        anim = GetComponent<Animator>();
        p = GameObject.Find("Pathfinding").GetComponent<Pathfinding>();
        arrow.SetActive(false);
        if (mode == 1) {
            ARSO.SetActive(false);//aRSessionOrigin.SetActive(false);
            //ARsession.SetActive(false);
        } 
        else if (mode == 2) MainCam.SetActive(false);
    }
    public void Gonav()
    {
        Scene scene =SceneManager.GetActiveScene();
        String sceneName = scene.name;
        if(sceneName == "VRmode")
        {
            //if (Drop.Loc3 == 1) Pathfinding.Preference(true);
            //else Pathfinding.Preference(false);
            floor = Drop.targetFloor;
            Debug.Log("Loc: " + Drop.targetLoc + "Floor: "+ Drop.targetFloor + "tog:" + Drop.Loc3);
       
            if(floor == 1)
            {
                target = LocList1F[Drop.targetLoc];
            }
            else if(floor == 2)
            {
                target = LocList2F[Drop.targetLoc];
            }
            else if(floor == 3)
            {
                target = LocList3F[Drop.targetLoc];
            }
            else if (floor == 4)
            {
                target = LocList4F[Drop.targetLoc];
            }
            else if (floor == 5)
            {
                target = LocList5F[Drop.targetLoc];
            }
            else if (floor == 6)
            {
                target = LocList6F[Drop.targetLoc];
            }
            if (Drop.Loc3 == 1) p.preference(true);
            else if (Drop.Loc3 == 0) p.preference(false);
        }
        else if(sceneName == "ARmode")
        {
            //if (Drop.Loc3 == 1) Pathfinding.Preference(true);
            //else Pathfinding.Preference(false);
            floor = DropAR.targetFloor;
            Debug.Log("Loc: " + DropAR.targetLoc + "Floor: " + DropAR.targetFloor);

            if (floor == 1)
            {
                target = LocList1F[DropAR.targetLoc];
            }
            else if (floor == 2)
            {
                target = LocList2F[DropAR.targetLoc];
            }
            else if (floor == 3)
            {
                target = LocList3F[DropAR.targetLoc];
            }
            else if (floor == 4)
            {
                target = LocList4F[DropAR.targetLoc];
            }
            else if (floor == 5)
            {
                target = LocList5F[DropAR.targetLoc];
            }
            else if (floor == 6)
            {
                target = LocList6F[DropAR.targetLoc];
            }
            Debug.Log("tog:" + DropAR.Loc3);
            if (DropAR.Loc3 == 1) p.preference(true); //樓梯
            else if (DropAR.Loc3 == 0) p.preference(false); //電梯

        }
        //print("floor="+Drop.floor+" "+"target="+Drop.target);
        Debug.Log("start:" + floor + ", "+ target);
        if (Pathfinding.is_Navigation == false)
        {
            //anim.speed = -1f;
            //anim.Play("sliderMenu");
            ButtonText.text = "結束導航";
            Debug.Log("target: "+target);
            //p.Start_Navigation(floor, target);
            //positionText.text =  "(floor, target) = (" +floor + ", " + target + ")";
            p.call_navigation(floor, target);
            arrow.SetActive(true);
            userInfo.enabled = true;
            floorBG.enabled = false;
            targetInfo.enabled = true;
        }
        else if (Pathfinding.is_Navigation == true)
        {
            ButtonText.text = "開始導航";
            arrow.SetActive(false);
            userInfo.enabled = false;
            floorBG.enabled = true;
            p.End_Navigation();
        }
        //int len =  xxxx.size();
        //print("當前長度"+len);
        //for(int i=0;i<len;i++)
        //{
        //Vector3 position= node_info[floor-1].floor_Node[Path.index(i)].pos;
        // print("當前目標"+position);
        //}

    }
    
    public void upstairs()
    {
        if(mode == 1 && MainCam.transform.position.y < 18f)
        {
            MainCam.transform.position = new Vector3(MainCam.transform.position.x, MainCam.transform.position.y + 3f, MainCam.transform.position.z);
        }
        else if(mode == 2 && aRSessionOrigin.transform.position.y < 18f)
        {
            aRSessionOrigin.transform.position = new Vector3(aRSessionOrigin.transform.position.x, aRSessionOrigin.transform.position.y + 3f, aRSessionOrigin.transform.position.z);
        }
    }
    public void downstairs()
    {
        if (mode == 1 && MainCam.transform.position.y >=3f)
        {
            MainCam.transform.position = new Vector3(MainCam.transform.position.x, MainCam.transform.position.y - 3f, MainCam.transform.position.z);
        }
        else if (mode == 2 && aRSessionOrigin.transform.position.y >= 3f)
        {
            aRSessionOrigin.transform.position = new Vector3(aRSessionOrigin.transform.position.x, aRSessionOrigin.transform.position.y - 3f, aRSessionOrigin.transform.position.z);
        }
    }
    public void tpPara() {
        floor = Drop.targetFloor;
        if (floor == 1)
        {
            target = LocList1F[Drop.targetLoc];
        }
        else if (floor == 2)
        {
            target = LocList2F[Drop.targetLoc];
        }
        else if (floor == 3)
        {
            target = LocList3F[Drop.targetLoc];
        }
        else if (floor == 4)
        {
            target = LocList4F[Drop.targetLoc];
        }
        else if (floor == 5)
        {
            target = LocList5F[Drop.targetLoc];
        }
        else if (floor == 6)
        {
            target = LocList6F[Drop.targetLoc];
        }
        p.teleport(floor-1, target);
    }
    public void positioningBtn()
    {
        StartCoroutine(enu);
    }
    private IEnumerator positioning()
    {
        if(canvas != null)
        {
            canvas.enabled = true;
            //campostext.text = "wait for moving";
            yield return new WaitForSeconds(5);
            //yield return new WaitWhile(() => aRCamera.transform.localPosition.z <2);
            canvas.enabled = false;
        }

        //campostext.text = "after moving";
        /*Vector3 OriginPos = aRSessionOrigin.transform.position;
         aRSessionOrigin.transform.position = new Vector3(aRSessionOrigin.transform.position.x + aRCamera.transform.localPosition.x, aRSessionOrigin.transform.position.y + aRCamera.transform.localPosition.y, aRSessionOrigin.transform.position.z + aRCamera.transform.localPosition.z);
         aRSessionOrigin.transform.rotation = Quaternion.Euler(0, 180f, 0);
         aRSessionOrigin.transform.position = OriginPos;*/
    }

    public void Update()
    {

        if (mode == 1)
        {
            move = MainCam.transform.position;
            if (MainCam.transform.position.y < 3f)
            {
                currentFloor = 1;
            }
            else if (MainCam.transform.position.y >= 3f && MainCam.transform.position.y < 6f)
            {
                currentFloor = 2;
            }
            else if (MainCam.transform.position.y >= 6f && MainCam.transform.position.y < 10f)
            {
                currentFloor = 3;

            }
            else if (MainCam.transform.position.y >= 10f && MainCam.transform.position.y < 13f)
            {
                currentFloor = 4;
            }
            else if (MainCam.transform.position.y >= 13f && MainCam.transform.position.y < 16f)
            {
                currentFloor = 5;

            }
            else currentFloor = 6;
        }
        else if (mode == 2) 
        { 
            move = Camera.transform.position;
            //positionText.text = move.ToString();
            if (Camera.transform.position.y < 3f)
            {
                currentFloor = 1;
            }
            else if (Camera.transform.position.y >= 3f && Camera.transform.position.y < 6f)
            {
                currentFloor = 2;
            }
            else if (Camera.transform.position.y >= 6f && Camera.transform.position.y < 10f)
            {
                currentFloor = 3;

            }
            else if (Camera.transform.position.y >= 10f && Camera.transform.position.y < 13f)
            {
                currentFloor = 4;
            }
            else if (Camera.transform.position.y >= 13f && Camera.transform.position.y < 16f)
            {
                currentFloor = 5;

            }
            else currentFloor = 6;
        }
        //positionText.text = "現在座標: "+move.ToString()+"\n目標: "+ Pathfinding.targetPos.ToString();
        if (Pathfinding.is_Navigation == true)
        {
            timer += Time.deltaTime;
            //positionText.text = "現在座標: " + move.ToString() + "\n目標: " + Pathfinding.targetPos.ToString();
            positionText.text = "導航目標\n" + Pathfinding.node_info[floor-1].nodes[target].nodeName + ", " + floor + "F";
            if(timer >= 0.025f)
            {
                arrow.transform.rotation = Quaternion.LookRotation(Pathfinding.targetPos - move, Vector3.up) * Quaternion.Euler(0, 90, 62);
                timer = 0f;
            }
        }
        else positionText.text = "";
        if(Pathfinding.is_Navigation == false)  userInfoText.text = "現在樓層\n" + currentFloor + "F"; 
        MainCam.transform.Translate(joystick.Vertical * moveSpeed * Time.deltaTime * Vector3.forward+ joystick.Horizontal*Vector3.right * moveSpeed * Time.deltaTime);
        //aRSessionOrigin.transform.Translate(joystick.Horizontal * moveSpeed * Time.deltaTime * Vector3.forward);
        MainCam.transform.Rotate(RotJoystick.Horizontal * turnSpeed * Vector3.up);
        //positionText.text = move.ToString();
        targetInfo.enabled = true;
        p.GetPosition(currentFloor, move);//當前樓層
        //IF(碰到樓梯)3FLOOR++     
        if (Input.GetKey(KeyCode.Space)) 
        {
            aRSessionOrigin.transform.position = new Vector3(aRSessionOrigin.transform.position.x, 4.5f, aRSessionOrigin.transform.position.z);
            MainCam.transform.position = new Vector3(MainCam.transform.position.x, 4.5f, MainCam.transform.position.z);
        }
     
        if (Input.GetKey("u")) { aRSessionOrigin.transform.position = new Vector3(aRSessionOrigin.transform.position.x, 7.5f, aRSessionOrigin.transform.position.z); }


    }








}

