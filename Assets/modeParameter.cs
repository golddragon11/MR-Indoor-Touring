using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Pathfinding;
using static Drop;
using static DropAR;
public class modeParameter : MonoBehaviour
{
    public static int mode = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void VRbuttonClicked()
    {
        //if (Drop.infoGot == false) return;
        mode = 1;
        SceneManager.LoadScene("VRmode"); 
    }
    public void ARbuttonClicked()
    {
        //if (Drop.infoGot == false) return;
        mode = 2;
        SceneManager.LoadScene("ARmode");
    }
}
