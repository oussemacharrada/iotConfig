using System;
using System.Collections.Generic;
using System.Linq;

namespace SuitSolution.Services
{
    public class SUITComponentIndex : SUITComponentId
    {
        public new int ToSUIT()
        {
            return SUITCommonInfo.ComponentIdToIndex(this);
        }

        public new SUITComponentIndex FromSUIT(int d)
        {
            base.FromSUIT(SUITCommonInfo.ComponentIds[d].ToSUIT() as List<object>);
            return this;
        }

        public new string ToDebug(string indent)
        {
            string oneIndent = "    "; // Define your indentation here
            string newIndent = indent + oneIndent;
            return $"{ToSUIT()} / [{string.Join("", componentIds.Select(item => item.ToDebug(newIndent)))}] /";
        }
    }
}