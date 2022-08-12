using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BusinessServerInterface
{
        [DataContract]
        public class AccountNotFoundFault
        {
            [DataMember]
            public string Issue { get; set; }
        }
    
}
