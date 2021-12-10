using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupAnim : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void playAnim()
    {
        Debug.Log("Triggered");
        animator.SetTrigger("appear");

        //anim.Play("Base Layer.alphaAnim", 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
