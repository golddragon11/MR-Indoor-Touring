using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QQQ : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    private int x=0;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

                if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            x=1;
        }

        if(x==1)
        {
            
           /*/ mousePos = Input.mousePosition;
            if(aRRaycast.Raycast(mousePos,hits,TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;
                GameObject temp = Instantiate(tapObject, pose.position, pose.rotation);
                Vector3 look = transform.position;
                look.y = temp.transform.position.y;
                temp.transform.LookAt(look);
            }/*/

                    Vector3 relativePos = target.position - transform.position;//兩物體之間的距離
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = rotation;
        }else
        {
                             Vector3 relativePos2 = target2.position - transform.position;//兩物體之間的距離
                            Quaternion rotation2 = Quaternion.LookRotation(relativePos2, Vector3.up);
                            transform.rotation = rotation2;
        }


    }
}

