using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndexedItem : IComparable{
    public int m_index;
    public float m_value;

    public IndexedItem(int p_index, float p_value){
        m_index = p_index;
        m_value = p_value;
    }

    int IComparable.CompareTo(object obj){
        IndexedItem indexedItem = (IndexedItem)obj;

        if(m_value > indexedItem.m_value){ return 1;}
        if(m_value < indexedItem.m_value) { return -1;}
        return 0;

    }

}

public class IndexedPriorityQueueFloat
{

    List<IndexedItem> m_priorityQueue;
    List<int> m_keyToPosition;

    public IndexedPriorityQueueFloat(List<float> p_itemValue, List<int> p_itemIndex){

        IndexedItem edge;
        m_priorityQueue = new List<IndexedItem>();

        if(p_itemIndex.Count != p_itemValue.Count){ return;}
        for(int i = 0; i < p_itemValue.Count; i++){
            edge = new IndexedItem(p_itemIndex[i], p_itemValue[i]);
            m_priorityQueue.Add(edge);
        }

        m_keyToPosition = new List<int>( new int[m_priorityQueue.Count]);

        SordAndUpdateKeys();

    }

    public IndexedPriorityQueueFloat(List<int> p_itemIndex){

        IndexedItem edge;
        m_priorityQueue = new List<IndexedItem>();

        if(p_itemIndex.Count != p_itemIndex.Count){ return;}
        for(int i = 0; i < p_itemIndex.Count; i++){
            edge = new IndexedItem(p_itemIndex[i], 0);
            m_priorityQueue.Add(edge);
        }

        m_keyToPosition = new List<int>( new int[m_priorityQueue.Count]);

        SordAndUpdateKeys();

    }

    public IndexedPriorityQueueFloat(int p_numberOfNodes){
        m_priorityQueue = new List<IndexedItem>();
        m_keyToPosition = new List<int>( new int[p_numberOfNodes]);
    }

    public void Insert(float p_value, int p_key){
        IndexedItem indexedItem = new IndexedItem(p_key, p_value);
        m_priorityQueue.Add(indexedItem);
        SordAndUpdateKeys();
    }

    public void SordAndUpdateKeys(){
        m_priorityQueue.Sort();
        for(int i = 0; i < m_priorityQueue.Count; i++){
            m_keyToPosition[m_priorityQueue[i].m_index] = i;
        }
    }

    public int TakeNextItem(){
        int item = m_priorityQueue[0].m_index;
        m_priorityQueue.RemoveAt(0);
        SordAndUpdateKeys();
        return item;
    }

    public void ChangeValue(int p_index, float p_newValue){
        m_priorityQueue[m_keyToPosition[p_index]].m_value = p_newValue;
        SordAndUpdateKeys();

    }

    public float PeakNextItem(){
        return m_priorityQueue[0].m_index;
    }

    public bool IsEmpty(){
        if(m_priorityQueue.Count <= 0){ return true;}
        else { return false;}
    }
}
