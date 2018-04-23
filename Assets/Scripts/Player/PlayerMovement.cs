﻿using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //TODO implement breaks to have more realistics movement
    private Rigidbody2D rigid;

    public float MovementSpeed;
    public float maxSpeed;

    public float jumpSpeed = 8f;
    public float gravityJumpModifier;

    public GroundCheck groundCheck;
    public bool IsGrounded = false;
    private bool FlippedX = false;

    private SpriteRenderer sr;
    private AudioSource _playerAudioSource;

    public AudioClip JumpSound;
    public AudioClip WalkSound;

    float defaultGravityValue;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _playerAudioSource = GetComponent<AudioSource>();

        defaultGravityValue = rigid.gravityScale;

    }

    //error here in detecting ground :(



    void FixedUpdate()
    {
        float s = Input.GetAxis("Horizontal");

        rigid.AddForce(new Vector2(s * MovementSpeed, 0));
        rigid.velocity = new Vector2(
            Vector2.ClampMagnitude(rigid.velocity,maxSpeed).x,rigid.velocity.y
            );

        if (s < 0f)
        {
            FlippedX = true;
        }
        else if (s > 0f)
        {
            FlippedX = false;
        }
        sr.flipX = FlippedX;

        IsGrounded = groundCheck.CheckIsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            _playerAudioSource.clip = JumpSound;
            _playerAudioSource.Play();
            IsGrounded = false;
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
            //rigid.velocity = new Vector2(rigid.velocity.x, JumpHeight);
        }

        if (!IsGrounded)
        {
            rigid.gravityScale += gravityJumpModifier * Time.deltaTime;
        }
        else
            rigid.gravityScale = defaultGravityValue;
    }
}
