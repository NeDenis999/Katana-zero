using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKick : MonoBehaviour
{
    [SerializeField] private GameObject _hit;
    [SerializeField] private GameObject _dash;
    [SerializeField] private GameObject _dashFinaly;
    [SerializeField] private GameObject _dashFinalyPosition;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private LineRenderer _line1;

    private Vector3 mousePosition;
    private float angle;
    private Vector2 toPoint;
    private bool _kick = false;
    private bool _slashAir = false;
    private bool _kickReload = false;
    private bool _kickGlitch = false;
    private bool _dashEnd = false;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isGrounded = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Inputing()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            _dash.SetActive(true);
            //TimeDilation();
        }

        if (Input.GetButton("Fire2"))
        {
            Dash();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            _dash.SetActive(false);
            _dashEnd = true;
        }
    }

    private void Animation()
    {
        if (Input.GetButtonDown("Fire1") && !_kickReload && !_dash.activeSelf) _animator.SetTrigger("Attack");

        //_particleSystem.flip = _spriteRenderer.flipX ? new Vector2(1, 0) : new Vector2(-1, 0);
        //_particleSystem.material.SetTexture("_MainTex", _spriteRenderer.sprite.texture);

        if (Input.GetButtonUp("Fire2"))
        {
            _animator.SetTrigger("Dash");
            _animator.SetBool("KickGlitch", _kickGlitch);
        }
    }

    private void Dash()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //положение мыши из экранных в мировые координаты
        float angle = Vector2.Angle(Vector2.right, mousePosition - _dashFinalyPosition.transform.position);//угол между вектором от объекта к мыше и осью х

        _line.SetPosition(0, new Vector2(transform.position.x, transform.position.y + 0.5f));//0-начальная точка линии


        if (Mathf.Abs((mousePosition.x) - (transform.position.x) + Mathf.Abs((mousePosition.y) - (transform.position.y))) < 7)
        {

            _dashFinaly.transform.position = new Vector2(mousePosition.x, mousePosition.y);
            _line.SetPosition(1, new Vector2(mousePosition.x, mousePosition.y));//1-конечная точка линии
        }
        else
        {
            _dashFinaly.transform.localPosition = new Vector2(5.74f, -0.18f);
            _dashFinalyPosition.transform.rotation = Quaternion.Euler(0f, 0f, _dashFinalyPosition.transform.position.y < mousePosition.y ? angle : -angle);
            _dashFinaly.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _line.SetPosition(1, new Vector2(_dashFinaly.transform.position.x, _dashFinaly.transform.position.y));//1-конечная точка линии
        }
    }

    private void DashEnd()
    {
        //_rigidbody.AddForce(new Vector2(_dashFinaly.transform.localPosition.x * _speed / 3, _dashFinaly.transform.localPosition.y * _speed / 3), ForceMode2D.Impulse);
        //_rigidbody.AddForce = (Vector3.Lerp(transform.position, _dashFinaly.transform.position, 1), ForceMode2D.Impulse);
        _rigidbody.MovePosition(Vector3.Lerp(transform.position, _dashFinaly.transform.position, 1));

        //_spriteRenderer.flipX = (_dashFinaly.transform.position.x) < transform.position.x;
    }

    private void SlashStart()
    {
        if (_kickReload)
        {
            _kickGlitch = true;
            _hit.SetActive(false);
        }

        _rigidbody.mass = 75;

        _kickReload = true;
        _kick = true;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //положение мыши из экранных в мировые координаты
        angle = Vector2.Angle(Vector2.right, mousePosition - _hit.transform.position);//угол между вектором от объекта к мыше и осью х

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            _hit.transform.eulerAngles = new Vector3(0f, 0f, _hit.transform.position.y < mousePosition.y ? angle : -angle);
        }
        else
        {
            _hit.transform.eulerAngles = new Vector3(0f, 0f, _hit.transform.position.y < mousePosition.y ? angle - 180.0f : -angle - 180.0f);
        }
    }

    private void Slash()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //положение мыши из экранных в мировые координаты
        angle = Vector2.Angle(Vector2.right, mousePosition - _hit.transform.position);//угол между вектором от объекта к мыше и осью х

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            _hit.GetComponent<SpriteRenderer>().flipX = false;
            _hit.transform.eulerAngles = new Vector3(0f, 0f, _hit.transform.position.y < mousePosition.y ? angle : -angle);
            _line1.transform.eulerAngles = new Vector3(0f, 0f, _hit.transform.position.y < mousePosition.y ? angle : -angle);
        }
        else
        {
            _hit.GetComponent<SpriteRenderer>().flipX = true;
            _hit.transform.eulerAngles = new Vector3(0f, 0f, _hit.transform.position.y < mousePosition.y ? angle - 180.0f : -angle - 180.0f);
            _line1.transform.eulerAngles = new Vector3(0f, 0f, _hit.transform.position.y < mousePosition.y ? angle - 180.0f : -angle - 180.0f);
        }

        _rigidbody.velocity = new Vector2(0f, 0f);

        toPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        toPoint = new Vector2(toPoint.x / (Mathf.Abs(toPoint.x) + Mathf.Abs(toPoint.y)), toPoint.y / (Mathf.Abs(toPoint.x) + Mathf.Abs(toPoint.y)));

        if (!_dashEnd)
        {
            if (_isGrounded)
            {
                _rigidbody.AddForce(toPoint * 500f, ForceMode2D.Impulse);
            }
            else if (!_slashAir)
            {
                _slashAir = true;
                _rigidbody.AddForce(toPoint * 500f, ForceMode2D.Impulse);
            }
            else
            {
                _rigidbody.AddForce(new Vector2(toPoint.x * 500f, toPoint.y * 200f), ForceMode2D.Impulse);
            }
        }

        _dashEnd = false;
    }

    private void SlashInaction()
    {
        _kick = false;
    }

    private IEnumerator SlashEnd()
    {
        yield return new WaitForSeconds(0.2f);
        _kickReload = false;
        _kickGlitch = false;
    }

    public IEnumerator SlashEnemy(Enemy enemy)
    {
        _line1.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.07f);
        _line1.gameObject.SetActive(false);
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
