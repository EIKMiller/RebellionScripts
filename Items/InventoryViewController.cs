using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryViewController : MonoBehaviour
{
    public InventoryController _ViewingInventory;
    [SerializeField] private ItemView _ItemView;

    [SerializeField] private Transform _ItemsContrainer;
    [SerializeField] private GameObject _InventoryPrefab;

    private List<GameObject> _SpawnedItems = new List<GameObject>();

    private void OnEnabled()
    {
        ClearSpawned();
    }

    public void ResetInventory()
    {
        ClearSpawned();
        GetItems();
    }

    public void Setup(InventoryController inventory)
    {
        _ViewingInventory = inventory;
        ResetInventory();
        _ItemView.Clear();
    }

    private void ClearSpawned()
    {
        foreach(var item in _SpawnedItems)
            Destroy(item);

        _SpawnedItems.Clear();
    }

    private void GetItems()
    {
        foreach(var item in _ViewingInventory.Items)
        {
            Debug.Log(item.ItemName);
            GameObject invObject = GameObject.Instantiate(_InventoryPrefab);
            invObject.transform.SetParent(_ItemsContrainer);
            invObject.GetComponent<InventoryObject>()?.Setup(item, _ItemView);
            _SpawnedItems.Add(invObject);
        }
    }
}
