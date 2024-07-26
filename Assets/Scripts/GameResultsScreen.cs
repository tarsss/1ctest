using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResultsScreen : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Game _game;

    private void Awake()
    {
        gameObject.SetActive(false);

        _restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        _game.OnWin += () => Show("Победа");
        _game.OnLose += () => Show("Поражение");
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);
        _text.text = text;
    }
}
