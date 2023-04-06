using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private int _lives = 1;

    private bool _isGrounded = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.timeScale < 1)
        {
            _spriteRenderer.color = new Color(0, 1, 1);
        }
        else
        {
            _spriteRenderer.color = new Color(1, 1, 1);
        }

        Animation();
    }

    public void Damage(Vector3 direction)
    {
        Dead(direction);
        _lives--;
    }

    public void Dead(Vector3 direction)
    {
        _rigidbody2D.AddForce(direction * 1000, ForceMode2D.Impulse);

        if (direction.y > 0)
        {
            _isGrounded = false;
        }

        _animator.SetTrigger("Hurt");

        _lives--;
    }

    private void Animation()
    {
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetInteger("Lives", _lives);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _isGrounded = false;
        }
    }
}
