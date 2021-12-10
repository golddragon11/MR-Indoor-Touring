using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using static Pathfinding;
using UnityEngine.SceneManagement;

public class DropAR : MonoBehaviour
{
    public ToggleGroup tg1;
    public static Dropdown dropdown;
    public static Dropdown dropdown2;
    public static bool infoGot = false;
    static List<string> listA = new List<string>() { "1F", "2F", "3F", "4F", "5F", "6F"};
    static public List<List<string>> listB = new List<List<string>>();
    public static int targetFloor, targetLoc, Loc3;
    private static bool isLoaded = false;

    public static void Init()
    {
        UpdateList();
    }

    void Start()
    {
        //StartCoroutine(listLoc());
        isLoaded = true;
        Debug.Log("AR is loaded");
        dropdown = GameObject.Find("floorListAR").GetComponent<Dropdown>();
        dropdown2 = GameObject.Find("locationListAR").GetComponent<Dropdown>();
    }

    public static void listAdd(Pathfinding.Floor floor)
    {
        int x = 0;
        List<string> LocName = new List<string>();
        for (int i = 0; i < floor.nodes.Count; i++)
        {
            string name = floor.nodes[i].nodeName;
            if (String.Compare(name, "無") != 0)
            {
                x++;
                //Debug.Log("i: "+i+floor.nodes[i].nodeName);
                LocName.Add(floor.nodes[i].nodeName);
            }
        }
        //Debug.Log("C: "+x);
        listB.Add(LocName);
    }
    public static void showtest(List<Pathfinding.Floor> info)
    {
        //info.ForEach(pri);

        /*dropdown.ClearOptions();
        dropdown2.ClearOptions();
        dropdown.AddOptions(listA);
        dropdown2.AddOptions(listB[0]);
        Loc3 = 1;
        targetFloor = 1;
        targetLoc = 0;*/
        //showTarget();
        /*foreach(Pathfinding.Floor floor in info)
        {
            foreach(Pathfinding.Node node in floor)
            {
                Debug.Log(node.nodeName);
            }
        }*/
        //Debug.Log(info[1].nodes[16].nodeName);
    }
    public void onvaluechanged()
    {
        dropdown2.value = 0;
        dropdown2.ClearOptions();
        dropdown2.AddOptions(listB[dropdown.value]);
        //Debug.Log("ddv: " + dropdown.value);
        if (dropdown.value == 0) targetFloor = 1;//1f
        else if (dropdown.value == 1) targetFloor = 2; //2f
        else if (dropdown.value == 2) targetFloor = 3; //3f
        else if (dropdown.value == 3) targetFloor = 4; //4f
        else if (dropdown.value == 4) targetFloor = 5; //5f
        else targetFloor = 6;
    }
    public void onValueChangedDD2()
    {
        Debug.Log(dropdown2.value);
        targetLoc = dropdown2.value;
    }
    public void onValueChangedT()
    {
        /*foreach(Toggle tg in tg1.ActiveToggles())
        {
            Debug.Log(tg.name);
        }*/
        if (String.Compare(tg1.ActiveToggles().FirstOrDefault().name, "stair") == 0) Loc3 = 1;     //stairs
        else Loc3 = 0;      //elevator
        Debug.Log(tg1.ActiveToggles().FirstOrDefault().name + ", " + Loc3);

    }

    public static IEnumerator listLoc()
    {
        yield return new WaitWhile(() => infoGot == true);
        Pathfinding.node_info.ForEach(listAdd);
        //Debug.Log(Pathfinding.node_info[0].nodes[2].nodeName);
        dropdown.ClearOptions();
        dropdown2.ClearOptions();
        dropdown.AddOptions(listA);
        dropdown2.AddOptions(listB[0]);
        Loc3 = 1;
        targetFloor = 1;
        targetLoc = 0;
    }
    
    public static void UpdateList()
    {
        Debug.Log("infoGot: " + infoGot);
        if (infoGot == false || isLoaded == false) return;
        Debug.Log("ARUpdate");
        Pathfinding.node_info.ForEach(listAdd);
        //Debug.Log(Pathfinding.node_info[0].nodes[2].nodeName);
        dropdown.ClearOptions();
        dropdown2.ClearOptions();
        dropdown.AddOptions(listA);
        dropdown2.AddOptions(listB[0]);
        Loc3 = 1;
        targetFloor = 1;
        targetLoc = 0;
    }
}
