using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircularQueue<T>
{
    public int Length => _queue.Count;

    List<T> _queue;
    int _index;

    public CircularQueue() {
        _queue = new List<T>();
        _index = 0;
    }

    public CircularQueue(IEnumerable<T> items)
    {
        _queue = new List<T>();
        _index = 0;
        Add(items);
    }

    public T Next() {
        T result = Peek();
        IncrementIndex();
        return result;
    }

    public T Peek() {
        return _queue[_index];
    }

    public void Add(T item) {
        _queue.Add(item);
    }

    public void Add(IEnumerable<T> items) {
        _queue.AddRange(items);
    }

    public void Remove(T item) {
        if (!_queue.Contains(item))
            return;
        int index = _queue.IndexOf(item);
        if (index >= _index) {
            DecrementIndex();
        }
        _queue.RemoveAt(index);
    }

    public void Clear() {
        _queue.Clear();
        _index = 0;
    }

    public void Shuffle() {
        System.Random r = new System.Random();
        _queue = _queue.OrderBy(x => r.Next()).ToList();
        _index = 0;
    }

    void IncrementIndex() {
        _index = (_index + 1) % Length;
    }

    void DecrementIndex()
    {
        _index = (_index - 1) % Length;
    }
}
