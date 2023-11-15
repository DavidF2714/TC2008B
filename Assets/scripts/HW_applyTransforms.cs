using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HW_applyTransforms : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField] float angle;
    [SerializeField] AXIS rotationAxis;
    [SerializeField] GameObject WheelOriginal;

    private GameObject[] wheels; 


    Mesh mesh;
    Vector3[] baseVertices; //vertices originakes
    Vector3[] newVertices; //vertices nuevos

    // Start is called before the first frame update
    void Start()
    {

        Vector3[] wheelPositions = new Vector3[4]{
            new Vector3(-9.458f,0.372f,4.104f),
            new Vector3(-11.194f,0.372f,4.104f),
            new Vector3(-11.194f,0.372f,1.104f),
            new Vector3(-9.458f,0.372f,1.104f)
        };
        
        Vector3 wheelScale = new Vector3(0.39f,0.39f,0.39f); 

        wheels = new GameObject[4];
        for (int i = 0; i <4; i++){
            wheels[i] = Instantiate(WheelOriginal);
            Matrix4x4 scale = HW_Transforms.ScaleMat(wheelScale.x, wheelScale.y, wheelScale.z);
            Matrix4x4 moveObject = HW_Transforms.TranslationMat(wheelPositions[i].x , wheelPositions[i].y , wheelPositions[i].z );
            Matrix4x4 composite = moveObject * scale;

            Mesh mesh = wheels[i].GetComponentInChildren<MeshFilter>().mesh;
            baseVertices = mesh.vertices; //vertices originakes
            newVertices = new Vector3[baseVertices.Length]; //vertices nuevos

            for (int j=0; j < baseVertices.Length; j++){
                Vector4 temp = new Vector4(baseVertices[j].x, baseVertices[j].y, baseVertices[j].z, 1);
                newVertices[j] = composite * temp;
            }

            mesh.vertices = newVertices;
            mesh.RecalculateNormals();


        }


        mesh = GetComponentInChildren<MeshFilter>().mesh;
        baseVertices = mesh.vertices;

        //Allocate memory for the copy of the vertex list
        newVertices = new Vector3[baseVertices.Length];

        //Copy the coordinates
        for (int i = 0; i < baseVertices.Length; i++)
        {
            newVertices[i] = baseVertices[i];
        }


    }

    // Update is called once per frame
    void Update()
    {        
        DoTransform(mainObject : gameObject);
        foreach (GameObject wheel in wheels)
        {
            DoTransform(mainObject : wheel);
        }
        
    }

    void DoTransform(GameObject mainObject){
        Mesh mesh = mainObject.GetComponentInChildren<MeshFilter>().mesh;
        Vector3[] baseVertices = mesh.vertices; //vertices originakes
        Vector3[] newVertices = new Vector3[baseVertices.Length]; //vertices nuevos

        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time, displacement.y * Time.time, displacement.z * Time.time);
        Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis); //cuadritos x segundo time=tiempo acumulado
        Matrix4x4 composite = move * rotate;

        for (int i=0;i<baseVertices.Length;i++){
            Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);
            newVertices[i] = composite * temp;
        }
        //Assign  the new vertices to the mesh

        mesh.vertices = newVertices;
        mesh.RecalculateNormals();

    }

}
