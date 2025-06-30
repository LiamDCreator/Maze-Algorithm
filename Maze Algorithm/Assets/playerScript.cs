using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // basic WASD movement 
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}