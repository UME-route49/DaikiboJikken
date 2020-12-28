using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;

    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = transform.position + new Vector3(0, 8, -5);
        //mainCamera.transform.LookAt(transform.position);

        rb.AddForce(new Vector3(1, 0, 0) * Input.GetAxisRaw("Horizontal") * 100
            + new Vector3(0, 0, 1) * Input.GetAxisRaw("Vertical") * 100);

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0) * Input.GetAxisRaw("Horizontal")
                + new Vector3(0, 0, 1) * Input.GetAxisRaw("Vertical"));
        }
        else
            animator.SetBool("Run", false);
    }
}
