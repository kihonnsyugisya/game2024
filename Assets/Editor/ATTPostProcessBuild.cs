#if UNITY_IOS

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
//20221206ATT��Frame��info.plist�������Œǉ�����X�N���v�g

public static class ATTPostProcessBuild
{

    [PostProcessBuild]//�r���h���Ɏ��s����
    private static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        // Info.plist �� Privacy - Tacking Usage Description(NSUserTrackingUsageDescription)��ǉ�����i�X�e�b�v�Q�j
        string infoPlistPath = buildPath + "/Info.plist";
        PlistDocument infoPlist = new PlistDocument();
        infoPlist.ReadFromFile(infoPlistPath);
        PlistElementDict root = infoPlist.root;
        root.SetString("NSUserTrackingUsageDescription", "���Ȃ��̍D�݂ɍ��킹���L����\�����邽�߂Ɏg�p����܂�");
        infoPlist.WriteToFile(infoPlistPath);

        // PBXProject�N���X�Ƃ����̂�p����AppTrackingTransparency.framework��ǉ����Ă����܂��i�X�e�b�v�R�j
        string pbxProjectPath = PBXProject.GetPBXProjectPath(buildPath);
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(pbxProjectPath);
        string targetGuid = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", true);
        pbxProject.WriteToFile(pbxProjectPath);
    }
}

#endif