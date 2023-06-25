using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BaseItem _OwningItem;
    public BaseItem OwningItem { get => _OwningItem; }

    private ItemView _View;

    [SerializeField] private Image _ItemIcon;
    [SerializeField] private TextMeshProUGUI _ItemName;

    private bool _CanClick = false;

    public void Setup(BaseItem item, ItemView view)
    {
        _OwningItem = item;
        _ItemIcon.sprite = item.ItemIcon;
        _ItemName.text = item.ItemName;
        _View = view;
    }

    private void Update()
    {
        if(_CanClick)
        {
            if(Input.GetMouseButton(0))
            {
                OnObjectPressed();
            }
        }
    }

    public void OnObjectPressed()
    {
        _View.SetItem(_OwningItem);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Hello World");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _CanClick = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _CanClick = false;
    }
}
