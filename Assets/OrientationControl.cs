using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class OrientationControl : UIBehaviour
{
    // Start is called before the first frame update
    public Canvas LLCanvas;
    public Canvas PCanvas;
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        String sceneName = scene.name;

        if (sceneName == "BeginningScene")
        {
            Debug.Log("BS");
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (sceneName == "ARmode")
        {
            Debug.Log("AR");
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (sceneName == "VRmode")
        {
            Debug.Log("VR");
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        String sceneName = scene.name;
        if (sceneName == "VRmode")
        {
            if(Screen.orientation == ScreenOrientation.Portrait)
            {
                LLCanvas.enabled = false;
                PCanvas.enabled = true;
            }
            else if(Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                PCanvas.enabled = false;
                LLCanvas.enabled = true;
            }
        }
        else
        {
            PCanvas.enabled = true;
            if(LLCanvas != null) LLCanvas.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q");
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
