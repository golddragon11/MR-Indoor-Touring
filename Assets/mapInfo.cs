using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;
public class mapInfo : MonoBehaviour
{
    public Flowchart flowchart;
    public Image img;
    private bool show;
    // Start is called before the first frame update
    void Start()
    {
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        show = flowchart.GetBooleanVariable("showMapInfo");
        if (show == true)
        {
            img.enabled = true;

        }
    }
    public void Onclicked()
    {
        img.enabled = false;
        flowchart.SetBooleanVariable("showMapInfo", false);
    }

}
