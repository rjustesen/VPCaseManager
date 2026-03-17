using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

[ComVisible(true)] 
[Guid("297e58ac-5bef-4c11-b9e8-a101714b4917")] 
[ClassInterface(ClassInterfaceType.None)]
[ProgId("VPCaseMan3.VPCaseManager2")] 
public class CaseManagerImpl : ICaseManager
{
    public int ITGSaveCaseInfo(string topicName, int selector, object value, int valueType)
    {
        return 0;
    }
    public int ITGReadCaseInfo( string topicName, int selector)
    {
        return 0;
    }
}


