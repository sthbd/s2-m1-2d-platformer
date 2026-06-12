using UnityEngine;

public class PlayerFollows : MonoBehaviour
{
    public Rigidbody2D target;
    public SpriteRenderer targetSpriteRenderer;
    public float lookAheadOffsetX;

    [Range(0f, 1f)]
    public float lerpValue = 0.5f;


    // Update is called once per frame
    void FixedUpdate()
    {
        //
        bool isFacingLeft = targetSpriteRenderer.flipX == true;
        // Ternary operator 
        // this value = is this true?   this is true     : this is false
        // float offsetX = isFacingLeft ? -lookAheadOffsetX : lookAheadOffsetX;

        float offsetX = isFacingLeft
            ? -lookAheadOffsetX // of true
            : +lookAheadOffsetX; // if false

        // we must maintain the camera's z positon and set only X and Y
        // Copy target X and Y
        Vector3 targetPosition = target.position;
        // override target z with camera's z position
        targetPosition.z = this.transform.position.z;
        // apply offset to camera X
        targetPosition.x += offsetX;

        // set camera positio to new location
        Vector3 newPosition = Vector3.Lerp(this.transform.position, targetPosition, lerpValue);
        this.transform.position = newPosition;

    }
    
}
