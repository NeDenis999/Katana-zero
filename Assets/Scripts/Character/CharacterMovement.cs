using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private int _horisontalStep;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxSpeed = 500f;
    [SerializeField] private float _jumpForce = 1f;

    private float _horizontal = 0f;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isGrounded = true;
    private bool _slip = false;
    private bool _immortality = false;
    private Vector2 _stepsDirection = new Vector2(1, 1);
    private bool _flip;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Inputing();
        Animation();
    }

    private void Inputing()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Horizontal") && _immortality) StartCoroutine(FlipEnd());

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            _rigidbody.AddForce(new Vector2(0, _speed * -1 / 50f), ForceMode2D.Force);
        }
    }

    private void Animation()
    {
        _animator.SetFloat("VSpeed", _rigidbody.velocity.y);
        _animator.SetFloat("HSpeed", Mathf.Abs(_horizontal));
        _animator.SetBool("IsGronded", _isGrounded);
        _animator.SetBool("Crouch", Input.GetAxisRaw("Vertical") < 0 && _isGrounded && !Input.GetButton("Horizontal"));
        _animator.SetBool("Slip", _slip);

        if (Input.GetButton("Horizontal"))
        {
            transform.localScale = new Vector3(_horizontal, 1, 1);
        }

        if (Input.GetAxisRaw("Vertical") < 0 && Input.GetButtonDown("Horizontal") && !_flip && _isGrounded || Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0 && Input.GetButton("Horizontal") && !_flip && _isGrounded)
        {
            _animator.SetTrigger("Flip");
        }
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        if (Mathf.Abs(_rigidbody.velocity.x) < _maxSpeed)
        {
            _rigidbody.AddForce(new Vector2(_horizontal * _speed, 0), ForceMode2D.Force);
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.mass = 75;
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        _flip = true;
        _immortality = true;
        _rigidbody.AddForce(new Vector2(_horizontal * _speed / 3, 0), ForceMode2D.Impulse);
    }

    private IEnumerator FlipEnd()
    {
        _immortality = false;
        yield return new WaitForSeconds(0.075f);
        _rigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        _flip = false;
    }

    private void SlidingOnWalls()
    {
        _slip = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
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
