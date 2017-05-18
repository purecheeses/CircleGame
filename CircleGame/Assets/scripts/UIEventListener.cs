using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIEventListener : MonoBehaviour, IPointerClickHandler
{
	public UIEventListener ()
	{
	}

	public delegate void UIEventProxy(PointerEventData eventData, GameObject go);

	public event UIEventProxy onClick;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null) {
			onClick (eventData, this.gameObject);
		}
	}
}


