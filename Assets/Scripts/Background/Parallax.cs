using UnityEngine;

public class Parallax : MonoBehaviour
{
    Material mat;
    float distance;
    public float speed = 0.2f;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        distance += Time.deltaTime*speed;
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
}
