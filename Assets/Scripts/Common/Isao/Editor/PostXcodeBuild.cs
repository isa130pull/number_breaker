﻿using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Hoge
{
    public class PostXcodeBuild
    {
        [PostProcessBuild]
        public static void SetXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS) return;
            
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            var rootDict = plist.root;
            // ここに記載したKey-ValueがXcodeのinfo.plistに反映されます
            rootDict.SetString("GADApplicationIdentifier",
                "ca-app-pub-4438906405954014~9227983871");
            
            // remove exit on suspend if it exists.
            const string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
            if(rootDict.values.ContainsKey(exitsOnSuspendKey))
            {
                rootDict.values.Remove(exitsOnSuspendKey);
            }
            
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}