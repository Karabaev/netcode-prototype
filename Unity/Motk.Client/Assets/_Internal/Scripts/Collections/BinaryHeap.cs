using System;
using System.Collections.Generic;

namespace Motk.Collections
{
  public class BinaryHeap<TKey, TItem>
    where TKey : IEquatable<TKey>
    where TItem : struct, IComparable<TItem>
  {
    private readonly IDictionary<TKey, int> _map;
    private readonly IList<TItem> _collection;

    private readonly Func<TItem, TKey> _lookupFunc;

    public int Count => _collection.Count;

    public void Enqueue(TItem item)
    {
      _collection.Add(item);
      var i = _collection.Count - 1;
      _map[_lookupFunc.Invoke(item)] = i;

      while(i > 0)
      {
        var j = (i - 1) / 2;

        if(_collection[i].CompareTo(_collection[j]) > 0)
          break;

        Swap(i, j);
        i = j;
      }
    }

    public TItem Dequeue()
    {
      if(_collection.Count == 0)
        throw new InvalidOperationException("There is no elements in heap");

      var result = _collection[0];
      RemoveRoot();
      _map.Remove(_lookupFunc.Invoke(result));
      return result;
    }

    public void Clear()
    {
      _collection.Clear();
      _map.Clear();
    }

    public bool TryGet(TKey key, out TItem? value)
    {
      if(!_map.TryGetValue(key, out var index))
      {
        value = default;
        return false;
      }

      value = _collection[index];
      return true;
    }

    public void Modify(TItem value)
    {
      if(!_map.TryGetValue(_lookupFunc.Invoke(value), out var index))
        throw new KeyNotFoundException(nameof(value));

      _collection[index] = value;
    }

    private void Swap(int a, int b)
    {
      (_collection[a], _collection[b]) = (_collection[b], _collection[a]);
      _map[_lookupFunc.Invoke(_collection[a])] = a;
      _map[_lookupFunc.Invoke(_collection[b])] = b;
    }

    private void RemoveRoot()
    {
      _collection[0] = _collection[^1];
      _map[_lookupFunc.Invoke(_collection[0])] = 0;
      _collection.RemoveAt(_collection.Count - 1);

      var i = 0;
      while(true)
      {
        var largest = FindLargestIndex(i);

        if(largest == i)
          return;

        Swap(i, largest);
        i = largest;
      }
    }

    private int FindLargestIndex(int i)
    {
      var leftIndex = 2 * i + 1;
      var rightIndex = 2 * i + 2;
      var largest = i;

      if(leftIndex < _collection.Count && _collection[leftIndex].CompareTo(_collection[largest]) < 0)
        largest = leftIndex;

      if(rightIndex < _collection.Count && _collection[rightIndex].CompareTo(_collection[largest]) > 0)
        largest = rightIndex;

      return largest;
    }

    public BinaryHeap(Func<TItem, TKey> lookupFunc)
    {
      _lookupFunc = lookupFunc;
      _collection = new List<TItem>();
      _map = new Dictionary<TKey, int>();
    }
  }
}