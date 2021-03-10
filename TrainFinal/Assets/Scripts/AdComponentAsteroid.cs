using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdComponentAsteroid : AdManager
{
    public TextMeshProUGUI panelName;
    public TextMeshProUGUI panelText;

    public string headerText;
    public string bodyText;



    private void OnMouseDown()
    {
        this.panelName.text = this.headerText;
        this.panelText.text = this.bodyText;
        ShowPanel();
        Destroy(this.gameObject);
    }
}
