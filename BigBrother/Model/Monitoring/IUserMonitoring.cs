using ClassLibrary;

namespace ClientBigBrother.Model.Monitoring
{
    public interface IUserMonitoring <T>
    {
        void SaveInformationAboutUserPc(T user);
        void SaveUsbConnection(T user);
        void SaveOpenProgramUser(T user);
        void SaveStartUpProgramsOnDestop(T user);
    }
}