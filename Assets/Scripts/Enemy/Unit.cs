using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected abstract void Damage();
    protected abstract void Dead();
}
