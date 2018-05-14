using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Mesh m;

    public int point_count = 500;

    public ParticleSystem ps;

    public float scale = 50f;

    void Start()
    {
        List<Vector3> sampled_points = new MeshSampler().sample(m, point_count);


        var particles = new ParticleSystem.Particle[point_count];
        ps.Emit(point_count);
        ps.GetParticles(particles);


        for(int i=0; i< particles.Length; i++)
        {
            particles[i].position = sampled_points[i] * scale;
            particles[i].startSize = 0.1f;
        }
        ps.SetParticles(particles, point_count);
    }

}
