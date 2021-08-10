using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Dialogue", order = 1)]
public class SO_Dialogue : ScriptableObject
{
    public string name;
    public AudioClip[] audioClips;
}
