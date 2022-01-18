using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class DataTableParameters
    {
        public List<DataTableColumn> columns { get; set; }
        public int draw { get; set; }
        public int length { get; set; }
        public List<DataOrder> order { get; set; }
        public Search search { get; set; }
        public int start { get; set; }
        public string Hi { get; set; }
    }

    public class Search
    {
        public bool regex { get; set; }
        public string value { get; set; }
    }

    public class DataTableColumn
    {
        public int data { get; set; }
        public string name { get; set; }
        public bool orderable { get; set; }
        public bool searchable { get; set; }
        public Search search { get; set; }

    }

    public class DataOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
    