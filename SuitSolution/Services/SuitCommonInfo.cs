using System;
using System.Collections.Generic;
using System.Text;
using SuitSolution.Services;

public class SUITCommonInfo
{
    public static List<SUITComponentId> ComponentIds { get;  set; }
    public static List<SUITDependency> Dependencies { get;  set; }
    public static int? CurrentIndex { get; set; }
    public int IndentSize { get; private set; }

    public SUITCommonInfo()
    {
        ComponentIds = new List<SUITComponentId>();
        Dependencies = new List<SUITDependency>();
        CurrentIndex = 0;
        IndentSize = 4;
    }

    public static int ComponentIdToIndex(object componentId)
    {
        int index = -1;

        for (int i = 0; i < ComponentIds.Count; i++)
        {
            if (ComponentIds[i] == componentId && i >= 0)
            {
                index = i;
            }
        }

        for (int i = 0; i < Dependencies.Count; i++)
        {
            if (Dependencies[i].DependencyDigest == componentId && i >= 0)
            {
                index = i;
            }
        }

        return index;
    }
}