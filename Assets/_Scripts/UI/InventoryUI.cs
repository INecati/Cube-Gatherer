using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text storageCubeCountTxt;
    [SerializeField] private TMP_Text playerCubeCountTxt;
    [SerializeField] private CubeGathererBase player;

    [SerializeField] private string storageCubeCountStr;
    [SerializeField] private string playerCubeCountStr;

    void Update()
    {
        storageCubeCountTxt.SetText(string.Format(storageCubeCountStr, CubeStorage.Instance.CubeCount));
        playerCubeCountTxt.SetText(string.Format(playerCubeCountStr, player.CubeCount));
    }
}
