using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applyTransforms : MonoBehaviour
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
        DoTransform();
    }

    void DoTransform(){
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time, displacement.y * Time.time, displacement.z * Time.time);

        Matrix4x4 moveOrigin = HW_Transforms.TranslationMat(-displacement.x , -displacement.y , -displacement.z );

        Matrix4x4 moveObject = HW_Transforms.TranslationMat(displacement.x , displacement.y , displacement.z );

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
