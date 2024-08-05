using System;
using System.Collections.Generic;

namespace SimMath;

public class IntervalMergeSet
{
  public struct Interval
  {
    public int start;
    public int end;
  }

  private bool           _empty = true;
  private int            _totalStart;
  private int            _totalEnd;
  private List<Interval> _gaps = new List<Interval>(5);

  public bool Empty()
  {
    return _empty;
  }

  public bool ContainsValue(int v)
  {
    if (_empty)
    {
      return false;
    }

    if (v < _totalStart || v > _totalEnd)
    {
      return false;
    }

    for (int i = 0; i < _gaps.Count; i++)
    {
      Interval gap = _gaps[i];
      if (v >= gap.start && v <= gap.end)
      {
        return false;
      }
    }

    return true;
  }

  public int GetLargestConsecutiveValue()
  {
    if (_empty)
    {
      throw new Exception();
    }

    if (_gaps.Count > 0)
    {
      return _gaps[0].start - 1;
    }
    
    return _totalEnd;
  }

  public void Insert(int v)
  {
    Insert(v, v);
  }
  
  public void Insert(int insertStart, int insertEnd, List<int>? newInsertResults = null)
  {
    newInsertResults?.Clear();
    if (insertEnd < insertStart)
    {
      return;
    }
    
    if (_empty)
    {
      _empty = false;
      _totalStart = insertStart;
      _totalEnd = insertEnd;
      AddRange(_totalStart, _totalEnd, newInsertResults);
      return;
    }
    
    for(int i = _gaps.Count - 1; i >= 0; i--)
    {
      Interval gap = _gaps[i];
      
      //case 1: new interval fully contains gap
      if (insertStart <= gap.start && insertEnd >= gap.end)
      {
        _gaps.RemoveAt(i);
        AddRange(gap.start, gap.end, newInsertResults);
      }
      //case 2: new interval overlaps part of gap
      else if (insertEnd > gap.start && insertStart < gap.end)
      {
        //subcase a: interval is fully contained in gap
        if (insertStart > gap.start && insertEnd < gap.end)
        {
          //we need to "split" the gap
          _gaps.RemoveAt(i);
          
          _gaps.Insert(i, new Interval() { start = gap.start + 1, end = insertStart - 1});
          _gaps.Insert(i + 1, new Interval() { start = insertEnd + 1, end = gap.start - 1});

          AddRange(insertStart, insertEnd, newInsertResults);
        }
        //subcase b: interval overlaps on left/right side
        else
        {
          if (insertStart <= gap.start)
          {
            AddRange(gap.start, insertEnd, newInsertResults);
            gap.start = insertEnd;
          }
          else
          {
            AddRange(insertStart, gap.end, newInsertResults);
            gap.end = insertStart;
          }
        }
      }
      
      //case 3: no overlap, do nothing
    }
    
    //check if new interval extends our starting bounds
    if (insertStart < _totalStart)
    {
      //would this create a gap?
      if (insertEnd < _totalStart - 1)
      {
        _gaps.Insert(0, new Interval() { start = insertEnd + 1, end = _totalStart - 1});
        AddRange(insertStart, insertEnd, newInsertResults);
      }
      else
      {
        AddRange(insertStart, _totalStart - 1, newInsertResults);  
      }
      
      _totalStart = insertStart;
    }

    if (insertEnd > _totalEnd)
    {
      if (insertStart > _totalEnd + 1)
      {
        _gaps.Add(new Interval() { start = _totalEnd + 1, end = insertStart - 1});
        AddRange(insertStart, insertEnd, newInsertResults);
      }
      else
      {
        AddRange(_totalEnd + 1, insertEnd, newInsertResults);
      }
      
      _totalEnd = insertEnd;
    }

    newInsertResults?.Sort();
  }

  private void AddRange(int start, int end, List<int>? result)
  {
    if (result == null)
    {
      return;
    }
    
    for (int i = start; i <= end; i++)
    {
      result.Add(i);
    }
  }
}