using UnityEngine;

public class Effect : MonoBehaviour
{
    public void EndAnim()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
