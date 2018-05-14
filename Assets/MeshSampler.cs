using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSampler{



    public List<Vector3> sample(Mesh m, int point_count) 
    {
        int index_count = (int)m.GetIndexCount(0);

        List<Vector3> vertices = new List<Vector3>(m.vertexCount);
        m.GetVertices(vertices);

        List<int> mesh_indices = new List<int>(index_count);
        m.GetIndices(mesh_indices, 0);

        List<float> triangle_areas = new List<float>();


        float min_area = Mathf.Infinity;
        for(int i=0; i< index_count; i+=3)
        {
            Vector3 v1 = vertices[mesh_indices[i+0]];
            Vector3 v2 = vertices[mesh_indices[i+1]];
            Vector3 v3 = vertices[mesh_indices[i+2]];

            float area = get_triangle_area (v1, v2, v3);
            min_area = Mathf.Min(area, min_area);
            triangle_areas.Add(area);
        }

        List<int> extended_triangle_indices = new List<int>();

        for(int i=0; i< triangle_areas.Count; i++)
        {
            triangle_areas[i] /= min_area;
            int indices_needed = Mathf.RoundToInt(triangle_areas[i]);

            for(int j=0; j< indices_needed; j++)
            {
                extended_triangle_indices.Add(i);
            }
        }

        List<int> selected_triangle_indices = new List<int>();

        for(int i=0; i< point_count; i++)
        {
            int selected_triangle_index = extended_triangle_indices[Mathf.FloorToInt(Random.Range(0f, 1f) * (extended_triangle_indices.Count-1))];

            selected_triangle_indices.Add(selected_triangle_index);
        }


        List<Vector3> sampled_points = new List<Vector3>();

        for(int i=0; i< point_count; i++)
        {
            int triangle_index = selected_triangle_indices[i];

            Vector3 v1 = vertices[mesh_indices[triangle_index * 3 + 0]];
            Vector3 v2 = vertices[mesh_indices[triangle_index * 3 + 1]];
            Vector3 v3 = vertices[mesh_indices[triangle_index * 3 + 2]];
            float w1 = Random.Range(0f, 1f);
            float w2 = Random.Range(0f, 1f);

            sampled_points.Add(get_barycentric_point(w1, w2, v1, v2, v3));



        }
        return sampled_points;

    }

    float get_triangle_area(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return Vector3.Cross(v2 - v1, v3 - v1).magnitude/2f;
    }
    Vector3 get_barycentric_point(float w1,float w2, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        if(w1+w2 > 1f)
        {
            w1 = 1f - w1;
            w2 = 1f - w2;
        }

        float w3 = 1f - (w1+w2);

        return v1 * w1 + v2 * w2 + v3 * w3;
    }

}
