using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseDown, OnMouseHold, OnMouseHover;
    public Action OnMouseUp;

	[SerializeField]
	Camera mainCamera;

	public LayerMask groundMask;

	private void Update()
	{
		CheckClickDownEvent();
		CheckClickUpEvent();
		CheckClickHoldEvent();
		CheckMouseHoverEvent();
	}

	private Vector3Int? RaycastGround()
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
		{
			Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
			return positionInt;
		}
		return null;
	}

	private void CheckClickHoldEvent()
	{
		if(Input.GetMouseButton(1) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			var position = RaycastGround();
			if (position != null)
            {
				OnMouseHold?.Invoke(position.Value);
			}
		}
	}

	private void CheckClickUpEvent()
	{
		if (Input.GetMouseButtonUp(1) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			OnMouseUp?.Invoke();
		}
	}

	private void CheckClickDownEvent()
	{
		if (Input.GetMouseButtonDown(1) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			var position = RaycastGround();
			if (position != null)
            {
				OnMouseDown?.Invoke(position.Value);
			}
		}
	}

	private void CheckMouseHoverEvent()
    {
		/*var position = RaycastGround();
		if (position != null)
		{
			OnMouseHover?.Invoke(position.Value);
		}*/
		if (EventSystem.current.IsPointerOverGameObject() == false)
        {
			var position = RaycastGround();
			if (position != null)
			{
				OnMouseHover?.Invoke(position.Value);
			}
		}
    }
}
