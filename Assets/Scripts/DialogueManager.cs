using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Working in conjunction with SO_Dialogue and SFX Manager to play dialogues
 * where initial speaker is randomized
 */
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance => _instance;

    [SerializeField] SO_Dialogue testDialogue;
    [SerializeField] float _convoGap = .5f;

    static DialogueManager _instance;

    Dictionary<SO_Dialogue, int> _lastStarted = new Dictionary<SO_Dialogue, int>();
    Dictionary<SO_Dialogue, CircularQueue<AudioClip>> _oneOffs = new Dictionary<SO_Dialogue, CircularQueue<AudioClip>>();

    const string DATA_PATH = "Dialogue/";

    int _currentDialogueID;
    int _currentOneOffID;

    /*
     * Loads the dialogue specified by name and plays it, invokes optional callback when complete
     * Returns SFX ID
     */
    public int PlayDialogue(string dialogueName, bool interrupt = true, System.Action callback = null)
    {
        SO_Dialogue dialogue = Resources.Load<SO_Dialogue>($"{DATA_PATH}{dialogueName}");
        if (interrupt)
        {
            StopAll();
        }
        return PlayDialogue(dialogue, callback);
    }

    /*
     * Stops dialogue
     */
    public void StopAll()
    {
        StopDialogue(_currentDialogueID);
        StopOneOff(_currentOneOffID);
    }

    /*
     * Plays the dialogue specified, invokes optional callback when complete
     * Returns SFX ID
     */
    public int PlayDialogue(SO_Dialogue dialogue, System.Action callback = null)
    {
        int sfxID;
        if (dialogue.oneOffWithAlternates)
        {
            sfxID = PlayOneOff(dialogue, callback);
            _currentOneOffID = sfxID;
        }
        else
        {
            //Ensures that new speaker begins dialogue each time
            if (_lastStarted.ContainsKey(dialogue))
            {
                _lastStarted[dialogue] = (_lastStarted[dialogue] + 1) % 2;
            }
            else
            {
                _lastStarted[dialogue] = Random.Range(0, 2);
            }
            AudioClip[] clips = StitchClips(dialogue, _lastStarted[dialogue]);
            sfxID = SFXManager.Instance.PlaySequentialSFX(clips, _convoGap, callback);
            _currentDialogueID = sfxID;
        }
        return sfxID;
    }

    /*
     * Returns total length of a dialogue in seconds
     * If one-off with alternates, returns average length
     */
    public float GetDialogueLength(SO_Dialogue dialogue)
    {
        float sumTotal = dialogue.speaker1Clips.Select(x => x.length).Sum();
        if (dialogue.oneOffWithAlternates)
        {
            return sumTotal / dialogue.speaker1Clips.Length;
        }
        return sumTotal;
    }

    /*
     * Stops the dialogue with given ID
     * Returns true if dialogue was found & stopped
     */
    public bool StopDialogue(int id)
    {
        return SFXManager.Instance.StopSequentialSFX(id);
    }

    public void StopOneOff(int id)
    {
        SFXManager.Instance.StopSFX(id);
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void PlayTestDialogue(int times)
    {
        StartCoroutine(PlayTestDialogueNTimes(testDialogue, times));
    }

    IEnumerator PlayTestDialogueNTimes(SO_Dialogue dialogue, int total)
    {
        float dialogueLength = GetDialogueLength(dialogue);
        for (int n = 0; n < total; n++)
        {
            PlayDialogue(dialogue);
            yield return new WaitForSeconds(dialogueLength + 1f);
        }
    }

    /*
     * Stiches together dialogue between speaker1 and speaker2
     * Returns resulting array of audioclips
     */
    AudioClip[] StitchClips(SO_Dialogue dialogue, int startingSpeaker)
    {
        int length = dialogue.speaker1Clips.Length;
        int speaker = startingSpeaker %= 2;
        AudioClip[][] speakerClips = new AudioClip[2][] {
            dialogue.speaker1Clips,
            dialogue.speaker2Clips
        };
        AudioClip[] result = new AudioClip[length];
        for (int n = 0; n < length; n++)
        {
            result[n] = speakerClips[speaker][n];
            speaker = (speaker + 1) % 2;
        }
        return result;
    }

    /*
     * Plays random audioclip from dialogue (for one-offs w/ alternates)
     * Returns SFX ID
     */
    int PlayOneOff(SO_Dialogue dialogue, System.Action callback = null)
    {
        CircularQueue<AudioClip> cq;
        if (_oneOffs.ContainsKey(dialogue))
        {
            cq = _oneOffs[dialogue];
        }
        else
        {
            cq = new CircularQueue<AudioClip>();
            cq.Add(dialogue.speaker1Clips);
            cq.Add(dialogue.speaker2Clips);
            cq.Shuffle();
            _oneOffs[dialogue] = cq;
        }
        return SFXManager.Instance.PlaySFX(cq.Next(), callback: callback);
    }
}
