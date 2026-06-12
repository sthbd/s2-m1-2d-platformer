    using UnityEngine;

public class WateringCan : MonoBehaviour

{
    public static int NumberCollected = 0;   

    public bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision2d)
    {
        // see if collider object is tagged as player 
        if (collision2d.gameObject.CompareTag("Player") == true)
        {

            // add number of these collected 

            NumberCollected +=1;
            isCollected = true;
            Debug.Log("Number of watering cans collected: " + NumberCollected);

            // disable object once it is collected
            this.gameObject.SetActive(false);
        }
    }
}
