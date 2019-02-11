using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Data
{
    public class DataModel
    {
        /* It figures suitable data model, properties for this project */

        public enum DataTypeName
        {
            Service,
            DB,
            Other
        }

        public string DataType { get; set; }
        public string DataServiceURL { get; set; }
        public string DataAccessUserName { get; set; }
        public string DataAccessPassword { get; set; }
        public string DataAccessQuery { get; set; }

    }
}
