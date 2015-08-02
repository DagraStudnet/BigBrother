using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.UserLibrary;

namespace ClientBigBrother.Model.Monitoring
{
    interface IManagmentMonitoring
    {
        IUser PcUser { get; }
    }
}
