using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;
    public int health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        health = baseHealth;
    }
}
