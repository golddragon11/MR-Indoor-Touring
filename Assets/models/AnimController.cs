using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimController : MonoBehaviour
{
    private Animator animator;
    private bool show;
    public Image myImage;
    public Sprite image1;
    private  bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Pathfinding.is_Navigation == true)
        {
            show = false;
            animator.SetBool("show", show);
        }

    }
    public void teleported()
    {
        isOpen = animator.GetBool("show");
        if(isOpen == true)
        {
            show = false;
            animator.SetBool("show", show);
            myImage.sprite = image1;
        }

    }

}
