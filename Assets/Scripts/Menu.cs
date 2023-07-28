using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Playground
{
    public class Menu : MonoBehaviour
    {
        [Tooltip("The panel used to show the menu")] [SerializeField]
        private GameObject _menuPanel;

        [Tooltip("The text used to show the high score")] [SerializeField]
        private TextMeshProUGUI _highScoreText;

        [Tooltip("Button used to start the game")] [SerializeField]
        private Button _playButton;

        /// <summary>
        /// Called when the user wants to start the round
        /// </summary>
        public event UnityAction RoundStarted;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(Play);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(Play);
        }

        /// <summary>
        /// Shows the menu panel
        /// </summary>
        /// <param name="highScore">The current high score</param>
        public void Show(int highScore = 0)
        {
            _highScoreText.text = highScore.ToString();
            _menuPanel.SetActive(true);
        }

        /// <summary>
        /// Hides the menu panel
        /// </summary>
        public void Hide()
        {
            _menuPanel.SetActive(false);
        }

        private void Play()
        {
            RoundStarted?.Invoke();
        }
    }
}