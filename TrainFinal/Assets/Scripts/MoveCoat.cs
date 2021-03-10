using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCoat : MonoBehaviour
{
    private GameObject imagePos;
    private Vector2 randPos;
    private bool MoveTowards = false;

    private void Start() {
        imagePos = GameObject.Find("ImageCoal");
    }

    void Update()
    {
        if(!MoveTowards){
            transform.Translate(randPos * Time.deltaTime * 2);
        } else {
            transform.position = Vector2.MoveTowards(transform.position, randPos,2 * Time.deltaTime);
            if((Vector2)transform.position == randPos){
                Destroy(this.gameObject);
            }
        }
    }

    public void ToImage(Vector2 randPosFromScript){
        randPos = randPosFromScript;
        StartCoroutine("ChangePos");
    }

    private IEnumerator ChangePos(){
        yield return new WaitForSeconds(2f);
        randPos = imagePos.transform.position;
        MoveTowards = true;
    }
}
