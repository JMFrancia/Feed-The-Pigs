using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    const string DIALOGUE_PATH = "Dialogue/";

    public static DialogueManager Instance => _instance;

    [SerializeField] SO_Dialogue testDialogue;

    static DialogueManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public int PlayDialogue(SO_Dialogue dialogue) {
        return SFXManager.Instance.PlaySequentialSFX(dialogue.audioClips, 1f, () => Debug.Log("Dialogue complete"));
    }

    public bool StopDialogue(int id) {
        return SFXManager.Instance.StopSequentialSFX(id);
    }

    IEnumerator PlayDialogueForSeconds(SO_Dialogue dialogue, float seconds) {
        int id = PlayDialogue(dialogue);
        yield return new WaitForSeconds(seconds);
        Debug.Log("Stopping dialogue now");
        StopDialogue(id);
    }
}
