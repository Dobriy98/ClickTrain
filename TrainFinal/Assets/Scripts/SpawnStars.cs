using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStars : MonoBehaviour
{
    [SerializeField] private GameObject star;
    private int screenW, screenH;
    private Vector3 screen;
    private int maxStars;
    private SpriteRenderer sr;

    void Start()
    {
        maxStars = 30;
        screenW = Screen.width;
        screenH = Screen.height;
        screen = Camera.main.ScreenToWorldPoint(new Vector3(screenW, screenH,0));
        StartCoroutine(SpawnStar());
    }


    IEnumerator SpawnStar(){
        while(true){
            Vector3 randPos = new Vector3(Random.Range(-screen.x, screen.x), Random.Range(-screen.y, screen.y),10);
            GameObject starObj = Instantiate(star, randPos, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Destroy(starObj,4f);
        }
    }


}
