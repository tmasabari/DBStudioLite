
using Microsoft.Win32;

namespace XML_Reader
{
    public class MyRegistry
    {

        public bool ReadValue(RegistryKey objSection, string sKey, string ValueName, out object sValue)
        {
            RegistryKey pRegKey;/* new Microsoft.Win32 Registry Key */
            sValue = null;
            //regkey = objSection.CreateSubKey(sKey); //@"Software\MyTestRegKey"
            pRegKey = objSection.OpenSubKey(sKey);
            if (pRegKey == null) return false;
            sValue = pRegKey.GetValue(ValueName);
            return (sValue != null);

            //string[] valnames = regkey.GetValueNames();
            //string val0 = (string)regkey.GetValue(valnames[0]);
            //string val1 = (string)regkey.GetValue(valnames[1]);
            //regkey.SetValue("Domain", (string)"Workgroup");
            //Registry.LocalMachine.Flush();
        }
        public void WriteValue(RegistryKey objSection, string sKey, string sValueName, string sValue)
        {
            RegistryKey pRegKey;/* new Microsoft.Win32 Registry Key */

            pRegKey = objSection.CreateSubKey(sKey); //@"Software\MyTestRegKey"
            //pRegKey = objSection.OpenSubKey(sKey);
            //if (pRegKey == null) retun;
            pRegKey.SetValue(sValueName, sValue);

            //string[] valnames = regkey.GetValueNames();
            //string val0 = (string)regkey.GetValue(valnames[0]);
            //string val1 = (string)regkey.GetValue(valnames[1]);
            //regkey.SetValue("Domain", (string)"Workgroup");
            //Registry.LocalMachine.Flush();
        }
    }
}