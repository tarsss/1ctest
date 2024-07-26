using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _player.OnHealthChange += UpdateText;
        UpdateText(_player.Health);
    }

    private void UpdateText(float value)
    {
        _text.text = $"Health: {value}";
    }
}