using System;

namespace Conto.Data.ExcelHelpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelTypeAttribute : Attribute
    {
        public Type DestinationType { get; set; }
    }
}
