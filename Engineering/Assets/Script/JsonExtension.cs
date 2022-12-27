using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonExtension
{
  private class ArrayWrapper<T>
  {
        public T[] array;
  }
    public static T[] ToArray<T>(this string json)
    {
        string newJson = "{\"array\": " + json + "}";
        ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>>(newJson);
        return wrapper.array;
    }
}
