using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShowAd : AdManager,IUnityAdsListener
{
    #if UNITY_IOS
    private string gameId = "3981836";
    #elif UNITY_ANDROID
    private string gameId = "3981837";
    #endif
    private bool testMode = false;
    private string placement = "rewardedVideo";
    public GameObject currentTarget;
    void Start()
    {
        Advertisement.Initialize (gameId, testMode);
        Advertisement.AddListener(this);
    }

    public void ShowRewardedVideo(){
        if(Advertisement.IsReady(placement)){
            Advertisement.Show(placement);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished){
            ShowCurrentTarget();
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    private void ShowCurrentTarget(){
        switch(this.target){
            case AdTarget.Asteroid0:
                RestoreCoal();
                break;
            case AdTarget.Asteroid1:
                RestoreGold();
             break;
            case AdTarget.Asteroid2:
                RestoreAll();
                break;
            case AdTarget.ChangeButton:
                ChangeButt();
                break;
        }
    }

    private void RestoreCoal(){
        if(GMScript != null){
            int maxVal = GMScript.ShowValues("maxCoat");
            GMScript.ChangeValues("coat", maxVal);

            if(currentTarget != null){
                currentTarget.GetComponent<AdComponentAsteroid>().HidePanel();
            }
        }
    }

    private void RestoreGold(){
        if(GMScript != null){
            int maxVal = GMScript.ShowValues("maxGold");
            GMScript.ChangeValues("gold", maxVal);

            if(currentTarget != null){
                currentTarget.GetComponent<AdComponentAsteroid>().HidePanel();
            }
        }
    }

    private void ChangeButt(){
        if(GMScript != null){
            if(currentTarget != null){
                currentTarget.GetComponent<AdComponent>().HidePanel();
            }
            int maxVal = GMScript.ShowValues("maxCoat");
            GMScript.ChangeValues("coatMinus", maxVal);

            int plusGold = GMScript.ShowValues("plusGold");
            GMScript.ChangeValues("gold", plusGold * 2);

        }
    }

    private void RestoreAll(){
        if(GMScript != null){
            int maxVal = GMScript.ShowValues("maxCoat");
            GMScript.ChangeValues("coat", maxVal);

            int maxValGold = GMScript.ShowValues("maxGold");
            GMScript.ChangeValues("gold", maxValGold);

            if(currentTarget != null){
                    currentTarget.GetComponent<AdComponentAsteroid>().HidePanel();
                }
        }
    }
}
