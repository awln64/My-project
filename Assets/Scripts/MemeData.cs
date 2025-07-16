using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MemeData", menuName = "Scriptable Objects/MemeData")]
public class MemeData : ScriptableObject {
    public string name;
    public Sprite image;
}
