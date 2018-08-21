﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatformController : MonoBehaviour {

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	// [HideInInspector] public static Transform location;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	public Transform frontCheck;

	private bool grounded = false;
	private bool frontCollide = false;
	private Animator anim;
	private Rigidbody2D rb2D;
	private bool shoot = false;

	public Transform firePoint;
	public GameObject simpleBullet;


	void Awake () {
		anim = GetComponent<Animator>();
		rb2D = GetComponent<Rigidbody2D>();
		// location = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		frontCollide = Physics2D.Linecast(transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		if(Input.GetButtonDown("Jump") && grounded) {
			jump = true;
		}

		if(Input.GetButtonDown("Fire1")) {
			shoot = true;
		}
	}

	void FixedUpdate() {
		float h = Input.GetAxis("Horizontal");

		anim.SetFloat("Speed", Mathf.Abs(h));

		if(!frontCollide){
			if(h*rb2D.velocity.x < maxSpeed) {
				rb2D.AddForce(Vector2.right * h * moveForce);
			}
			if(Mathf.Abs(rb2D.velocity.x) > maxSpeed) {
				rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * maxSpeed, rb2D.velocity.y);
			}
		}

		if((h>0 && !facingRight) || (h<0 && facingRight)) {
			Flip();
		}

		if(jump) {
			anim.SetTrigger("Jump");
			rb2D.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}

		if(shoot) {
			shoot = false;
			Instantiate(simpleBullet, firePoint.position, firePoint.rotation);
		}
	}






    void OnTriggerEnter2D(Collider2D other) 
    {
    	if (other.gameObject.CompareTag("Pickup"))
    {

    		other.gameObject.SetActive (false);

    }


    }














	void Flip() {
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
