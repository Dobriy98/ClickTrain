using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePanelScript : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject detectClicks;
    private Animator animator;
    private Animator animatorButt;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (buttonsPanel != null)
        {
            animatorButt = buttonsPanel.GetComponent<Animator>();
        }
    }

    public void ShowHidePanel()
    {
        if (animator != null && animatorButt != null)
        {
            if (Input.touchCount == 1)
            {
                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);

                bool isOpenButt = animatorButt.GetBool("show");
                animatorButt.SetBool("show", !isOpenButt);

                detectClicks.SetActive(true);
            }
        }
    }

    public void CloseDetectClicks()
    {
        if (animator != null && animatorButt != null)
        {
            bool isOpen = animator.GetBool("show");
            if (isOpen)
            {
                animator.SetBool("show", !isOpen);
                bool isOpenButt = animatorButt.GetBool("show");
                Debug.Log(isOpenButt);
                animatorButt.SetBool("show", !isOpenButt);
            }

            detectClicks.SetActive(false);
        }
    }
}
