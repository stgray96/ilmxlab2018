/* Stewart Gray III - CELL Project
 * January 6, 2017
 * Script for animating the translucent wave above the hexagonal meshes
 */
 using UnityEngine;
 using System.Collections;
 
 public class WaveGen : MonoBehaviour
 {
 	 private AudioAnalyzer listener;
	 [Range(0.1f, 10f)] 	
     public float scale = 0.1f;
     [Range(1, 5)]
     public float speed = 1.0f;
     public float noiseStrength = 1f;
     public float noiseWalk = 1f;
 
     private Vector3[] baseHeight;

     /* Allow this script to recieve input from the current audio stream */
     void Start(){
     	listener= gameObject.GetComponent<AudioAnalyzer>();
     }

     /* Update the positions of the verticies within the mesh each frame to create an oscillating wave */
     void Update () {
         // Update the mesh that already exists within the scene (ie: a planar mesh)
         Mesh mesh = GetComponent<MeshFilter>().mesh;
         if (baseHeight == null)
             baseHeight = mesh.vertices;
   		
         Vector3[] vertices = new Vector3[baseHeight.Length];
         for (int i=0;i<vertices.Length;i++){
             Vector3 vertex = baseHeight[i];
             vertex.y += 
                (0.25f*Mathf.Sin(4f*Mathf.PI*baseHeight[i].x+4f*Time.time)
                *Mathf.Sin(2f * Mathf.PI * baseHeight[i].z + Time.time)
                +0.10f * Mathf.Cos(3f * Mathf.PI * baseHeight[i].x + 5f * Time.time) * Mathf.Cos(5f * Mathf.PI *baseHeight[i].z + 3f * Time.time)
                +0.15f * Mathf.Sin(Mathf.PI * baseHeight[i].x + 0.6f * Time.time))*(scale+1.25f)*(speed); 
             vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Cos(Time.time * 0.1f)) * noiseStrength;
             vertices[i] = vertex;            
            
         }
         // Flush the updated verticies back to the mesh, ensure their normals are facing north, and display the results
         mesh.vertices = vertices;
         mesh.RecalculateNormals();


     }
 }
