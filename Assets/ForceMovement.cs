using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ForceMovement : MonoBehaviour
{
    public float force;
    public float rotation_speed;
    private float rotation_tmp;
    private Rigidbody rb;
 
    void Start() {
        rb = GetComponent<Rigidbody>();
    }
 
      void FixedUpdate() {
          if (Input.GetKey(KeyCode.W))
          {
            rb.velocity = Vector3.zero;
              rb.AddForce(transform.forward * force,ForceMode.VelocityChange);
          } else if(Input.GetKey(KeyCode.S)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * (-1) * force,ForceMode.VelocityChange);
        }
        else
        {
            rb.Sleep();
        }
          rotation_tmp = Input.GetAxis("Horizontal") * rotation_speed;
          rotation_tmp *= Time.deltaTime;
          transform.Rotate(0,rotation_tmp,0); // rotate around Y axis
      }
}