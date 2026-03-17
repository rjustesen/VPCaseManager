using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


    [ComImport]
    [Guid("25e326dc-d178-4b08-a3a1-7c3d2d574f99")] // Replace with a unique GUID
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICaseManager
    {
        [DispId(1)]
        int ITGSaveCaseInfo(string topicName, int selector, object value, int valueTpye);
        [DispId(2)]
        int ITGReadCaseInfo(string topicName, int selector);
    }
