using UnityEngine;
using UnityEngine.InputSystem;

public class TargetManager : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject targetObject;
    public string targetTag;

    public LayerMask clickableLayers;

    public void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("mousePressed");
            UpdateTarget();
        }
    }
    public void UpdateTarget()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, clickableLayers))
        {
            targetPosition = hit.point;
            targetObject = hit.transform.gameObject;
            targetTag = hit.transform.tag;

            Debug.Log($"새로운 타겟 지정: {targetObject.name} ({targetTag}) / 위치: {targetPosition}");
        }
        else
        {
            ClearTarget();
        }
    }

    public void ClearTarget()
    {
        targetPosition = Vector3.zero;
        targetObject = null;
        targetTag = string.Empty;
        Debug.Log("[Target] 지정 취소됨");
    }
}