using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour {
    
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool _playerInRange;

    private void Awake() {
        _playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if(_playerInRange)
        {
            visualCue.SetActive(true);
            
            if (Keyboard.current.eKey.wasPressedThisFrame) // check if "E" key was pressed
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Player")) { // check if the collider has the "Player" tag
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Player")) { // check if the collider has the "Player" tag
            _playerInRange = false;
        }
    }
}