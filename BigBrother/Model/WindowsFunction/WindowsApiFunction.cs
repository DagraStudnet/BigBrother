using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ClientBigBrother.Model.WindowsFunction
{
    class WindowsApiFunction
    {
        //HWND WINAPI GetForegroundWindow (void);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        //  int WINAPI GetWindowText (__in HWND hWnd,__out LPTSTR lpString, __in int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, [Out]StringBuilder lpString, int nMaxCount);
    }
}
