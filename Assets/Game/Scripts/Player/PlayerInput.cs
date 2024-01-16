using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float borderRadius;
    public StarShip playerShip;
    void Start()
    {
        
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 centerOfCircle = Vector3.zero;
            float distance = Vector3.Distance(hit.point, centerOfCircle);
            if (distance < borderRadius)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    playerShip.ordersManager.AddOrder(new OrderMoveTo(playerShip.transform, hit.point));
                }
            }
        }
    }
}
