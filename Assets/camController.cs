using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class camController : MonoBehaviour
{
    public Camera ARCamera;
    public Camera miniMapCam;
    public LayerMask mask1F; // select "Everything" in inspector
    public LayerMask mask2F;    // select desired layer in inspector
    public LayerMask mask3F;
    public LayerMask Cmask1F; // select "Everything" in inspector
    public LayerMask Cmask2F;    // select desired layer in inspector
    public LayerMask Cmask3F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        miniMapCam.transform.position = new Vector3(ARCamera.transform.position.x, 15.31f, ARCamera.transform.position.z);

        if (ARCamera.transform.position.y >= 3 && ARCamera.transform.position.y <= 6) floorChanged(2);  // 2F
        else if (ARCamera.transform.position.y < 3) floorChanged(1);               // 1F
        else floorChanged(3);
    }

    void LateUpdate()
    {
        miniMapCam.transform.eulerAngles = new Vector3(88f, ARCamera.transform.eulerAngles.y, 0);
    }

    void floorChanged(int floor)
    {
        Scene scene = SceneManager.GetActiveScene();
        String sceneName = scene.name;
        if (floor == 1)
        {
            miniMapCam.cullingMask = mask1F;
            if (sceneName == "ARmode") ARCamera.cullingMask = Cmask1F;
        }
        else if(floor == 2)
        {
            miniMapCam.cullingMask = mask2F;
            if (sceneName == "ARmode") ARCamera.cullingMask = Cmask2F;
        }
        else
        {
            miniMapCam.cullingMask = mask3F;
            if (sceneName == "ARmode") ARCamera.cullingMask = Cmask3F;
        }

    }
}
