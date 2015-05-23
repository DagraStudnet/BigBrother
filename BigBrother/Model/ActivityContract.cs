using System.Runtime.Serialization;
using ClassLibrary;

namespace ClientBigBrother.Model
{
    [KnownType(typeof (Activity))]
    [DataContract]
    public class ActivityContract : Activity
    {
    }
}