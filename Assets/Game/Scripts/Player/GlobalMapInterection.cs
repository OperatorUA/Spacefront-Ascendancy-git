using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapInterection : MonoBehaviour
{
    public PrefabStorage iconPrefabStorage;


    private GameObject selectedIcon;

    Camera _camera;
    public void Awake()
    {
        _camera = Camera.main;
        iconPrefabStorage = ResourceLoader.Load<PrefabStorage>("IconPrefabStorage");
    }
    public void Update()
    {
        int LayerIndex = 6;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerIndex))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectStar(hit.transform.position);
            }
        }
    }

    private void SelectStar(Vector3 selectedObjectPosition)
    {
        if (selectedIcon != null) selectedIcon.transform.position = selectedObjectPosition;
        else {
            selectedIcon = Instantiate(iconPrefabStorage.GetByID("Icon_SelectedStar"), selectedObjectPosition, Quaternion.identity);
            selectedIcon.transform.SetParent(transform);
            InGameIcon iconComponent = selectedIcon.AddComponent<InGameIcon>();
            iconComponent.scalingComponent.iconSize = 0.065f;
            iconComponent.scalingComponent.scaleSpeed = 1.5f;
            iconComponent.rotationComponent.rotationSpeed = 55f;
        }
    }
}
