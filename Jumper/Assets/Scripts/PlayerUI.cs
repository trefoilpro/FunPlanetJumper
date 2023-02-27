using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
   [SerializeField] private Text _playerText;
    private Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
        _playerText.text = "Name";
    }
}
