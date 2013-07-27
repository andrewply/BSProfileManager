using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace BSProfileManager
{
    class BSHelper
    {
        private SettingHelper settingHelper;

        public BSHelper()
        {
            settingHelper = new SettingHelper();
        }

        public void closeBS()
        {
            System.Diagnostics.Process.Start(settingHelper.getCloseExePath());
            Thread.Sleep(2000);
        }

        public void modifyBSRegister(String guid)
        {
            //open registry
            RegistryKey bsKey = Registry.LocalMachine.OpenSubKey(Global.REG_BS_PATH, true);
            string keyValue = bsKey.GetValue(Global.REG_KEYNAME).ToString();
            
            //find and replace guid
            MatchCollection matches = Regex.Matches(keyValue, Global.GUID_REG_EX, RegexOptions.IgnoreCase);
            Debug.WriteLine(keyValue);
            foreach (Match match in matches)
            {
                keyValue = keyValue.Replace(match.Value, guid);
            }
            Debug.WriteLine(keyValue);
            //save registry
            bsKey.SetValue(Global.REG_KEYNAME, keyValue, RegistryValueKind.String);
            bsKey.Close();
            Thread.Sleep(2000);
        }

        public void startBS()
        {
            System.Diagnostics.Process.Start(settingHelper.getStartExePath());
            Thread.Sleep(2000);
        }
    }
}
