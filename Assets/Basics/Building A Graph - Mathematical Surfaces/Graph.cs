using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform m_Point;
    [Range(10,100)]
    public int m_Resolution = 50;
    Transform[] m_Points;
    public GraphFunctionName function;
    static GraphFunction[] functions =
    {
        SineFunction, MultiSineFunction, Sine2DFunction, MultiSine2DFunction, Ripple, Cylinder, Sphere, Torus
    };

    const float PI = Mathf.PI;

    // Start is called before the first frame update
    void Awake()
    {
        m_Points = new Transform[m_Resolution * m_Resolution];
        float step = 2.0f / m_Resolution;
        Vector3 scale = Vector3.one * step;

        for (int i = 0; i < m_Points.Length; i++)
        {
            Transform l_point = Instantiate(m_Point);
            l_point.localScale = scale;
            l_point.SetParent(transform);
            m_Points[i] = l_point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        float step = 2f / m_Resolution;
        for (int i = 0, z = 0; z < m_Resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < m_Resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                m_Points[i].localPosition = f(u, v, t);
            }
        }
    }

    static Vector3 SineFunction(float x, float z, float t)
    {
        Vector3 v;
        v.x = x;
        v.y = Mathf.Sin(PI * (x + t));
        v.z = z;
        return v;
    }

    static Vector3 MultiSineFunction(float x, float z, float t)
    {
        Vector3 v;
        v.x = x;
        float y = Mathf.Sin(PI * (x + t));
        y += Mathf.Sin(2.0f * PI * (x + t)) / 2.0f;
        y *= 2f / 3f;
        v.y = y;
        v.z = z;
        return v;
    }

    static Vector3 Sine2DFunction(float x, float z, float t)
    {
        Vector3 v;
        v.x = x;
        float y = Mathf.Sin(PI * (x + t));
        y += Mathf.Sin(PI * (z + t));
        y *= 0.5f;
        v.y = y;
        v.z = z;
        return v;
    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 v;
        v.x = x;
        float y =4.0f *  Mathf.Sin(PI * (x + z + (t * 0.5f)));
        y += Mathf.Sin(PI * (x + t));
        y += Mathf.Sin(2f * PI * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        v.y = y;
        v.z = z;
        return v;
    }

    static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 v;
        v.x = x;
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(PI * (d * 4.0f - t));
        y /= 1 + (10f * d);
        v.y = y;
        v.z = z;
        return v;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        float r = 0.8f + Mathf.Sin(PI * (6f * u + 2f * v + t)) * 0.2f;
        Vector3 p;
        p.x = r * Mathf.Sin( PI * u);
        p.y = v;
        p.z = r * Mathf.Cos(PI * u);
        return p;
    }

    static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.8f + Mathf.Sin(PI * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(PI * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(PI * v * 0.5f);
        Vector3 p;
        p.x = s * Mathf.Sin(PI * u);
        p.y = r *Mathf.Sin(PI * v * 0.5f);
        p.z = s * Mathf.Cos(PI * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.65f + Mathf.Sin(PI * (6f * u + t)) * 0.1f;
        float r2 = 0.2f + Mathf.Sin(PI * (4f * v + t)) * 0.05f;
        float s = r2*Mathf.Cos(PI * v) + r1;
        Vector3 p;
        p.x = s * Mathf.Sin(PI * u);
        p.y = r2 * Mathf.Sin(PI * v);
        p.z = s * Mathf.Cos(PI * u);
        return p;
    }
}
