using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTime : MonoBehaviour
{
    [Range(0.1f, 5f)]
    public float timeMultiplier = 1;

    public GameObject sceneLight = null;

    public GameObject torchLight = null;

    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        sceneLight.transform.rotation = Quaternion.Euler(22, Time.time*timeMultiplier, 0);
        sceneLight.GetComponent<Light>().intensity = Mathf.Abs((Mathf.Abs(sceneLight.transform.rotation.eulerAngles.y) - 180) / 360f);
        torchLight.GetComponent<Light>().intensity = 2f - Mathf.Abs((Mathf.Abs(sceneLight.transform.rotation.eulerAngles.y) - 180) / 90f);
    }
}
