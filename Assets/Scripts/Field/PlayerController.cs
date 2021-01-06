using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;

    Rigidbody rb;
    Animator animator;

    public bool canControl = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        canControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canControl) return;

        mainCamera.transform.position = transform.position + new Vector3(0, 8, -5);

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0) * Input.GetAxisRaw("Horizontal")
                + new Vector3(0, 0, 1) * Input.GetAxisRaw("Vertical"));
        }
        else
            animator.SetBool("Run", false);
    }

    private void FixedUpdate()
    {
        if (!canControl) return;

        rb.AddForce(new Vector3(1, 0, 0) * Input.GetAxisRaw("Horizontal") * 10
            + new Vector3(0, 0, 1) * Input.GetAxisRaw("Vertical") * 10, ForceMode.Impulse);
    }
}
