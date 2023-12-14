using System;
using System.Collections.Generic;
using SuitSolution.Services;

public class SUITCommonInfo
{
    public static List<SUITComponentId> ComponentIds { get;  set; }
    public static List<SUITDependency> Dependencies { get;  set; }
    public static int CurrentIndex { get; set; }

    public static SUITCommonInfo Instance { get; } = new SUITCommonInfo();

    public static string OneIndent { get; } = "    ";

    public SUITCommonInfo()
    {
        ComponentIds = new List<SUITComponentId>();
        Dependencies = new List<SUITDependency>();
        CurrentIndex = 0;
    }
    public static int GetDependencyIndex(object dependencyId)
    {
        int index = Dependencies.FindIndex(dep => dep.Matches(dependencyId));
        return index;
    }
    public static int ComponentIdToIndex(object componentId)
    {
        int index = ComponentIds.FindIndex(cid => cid.Equals(componentId));
        if (index >= 0)
        {
            return index; // This is a component index
        }

        index = Dependencies.FindIndex(dep => dep.DependencyDigest.Equals(componentId));
        if (index >= 0)
        {
            return new DependencyIndex(index); // This is a dependency index
        }

        return -1;
    }

}