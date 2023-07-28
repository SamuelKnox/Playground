using UnityEngine;

namespace Playground
{
    [RequireComponent(typeof(Menu))]
    [RequireComponent(typeof(Gameplay))]
    public class GameManager : MonoBehaviour
    {
        private Menu _menu;
        private Gameplay _gameplay;
        private int _highScore;

        private void Awake()
        {
            _menu = GetComponent<Menu>();
            _gameplay = GetComponent<Gameplay>();
        }

        private void OnEnable()
        {
            _menu.RoundStarted += StartRound;
            _gameplay.RoundEnded += EndRound;
        }

        private void Start()
        {
            _gameplay.Hide();
            _menu.Show();
        }

        private void OnDisable()
        {
            _menu.RoundStarted -= StartRound;
            _gameplay.RoundEnded -= EndRound;
        }

        private void StartRound()
        {
            _menu.Hide();
            _gameplay.Show();
            _gameplay.StartRound();
        }

        private void EndRound(int score)
        {
            _gameplay.Hide();
            _highScore = Mathf.Max(score, _highScore);
            _menu.Show(_highScore);
        }
    }
}