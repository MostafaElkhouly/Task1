using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Entities.Base
{
    public class StoredPramaters
    {
        public string ParameterName { set; get; }
        public DbType ParameterType { set; get; }
        public object Value { set; get; }
    }
}
