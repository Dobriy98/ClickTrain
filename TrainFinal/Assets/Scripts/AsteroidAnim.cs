using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidAnim : MonoBehaviour
{
    private Animation objAnim;
    public AnimationClip anim;

    void Start()
    {
        objAnim = GetComponent<Animation>();
        objAnim.Play(anim.name);
    }

    private void Update()
    {
        if (!objAnim.IsPlaying(anim.name))
        {
            this.gameObject.SetActive(false);
        }
    }
}
