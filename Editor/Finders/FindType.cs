using System;

namespace MissingReferencesUtility
{
    [Flags]
    public enum FindType
    {
        None           = 0,
        CurrentScene   = 1 << 1,
        AllScenes      = 1 << 2,
        Assets         = 1 << 3
    }   
}
