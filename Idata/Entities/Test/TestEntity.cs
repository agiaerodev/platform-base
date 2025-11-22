using Idata.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idata.Entities.Test
{
    public class TestEntity : EntityBase
    {
        public string name { get; set; }

        public TestEntity()
        {
            searchable_fields = "name";
        }
    }
}
