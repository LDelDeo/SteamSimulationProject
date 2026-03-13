using UnityEngine;
using UnityEngine.UI;

public class FaceManager : MonoBehaviour
{
    [Header("Generic Facial Feature Arrays")]
    public Sprite[] heads;
    public Sprite[] eyes;
    public Sprite[] mouths;
    public Sprite[] ears;
    public Sprite[] eyebrows;
    public Sprite[] noses;
    public Sprite[] glasses;

    [Header("Male Specific Arrays")]
    public Sprite[] maleHair;
    public Sprite[] facialHair;

    [Header("Female Specific Arrays")]
    public Sprite[] femaleHair;

    [Header("Face/Hair Colors")]
    public Color32[] skinTones; // This gets applied to head, ears, nose
    public Color32[] hairColor; // This gets applied to hair, facial hair, eyebrows
}
