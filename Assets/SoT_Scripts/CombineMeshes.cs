using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoT.Classes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class CombineMeshes : MonoBehaviour
    {
        [SerializeField]
        List<MeshFilter> _meshesToCombine;

        private void Start()
        {
            CombineInstance[] combinedMeshes = new CombineInstance[_meshesToCombine.Count];

            int i = 0;
            while (i < _meshesToCombine.Count)
            {
                combinedMeshes[i].mesh = _meshesToCombine[i].sharedMesh;
                combinedMeshes[i].transform = _meshesToCombine[i].transform.localToWorldMatrix;
                _meshesToCombine[i].gameObject.SetActive(false);

                i++;
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combinedMeshes);
            transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            transform.gameObject.SetActive(true);
        }
    }
}
