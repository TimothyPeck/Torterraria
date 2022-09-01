using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        //moves the camera to the position of the player, offest by the defined offset
        transform.position = player.position + offset;
    }
}
