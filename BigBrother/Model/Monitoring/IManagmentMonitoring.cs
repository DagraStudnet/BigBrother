using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace ClientBigBrother.Model.Monitoring
{
    interface IManagmentMonitoring
    {
        IUser User { get; set; }
    }
}
