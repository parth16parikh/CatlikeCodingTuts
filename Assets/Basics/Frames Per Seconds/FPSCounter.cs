using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public int m_AverageFPS { get; private set; }
    public int m_HighestFPS { get; private set; }
    public int m_LowestFPS { get; private set; }
    public int m_FramesRange = 60;

    private int[] m_FPSBuffer;
    private int m_FPSBufferIndex;

    void InitializeBuffer()
    {
        if(m_FramesRange <= 0)
        {
            m_FramesRange = 1;
        }
        m_FPSBuffer = new int[m_FramesRange];
        m_FPSBufferIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_FPSBuffer == null || m_FPSBuffer.Length != m_FramesRange)
        {
            InitializeBuffer();
        }
        UpdateFrameBuffer();
        CalculateAverageFPS();
    }

    private void CalculateAverageFPS()
    {
        int sum = 0;
        int highest = int.MinValue;
        int lowest = int.MaxValue;
        for(int i = 0; i < m_FPSBuffer.Length; i++)
        {
            sum += m_FPSBuffer[i];
            if(m_FPSBuffer[i] < lowest)
            {
                lowest = m_FPSBuffer[i];
            }
            if (m_FPSBuffer[i] > highest)
            {
                highest = m_FPSBuffer[i];
            }
        }
        m_AverageFPS = (int)(sum / m_FramesRange);
        m_HighestFPS = highest;
        m_LowestFPS = lowest;
    }

    private void UpdateFrameBuffer()
    {
        m_FPSBuffer[m_FPSBufferIndex++] = (int)(1.0f / Time.unscaledDeltaTime);
        if(m_FPSBufferIndex >= m_FramesRange)
        {
            m_FPSBufferIndex = 0;
        }
    }
}
