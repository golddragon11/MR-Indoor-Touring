using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Drop;
using static DropAR;

public class returnBeginning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Drop.Init();
        DropAR.Init();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*static returnBeginning()
    {
        DropAR.Init();
        Drop.Init();
    }*/
    public void ButtonClicked()
    {
        SceneManager.LoadScene("BeginningScene");
        //SceneManager.UnloadScene("VRmode");
    }
}
