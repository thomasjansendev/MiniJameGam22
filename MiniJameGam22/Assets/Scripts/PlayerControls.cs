using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace src.player
{
    public class PlayerControls : MonoBehaviour

    {
        private PlayerMovement _playerMovement;

        private void Start()
        {
            _playerMovement = transform.GetComponent<PlayerMovement>();
        }

        void OnMove(InputValue value)
        {
            _playerMovement.MovementInput = value.Get<Vector2>();
        }
        
        void OnQuit()
        {
            // def allows this to work in the editor or in regular game
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}