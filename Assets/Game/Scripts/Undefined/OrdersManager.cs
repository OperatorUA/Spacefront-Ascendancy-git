using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    private List<BaseOrder> ordersQueue = new List<BaseOrder>();

    public void Update()
    {
        ProcessOrders();
    }

    private void ProcessOrders()
    {
        if (ordersQueue.Count > 0)
        {
            BaseOrder currentOrder = ordersQueue[0];
            currentOrder.ExecuteOrder();

            if (currentOrder.IsOrderCompleted())
            {
                Debug.Log("Order completed");
                ordersQueue.RemoveAt(0);
            }
        }
    }

    public void AddOrder(BaseOrder order)
    {
        ordersQueue.Add(order);
    }
}
