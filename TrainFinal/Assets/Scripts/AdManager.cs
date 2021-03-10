using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public GameObject parentPanel;
    public GameObject panel;
    public GameManager GMScript;
    public AdTarget target;

    [SerializeField] private Animation animTrain;
    [SerializeField] private AudioSource audioSourceTrain;
    private ShowAd showAdScript;
    private void Start()
    {
        showAdScript = GameObject.Find("GameManager").GetComponent<ShowAd>();
    }

    public enum AdTarget
    {
        Asteroid0,
        Asteroid1,
        Asteroid2,
        ChangeButton
    };

    public virtual void ShowPanel()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        showAdScript.target = target;
        showAdScript.currentTarget = this.gameObject;
        parentPanel.SetActive(true);
        if (panel != null)
        {
            panel.SetActive(true);
            if (animTrain != null)
            {
                animTrain["Train_animation_1"].speed = 0;
                audioSourceTrain.Stop();
            }
        }
    }

    public void HidePanel()
    {
        if (animTrain != null)
        {
            audioSourceTrain.Play();
            animTrain["Train_animation_1"].speed = 1;
        }
        parentPanel.SetActive(false);
        panel.SetActive(false);
    }
}
