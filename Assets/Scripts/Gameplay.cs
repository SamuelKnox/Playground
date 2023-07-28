using System;
using Niantic.Lightship.AR.Subsystems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARSubsystems;

namespace Playground
{
    public class Gameplay : MonoBehaviour
    {
        [Tooltip("The ARPersistentAnchorManager")] [SerializeField]
        private ARPersistentAnchorManager _arPersistentAnchorManager;

        [Tooltip("The payloads for the locations to switch between")] [SerializeField]
        private string[] _payloads;

        [Tooltip("The panel used to show the gameplay")] [SerializeField]
        private GameObject _gameplayPanel;

        [Tooltip("The text used to show the time elapsed")] [SerializeField]
        private TextMeshProUGUI _timeRemainingText;
        
        [Tooltip("The text used to show the player's score")] [SerializeField]
        private TextMeshProUGUI _scoreText;

        [Tooltip("How many minutes you have to walk between locations")] [SerializeField] [Range(1.0f, 10.0f)]
        private float _roundMinutes = 5.0f;

        /// <summary>
        /// Called when the round has ended
        /// </summary>
        public event UnityAction<int> RoundEnded;

        private ARPersistentAnchor _targetARPersistentAnchor;
        private TimeSpan _timeRemaining;
        private int _score;
        private int _targetLocationIndex;
        private bool _roundActive;

        private void OnEnable()
        {
            _arPersistentAnchorManager.arPersistentAnchorStateChanged += HandleARPersistentAnchorStateChanged;
        }

        private void Update()
        {
            if (!_roundActive)
            {
                return;
            }
            UpdateTimeRemaining();
            if (_timeRemaining <= TimeSpan.Zero)
            {
                EndRound();
            }
        }

        private void OnDisable()
        {
            _arPersistentAnchorManager.arPersistentAnchorStateChanged -= HandleARPersistentAnchorStateChanged;
        }

        /// <summary>
        /// Shows the gameplay panel
        /// </summary>
        public void Show()
        {
            _gameplayPanel.SetActive(true);
        }

        /// <summary>
        /// Hides the gameplay panel
        /// </summary>
        public void Hide()
        {
            _gameplayPanel.SetActive(false);
        }

        /// <summary>
        /// Starts the round
        /// </summary>
        public void StartRound()
        {
            _timeRemaining = TimeSpan.FromMinutes(0.1);
            var arPersistentAnchorPayload = new ARPersistentAnchorPayload(_payloads[_targetLocationIndex]);
            _arPersistentAnchorManager.TryTrackAnchor(arPersistentAnchorPayload, out _targetARPersistentAnchor);
            _gameplayPanel.SetActive(true);
            _roundActive = true;
        }

        private void EndRound()
        {
            _roundActive = false;
            _arPersistentAnchorManager.DestroyAnchor(_targetARPersistentAnchor);
            RoundEnded?.Invoke(_score);
        }

        private void HandleARPersistentAnchorStateChanged(ARPersistentAnchorStateChangedEventArgs arPersistentAnchorStateChangedEventArgs)
        {
            if (_targetARPersistentAnchor.trackingState == TrackingState.Tracking)
            {
                IncrementScore();
                ChangeTargetLocation();
            }
        }

        private void UpdateTimeRemaining()
        {
            _timeRemaining -= TimeSpan.FromSeconds(Time.deltaTime);
            _timeRemainingText.text = Mathf.CeilToInt((float)_timeRemaining.TotalSeconds).ToString();
        }

        private void IncrementScore()
        {
            _score++;
            _scoreText.text = _score.ToString();
        }

        private void ChangeTargetLocation()
        {
            _arPersistentAnchorManager.DestroyAnchor(_targetARPersistentAnchor);
            _targetLocationIndex = ++_targetLocationIndex % _payloads.Length;
            var arPersistentAnchorPayload = new ARPersistentAnchorPayload(_payloads[_targetLocationIndex]);
            _arPersistentAnchorManager.TryTrackAnchor(arPersistentAnchorPayload, out _targetARPersistentAnchor);
        }
    }
}