
using System;
using System.Collections.Generic;

public class RunningAverager
{
  private List<(DateTime, float)> _entries = new List<(DateTime, float)>();
  private int                     _windowMaxEntries;
  private TimeSpan                _windowMaxDuration;

  public RunningAverager(TimeSpan windowMaxDuration, int windowMaxEntries = -1)
  {
    _windowMaxEntries = windowMaxEntries;
    _windowMaxDuration = windowMaxDuration;
  }

  public int GetEntryCount()
  {
    return _entries.Count;
  }

  public void Clear()
  {
    _entries.Clear();
  }

  public void AddEntry(float v)
  {
    _entries.Add((DateTime.UtcNow, v));
  }

  public bool TryGetAverage(out float avg)
  {
    if (_entries.Count == 0)
    {
      avg = -1;
      return false;
    }

    DateTime minDateTime = DateTime.UtcNow - _windowMaxDuration;
    for (int i = _entries.Count - 1; i >= 0; i--)
    {
      if (_entries[i].Item1 < minDateTime)
      {
        _entries.RemoveAt(i);
      }
    }

    if (_windowMaxEntries > 0)
    {
      while (_entries.Count > _windowMaxEntries)
      {
        _entries.RemoveAt(0);
      }
    }

    float sum = 0;
    for (int i = 0; i < _entries.Count; i++)
    {
      sum += _entries[i].Item2;
    }

    avg = sum / _entries.Count;
    return true;
  }
}