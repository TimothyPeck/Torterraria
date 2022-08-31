using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour
{
    [Range(0.1f, 5f)]
    public float timeMultiplier = 1;

    public GameObject sceneLight = null;

    public GameObject torchLight = null;

    public Image backgroundImage = null;

    public Sprite backgroundDay = null;

    public Sprite backgroundNight = null;

    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float absFloat = Mathf.Abs(sceneLight.transform.rotation.eulerAngles.y);
        sceneLight.transform.rotation = Quaternion.Euler(22, Time.time*timeMultiplier, 0);
        sceneLight.GetComponent<Light>().intensity = Mathf.Abs((absFloat - 180) / 360f);
        torchLight.GetComponent<Light>().intensity = 2f - Mathf.Abs((absFloat - 180) / 90f);
        if (absFloat < 270 && absFloat > 90)
        {
            backgroundImage.sprite = backgroundNight;
        }
        else
        {
            backgroundImage.sprite = backgroundDay;
        }
    }
}
