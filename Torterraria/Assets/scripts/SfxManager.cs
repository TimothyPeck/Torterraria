using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioSource audio;

    public AudioClip enemyDeath;
    public AudioClip enemyHit;
    public AudioClip enemy1;
    public AudioClip enemy2;
    public AudioClip enemy3;
    public AudioClip enemy4;
    public AudioClip boss;
    public AudioClip playerDeath;
    public AudioClip playerHit;
    public AudioClip mine;
    public AudioClip dig;
    public AudioClip cut;
    public AudioClip drop;

    public static SfxManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }
}
