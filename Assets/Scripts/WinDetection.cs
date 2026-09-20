using UnityEngine;

public class WinDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int winningPlayer;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Rope"))
        {
            Debug.Log("Player " + winningPlayer + " wins the round");
        }
    }  

}