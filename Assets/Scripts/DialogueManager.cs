using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance => _instance;

    [SerializeField] SO_Dialogue testDialogue;
    [SerializeField] float _convoGap = .5f;

    static DialogueManager _instance;

    Dictionary<SO_Dialogue, int> _lastStarted = new Dictionary<SO_Dialogue, int>();
    Dictionary<SO_Dialogue, CircularQueue<AudioClip>> _oneOffs = new Dictionary<SO_Dialogue, CircularQueue<AudioClip>>();

    /*
     * Plays the dialogue specified, invokes optional callback when complete
     * Returns SFX ID
     */
    public int PlayDialogue(SO_Dialogue dialogue, System.Action callback = null) {
        int sfxID;
        if (dialogue.oneOffWithAlternates)
        {
            sfxID = PlayOneOff(dialogue);
        }
        else {
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
        }
        return sfxID;
    }

    /*
     * Stops the dialogue with given ID
     * Returns true if dialogue was found & stopped
     */
    public bool StopDialogue(int id)
    {
        return SFXManager.Instance.StopSequentialSFX(id);
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

    /*
     * Stiches together dialogue between speaker1 and speaker2
     * Returns resulting array of audioclips
     */
    AudioClip[] StitchClips(SO_Dialogue dialogue, int startingSpeaker) {
        int length = dialogue.speaker1Clips.Length;
        int speaker = startingSpeaker %= 2;
        AudioClip[][] speakerClips = new AudioClip[2][] {
            dialogue.speaker1Clips,
            dialogue.speaker2Clips
        };
        AudioClip[] result = new AudioClip[length];
        for (int n = 0; n < length; n++) {
            result[n] = speakerClips[speaker][n];
            speaker = (speaker + 1) % 2;
        }
        return result;
    }

    /*
     * Plays random audioclip from dialogue (for one-offs w/ alternates)
     * Returns SFX ID
     */
    int PlayOneOff(SO_Dialogue dialogue) {
        CircularQueue<AudioClip> cq;
        if (_oneOffs.ContainsKey(dialogue))
        {
            cq = _oneOffs[dialogue];
        }
        else {
            cq = new CircularQueue<AudioClip>();
            cq.Add(dialogue.speaker1Clips);
            cq.Add(dialogue.speaker2Clips);
            cq.Shuffle();
            _oneOffs[dialogue] = cq;
        }
        return SFXManager.Instance.PlaySFX(cq.Next());
    }
}
