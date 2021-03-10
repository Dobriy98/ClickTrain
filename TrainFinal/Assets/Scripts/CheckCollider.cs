using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckCollider : MonoBehaviour
{
    [SerializeField] private GameManager GMScript;
    [SerializeField] private Slider sliderTemp;
    [SerializeField] private Image sliderTempImage;
    private Animator anim;

    private void Start()
    {
        anim = GameObject.Find("Train_001").GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "train")
        {
            sliderTemp.value += 1;
            if (sliderTemp.value >= 0)
            {
                sliderTempImage.color = new Color32(255, 50, 60, 255);
            }
            else
            {
                sliderTempImage.color = new Color32(50, 60, 255, 255);
            }
            GMScript.ChangeValues("coatMinus", 1);
            GMScript.ChangeValues("tempPlus", 1);
        }
    }
}
