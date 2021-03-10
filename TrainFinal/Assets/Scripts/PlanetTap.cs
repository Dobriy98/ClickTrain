using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlanetTap : MonoBehaviour
{
    [SerializeField] private GameManager GMScript;
    [SerializeField] private GameObject coat;
    public int plusCoat;
    private float speed;
    private float maxSpeed = 5;
    private bool maxSpeedBool = false;
    private static int secondsToDownSpeed = 20;
    private static int secondsToDownTemp = 5;
    private bool flag = false;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    flag = false;
                    break;
                case TouchPhase.Moved:
                    flag = true;
                    break;
                case TouchPhase.Ended:
                    if (flag == false)
                    {
                        if (GMScript != null)
                        {
                            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                            RaycastHit hit;
                            if (Physics.Raycast(touchPos, Camera.main.transform.forward, out hit))
                            {
                                if (hit.collider.tag == "planet")
                                {
                                    if (GMScript.ShowValues("coat") < GMScript.ShowValues("maxCoat"))
                                    {
                                        audioSource.Play();
                                        GMScript.ChangeValues("coat", plusCoat);
                                        SpawnCoat(touchPos);
                                    }
                                }
                            }
                        }
                    }
                break;
            }
        }
    }

    private void SpawnCoat(Vector2 posOnPlanet)
    {
        Vector2 randPos = new Vector2(Random.Range(-0.5f, 0f), Random.Range(0.5f, 1f));
        Vector2 posSpawn = posOnPlanet;
        GameObject item = Instantiate(coat, posSpawn, Quaternion.identity);
        item.GetComponent<MoveCoat>().ToImage(randPos);
    }
}
