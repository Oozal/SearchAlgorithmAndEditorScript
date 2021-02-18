using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IheapItem<T>
{
    public T[] items;
    public int currentIndex;

    public Heap(int heapSize)
    {
        items = new T[heapSize];
    }
    public void Add(T item)
    {
        items[currentIndex] = item;
        currentIndex += 1;
        item.heapIndex = currentIndex - 1;
        
        SortUp(item);
    }

    public void SortUp(T item)
    {
        Debug.Log("sorting up");
        int parentHeapIndex = (item.heapIndex - 1) / 2;
        if (parentHeapIndex >= 0)
        {
            while (true)
            {

                if (item.CompareTo(items[parentHeapIndex]) > 0)
                {
                    Swap(item, items[parentHeapIndex]);
                }
                else
                {
                    break;
                }
                parentHeapIndex = (item.heapIndex - 1) / 2;
            }
        }
    }

    public void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int tempIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = tempIndex;
    }
    public T GetFirst()
    {
        T firstItem = items[0];
        items[0] = items[currentIndex - 1];
        items[0].heapIndex = 0;
        currentIndex -= 1;
        SortDown(items[0]);
        return firstItem;
       
    }

    public void SortDown(T item)
    {
        
        while (true)
        {
            int rightChild = (2 * item.heapIndex) + 2;
            int leftChild = (2 * item.heapIndex) + 1;
            int swapIndex;
            if(leftChild<=currentIndex-1)
            {
                swapIndex = leftChild;
                if (rightChild < currentIndex - 1)
                {
                    if (items[rightChild].CompareTo(items[leftChild]) > 0)
                    {
                        swapIndex = rightChild;
                    }
                    else
                        swapIndex = leftChild;
                }
            }
            
            else
                break;
            if (items[swapIndex].CompareTo(item) > 0)
            {
                Swap(items[swapIndex], item);
            }
            else
                break;
            
            
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    public int Count()
    {
        return currentIndex;
    }

    public void UpdateHeap(T item)
    {
        SortUp(item);
    }
}

public interface IheapItem<T> : IComparable<T>
{
    int heapIndex
    {
        get;
        set;
    }
}

