using UnityEngine;

/*
 * To operate in conjunction with Dialogue Manager
 */
[CreateAssetMenu(fileName = "Data", menuName = "Dialogue", order = 1)]
public class SO_Dialogue : ScriptableObject
{
    public string convo;
    public bool oneOffWithAlternates = false;
    public AudioClip[] speaker1Clips;
    public AudioClip[] speaker2Clips;
}
