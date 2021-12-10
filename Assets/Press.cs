using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Press : MonoBehaviour
{
    Canvas another;
    Canvas another2;
    Canvas another3;
    public GameObject miniMapPanel;
    public GameObject OptionPanel;
    public GameObject QAPanel;
    // Start is called before the first frame update
    void Start()
    {
        /*another = GameObject.Find("optionCanvas").GetComponent<Canvas>();
        another.enabled = false;
        another2 = GameObject.Find("miniMapCanvas").GetComponent<Canvas>();
        another2.enabled = false;*/
        OptionPanel.SetActive(false);
        miniMapPanel.SetActive(true);
        QAPanel.SetActive(false);
        //another3 = GameObject.Find("QACanvas").GetComponent<Canvas>();
        //another3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printMessage()
    {
        print("button pressed");
    }

        public void ShowMap()
    {
        miniMapPanel.SetActive(true);
    }

        public void HideMap()
    {
        miniMapPanel.SetActive(false);
    }

        public void ShowSet()
    {
        //another.enabled=true;
        OptionPanel.SetActive(true);
    }

        public void HideSet()
    {
        //another.enabled=false;
        OptionPanel.SetActive(false);
    }

        public void ShowQuest()
    {
        //another3.enabled=true;
        QAPanel.SetActive(true);
    }

        public void HideQuest()
    {
        //another3.enabled=false;
        QAPanel.SetActive(false);
    }

    


}
