using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 
[DisallowMultipleComponent]
public class Script_TMProWordShaky : Script_TMProBehavior
{
    [SerializeField] private Vector2 jitterXRange;
    [SerializeField] private Vector2 jitterYRange;
    
    private TMP_Text textMesh;
    private Mesh mesh;
 
    private Vector3[] vertices;
 
    private List<int> wordIndexes;
    private List<int> wordLengths;
 
    void Start()
    {
        try
        {
            textMesh = GetComponent<TMP_Text>();
    
            wordIndexes = new List<int>{0};
            wordLengths = new List<int>();
    
            string s = textMesh.text;
            for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
            {
                wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
                wordIndexes.Add(index + 1);
            }
            wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
        }
        catch (System.Exception e)
        {
            Debug.Log($"{name} <b>Failed to init with following error:</b> {e}");
        }
    }
 
    void Update()
    {
        try
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;
    
            for (int w = 0; w < wordIndexes.Count; w++)
            {
                int wordIndex = wordIndexes[w];
                Vector3 offset = ShakeOffset();
    
                for (int i = 0; i < wordLengths[w]; i++)
                {
                    TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];
    
                    int index = c.vertexIndex;
    
                    vertices[index] += offset;
                    vertices[index + 1] += offset;
                    vertices[index + 2] += offset;
                    vertices[index + 3] += offset;
                }
            }
    
            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }
        catch (System.Exception e)
        {
            Debug.Log($"{name} <b>Failed to update with following error:</b> {e}");
        }
    }
 
    private Vector3 ShakeOffset() {
        return new Vector3(
            Random.Range(jitterXRange.x, jitterXRange.y),
            Random.Range(jitterYRange.x, jitterYRange.y),
            0f
        );
    }
}