using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClicks : MonoBehaviour
{
    [SerializeField] private Animator resoursesAnimator;
    [SerializeField] private Animator animatorButt;

    public void ResoursesButt(){
        if(resoursesAnimator != null){
            bool isOpen = resoursesAnimator.GetBool("show");
            resoursesAnimator.SetBool("show",!isOpen);
        }
        if(animatorButt != null){
            bool isOpenButt = animatorButt.GetBool("show");
            animatorButt.SetBool("show",!isOpenButt);
        }
    }

    public void CloseSideMenu(){
        if(resoursesAnimator != null){
            bool isOpen = resoursesAnimator.GetBool("show");
            resoursesAnimator.SetBool("show",!isOpen);
        }
        if(animatorButt != null){
            bool isOpenButt = animatorButt.GetBool("show");
            animatorButt.SetBool("show",!isOpenButt);
        }
    }
}
