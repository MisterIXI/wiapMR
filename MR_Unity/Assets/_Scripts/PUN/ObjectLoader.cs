// taken from: https://gist.github.com/supachailllpay/893cd5b0c31dff3bb025
using System.Collections.Generic;
using UnityEngine;

namespace WiapMR.PUN
{
    public class ObjectLoader : MonoBehaviour
    {
        private bool _isLoaded;
        void Awake()
        {
            _isLoaded = true;
        }

        public void Load(string[] fileContent)
        {
            if (!_isLoaded)
                return;

            ConstructModel(fileContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileContent">File content of a .obj file read with File.ReadAllLines(path);</param>
        void ConstructModel(string[] fileContent)
        {
            _isLoaded = false;
            FileReader.ObjectFile obj = FileReader.ReadObjectFile(fileContent);
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            filter.mesh = PopulateMesh(obj);
            _isLoaded = true;
        }

        Mesh PopulateMesh(FileReader.ObjectFile obj)
        {
            Mesh mesh = new Mesh();
            List<int[]> triplets = new List<int[]>();
            List<int> submeshes = new List<int>();
            for (int i = 0; i < obj.f.Count; i += 1)
            {
                for (int j = 0; j < obj.f[i].Count; j += 1)
                {
                    triplets.Add(obj.f[i][j]);
                }
                submeshes.Add(obj.f[i].Count);
            }
            Vector3[] vertices = new Vector3[triplets.Count];
            Vector3[] normals = new Vector3[triplets.Count];
            Vector2[] uvs = new Vector2[triplets.Count];
            for (int i = 0; i < triplets.Count; i += 1)
            {
                vertices[i] = obj.v[triplets[i][0] - 1];
                normals[i] = obj.vn[triplets[i][2] - 1];
                if (triplets[i][1] > 0)
                    uvs[i] = obj.vt[triplets[i][1] - 1];
            }
            mesh.name = obj.o;
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.subMeshCount = submeshes.Count;

            int vertex = 0;
            for (int i = 0; i < submeshes.Count; i += 1)
            {
                int[] triangles = new int[submeshes[i]];
                for (int j = 0; j < submeshes[i]; j += 1)
                {
                    triangles[j] = vertex;
                    vertex += 1;
                }
                mesh.SetTriangles(triangles, i);
            }
            mesh.RecalculateBounds();
            mesh.Optimize();
            return mesh;
        }
    }
}
