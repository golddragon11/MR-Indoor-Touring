using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text1;
    void Start()
    {
        text1 = GetComponent<Text>();
        //text1.text = Drop.listB[Drop.Loc1][Drop.Loc2];
        //text1.text = Pathfinding.DebugText.text;
        //Debug.Log(Drop.listB[Drop.Loc1][Drop.Loc2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
