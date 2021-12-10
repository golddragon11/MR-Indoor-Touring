using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class directionControl : MonoBehaviour
{
    private bool ifStart;
    public GameObject arrow;
    private float xOfTarget;
    // Start is called before the first frame update
    void Start()
    {
        ifStart = false;        //初始化
        arrow.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Pathfinding.is_Navigation == false) arrow.SetActive(false);
        else {
            Debug.Log("arrow");
            arrow.SetActive(true);
            arrow.transform.rotation = Quaternion.LookRotation(Pathfinding.targetPos - arrow.transform.position, Vector3.up) * Quaternion.Euler(0, 90, 90);
        }


        /*xOfTarget = Camera.main.WorldToViewportPoint(Pathfinding.targetPos).x;
        //Debug.Log(xOfTarget);       
        //Debug.Log(img.transform.position);
        //text1.text = Pathfinding.targetPos.ToString();
        if (ifStart && Pathfinding.is_Navigation == true)        //判斷開始導航
        {
            if (isInView(Pathfinding.targetPos))      //判斷物體有沒有在畫面上
            {
                //img.enabled = false;
                img.transform.position = new Vector3(541.7693f, 1219f, -5.296f);
                img.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                img.enabled = true;
                if(xOfTarget <= 0)       //在畫面的左邊
                {
                    img.transform.position =new Vector3(158f, 1219f, 0f);
                    img.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else if (xOfTarget >=1)      //在畫面的右邊
                {
                    img.transform.position = new Vector3(923f, 1219f, 0f);
                    img.transform.rotation = Quaternion.Euler(0, 0, -90);
                }

            }
        }*/
    }
    public bool isInView(Vector3 worldPos)
    {
        Transform camTransform = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);     //判斷物體是否在相機前面


        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        else
            return false;
    }
}
