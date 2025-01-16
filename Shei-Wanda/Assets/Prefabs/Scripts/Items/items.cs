using UnityEngine;

public abstract class Item : MonoBehaviour
{
 
    public string Name;
    public string Type;

    public abstract void Use();
}
