
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ParallaxControllerScript : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;
    float farthestBack;
    
    [SerializeField] public float ParallaxSpeed;

    void Start()
    {
        cam =Camera.main.transform;
        camStartPos = cam.position;
        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];
        for(int i = 0; i < backCount; i++){
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount){
        for(int i = 0; i < backCount; i++){
            if((backgrounds[i].transform.position.z - cam.position.z)>farthestBack){
                farthestBack = backgrounds[i].transform.position.z-cam.position.z;
            }
        }

        for (int i=0; i<backCount;i++){
            backSpeed[i]=1-(backgrounds[i].transform.position.z-cam.position.z) / farthestBack;
        }

    }

    void LateUpdate()
    { 
        distance = cam.position.x - camStartPos.x;
        transform.position= new UnityEngine.Vector3(cam.position.x+2, cam.position.y, 0);
        for(int i =0; i < backgrounds.Length;i++){
            float speed = backSpeed[i]*ParallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new UnityEngine.Vector2(distance, 0)*speed);
        }
    }

}
