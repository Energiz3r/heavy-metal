using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool pressedJump;
    float horizontalInput;
    Rigidbody playerObj;
    bool isGrounded;
    [SerializeField] Transform groundCheckTransform = null;
    [SerializeField] LayerMask playerMask;
    int superJumps;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressedJump = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    // called once every physics update - 100hz default
    private void FixedUpdate()
    {
        playerObj.velocity = new Vector3(horizontalInput * 2, playerObj.velocity.y, 0);
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }
        if (pressedJump)
        {
            float jumpPower = 5f;
            if (superJumps > 0)
            {
                jumpPower *= 2;
                superJumps--;
            }
            playerObj.AddForce(Vector2.up * jumpPower, ForceMode.VelocityChange);
            pressedJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            superJumps++;
        }
    }
}
