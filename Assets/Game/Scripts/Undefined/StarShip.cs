using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StarShip : MonoBehaviour
{
    public OrdersManager ordersManager;

    public float moveSpeed;
    public float rotationSpeed;


    protected virtual void Start()
    {
        ordersManager = gameObject.AddComponent<OrdersManager>();
    }
}
