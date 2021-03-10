using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldToInformation : MonoBehaviour, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private ToolTip toolTipScript;
    [SerializeField] private GameManager GMScript;
    private Transform toolPos;

    private void Start() {
        toolPos = this.gameObject.transform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        string[] textForTip = new string[2];
        if(GMScript != null){
            textForTip = GMScript.InfForTip(gameObject.name);
        }
        if(toolTipScript != null){
            toolTipScript.ShowToolTip(textForTip[1], textForTip[0], toolPos);
        }
    }
    public void OnPointerExit(PointerEventData eventData){
        if(toolTipScript != null){
            toolTipScript.HideToolTip();
        }
    }
}
