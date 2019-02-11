using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Business.Entity
{
    [DataContract]
    public class NavModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics & Constants
        // Statics
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        [DataMember]
        public string NavTitle { get; set; }

        [DataMember]
        public string NavUrl { get; set; }
        #endregion
    }
}
