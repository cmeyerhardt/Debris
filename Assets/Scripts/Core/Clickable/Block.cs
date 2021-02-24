using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isGrounded = false;

    public Rigidbody rb = null;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Surface" || collision.gameObject.tag == "Obstacle")
        {
            isGrounded = true;
        }
        else
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if (block != null && block.isGrounded)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null && rb.velocity.magnitude >= .01f) 
        {
            isGrounded = false;
            rb.useGravity = true;
        }
    }
}
