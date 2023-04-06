using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private CharacterKick _characterKick;

    private void Awake()
    {
        _characterKick = GetComponentInParent<CharacterKick>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy _enemy))
        {
            _characterKick.StartCoroutine(_characterKick.SlashEnemy(collision.gameObject.GetComponent<Enemy>()));
            collision.GetComponent<Enemy>().Damage(gameObject.transform.localScale);
        }
    }
}
