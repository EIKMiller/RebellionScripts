using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private PlayerController _Player;

    [Header("Slot Elements")]
    [SerializeField] private HUDWeaponSlot _HudSlotOne;
    [SerializeField] private HUDWeaponSlot _HudSlotTwo;

    [Header("Action Points Elements")]
    [SerializeField] private TextMeshProUGUI _ApText;

    private void Awake()
    {
        if(!_Player)
            _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SetSlotOne(BaseItem item)
    {
        if(_HudSlotOne)
            _HudSlotOne.SetItem(item);
    }

    public void SetSlotTwo(BaseItem item)
    {
        if(_HudSlotTwo)
            _HudSlotTwo.SetItem(item);
    }

    public void UpdateAp()
    {
        if(!_Player)
            return;

        int maxAP = _Player.MaxActionPoints;
        int currentAP = _Player.CurrentActionPoints;

        _ApText.text = $"{currentAP}/{maxAP}";
    }
}
