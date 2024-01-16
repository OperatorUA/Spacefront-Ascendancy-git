using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseOrder : MonoBehaviour
{
    public abstract void ExecuteOrder();
    public abstract bool IsOrderCompleted();
}
