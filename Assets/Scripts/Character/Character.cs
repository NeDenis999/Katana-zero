using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _blackout;
    [SerializeField] private ParticleSystemRenderer _particleSystem;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isGrounded = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (Input.GetButtonDown("Fire3"))
        {
            TimeDilation();
        }

        if (!Input.GetButton("Fire3") && !Input.GetButton("Fire2"))
        {
            TimeNormal();
        }
    }

    private void Animation()
    {
        _animator.SetBool("TimeDilation", Input.GetButton("Fire3"));
    }

    private void TimeDilation()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.005f;
        _blackout.SetActive(true);
        _spriteRenderer.color = new Color(0f, 1f, 1f);
        _particleSystem.gameObject.SetActive(true);
    }

    private void TimeNormal()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        _blackout.SetActive(false);
        _spriteRenderer.color = new Color(1f, 1f, 1f);
        _particleSystem.gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Steps")
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Steps")
        {
            _isGrounded = false;
        }
    }
}
