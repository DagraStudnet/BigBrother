using System.Runtime.Serialization;
using ClassLibrary;

namespace ClientBigBrother.Model.PcUser
{
    [KnownType(typeof (Activity))]
    [DataContract]
    public class ActivityContract : Activity
    {
    }
}