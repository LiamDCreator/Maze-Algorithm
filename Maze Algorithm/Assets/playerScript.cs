using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // -1, 0, or 1
        float moveY = Input.GetAxisRaw("Vertical");   // -1, 0, or 1

        Vector3 move = new Vector3(moveX, moveY, 0).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}