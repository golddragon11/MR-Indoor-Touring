using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class tap : MonoBehaviour
{
    [Header("想放的")]
    public GameObject obj;

    //private ARRaycastManager aRRaycast;

    //private List<ARRaycastHit> hits =new List<ARRaycastHit>();

   // private Vector2 mousePos;

    private void Start()

    {
        //aRRaycast = GetComponent<ARRcastManager>();
    }
     private void update()

    {
        TapObject();
    }

    private void TapObject()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
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
                obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                obj.transform.position = transform.position + new Vector3(2,0,0);
                obj.transform.localScale = new Vector3(4, 4, 4);
    


        }
    }


}
