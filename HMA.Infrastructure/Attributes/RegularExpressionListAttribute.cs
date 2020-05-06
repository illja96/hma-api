using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HMA.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RegularExpressionListAttribute : RegularExpressionAttribute
    {
        public RegularExpressionListAttribute(string pattern) : base(pattern) { }

        public override bool IsValid(object value)
        {
            var list = value as List<string>;
            if (list == null)
            {
                return false;
            }

            var isValid = list.All(e => base.IsValid(e));
            return isValid;
        }
    }
}
