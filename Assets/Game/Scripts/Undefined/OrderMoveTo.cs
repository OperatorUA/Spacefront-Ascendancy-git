using TMPro;
using UnityEngine;

public class OrderMoveTo : BaseOrder
{
    public Vector3 targetPosition;
    public Transform unitTransform;
    public StarShip unitStarship;
    public override void ExecuteOrder()
    {
        Debug.Log("Moving to " + targetPosition);
        unitTransform.position = Vector3.MoveTowards(unitTransform.position, targetPosition, Time.deltaTime * unitStarship.moveSpeed);

        Vector3 direction = (targetPosition - unitTransform.position).normalized;
        // Rotate towards the target
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        unitTransform.rotation = Quaternion.Lerp(unitTransform.rotation, targetRotation, Time.deltaTime * unitStarship.rotationSpeed);
    }

    public override bool IsOrderCompleted()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return true;
        return Vector3.Distance(unitTransform.position, targetPosition) < 0.1f;
    }

    public OrderMoveTo(Transform unitTransform, Vector3 targetPosition)
    {
        this.unitTransform = unitTransform;
        this.targetPosition = targetPosition;
        unitStarship = unitTransform.gameObject.GetComponent<StarShip>();
    }
}
