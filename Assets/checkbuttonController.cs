using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkbuttonController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    private bool popup;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator.GetBool("popup"));
    }
    public void closewindow()
    {
        popup = false;
        animator.SetBool("popup", popup);
    }
}
