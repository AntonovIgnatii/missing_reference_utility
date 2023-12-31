﻿using UnityEngine;

namespace MissingReferencesUtility
{
    public static class TransformExtensions
    {
        public static string GetPath(this Transform current) {
            if (current.parent == null)
                return "/" + current.name;
            return current.parent.GetPath() + "/" + current.name;
        }
    }
}