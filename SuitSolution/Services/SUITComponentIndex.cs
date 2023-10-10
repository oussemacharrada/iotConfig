using System;
using System.Collections.Generic;

namespace SuitSolution.Services
{
    public class SUITComponentIndex : SUITComponentId
    {
        public int ToSUIT()
        {
            return SUITCommonInfo.ComponentIdToIndex(this);
        }

        public new SUITComponentIndex FromSUIT(int d)
        {
            return (SUITComponentIndex)base.FromSUIT(SUITCommonInfo.ComponentIds[d].ToSUIT());
        }

        public string ToDebug(string indent)
        {
            string oneIndent = "    "; // Define your indentation here
            string newIndent = indent + oneIndent;
            return $"{ToSUIT()} / [{newIndent}] /";
        }
    }
}