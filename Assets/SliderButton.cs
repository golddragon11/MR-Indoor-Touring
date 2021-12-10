using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
    public GameObject PanelMenu;
    public Sprite image1;
    public Sprite image2;
    private Image myImage;
    private static bool isOpen; 
    void Start()
    {
        myImage = GetComponent<Image>();
    }
    public void ShowHideMenu()
    {
        if (PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
            if (animator != null)
            {
                isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);
                if (isOpen == true)
                {
                    myImage.sprite = image1;
                }
                else myImage.sprite = image2;
            }
        }
    }
}
