using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Option : MonoBehaviour
{
	public TextMeshProUGUI output;
	
	public void HandleInputData(int val)
	{
		if(val == 0)
		{
            output.text = "A";
		}
		if(val == 1)
		{
            output.text = "B";
		}
		if(val == 2)
		{
            output.text = "C";
		}
	}
}
