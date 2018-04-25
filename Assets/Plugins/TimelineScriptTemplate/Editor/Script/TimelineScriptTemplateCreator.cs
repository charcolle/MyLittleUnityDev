using System;
using UnityEngine;
using UnityEditor;

namespace charcolle.Utility.TimelineScriptTemplate {

    public class TimelineScriptTemplateCreator : MonoBehaviour {

        private readonly static string PANEL_TITLE       = "Create Timeline Asset Script";
        private readonly static string PANEL_DEFAULTNAME = "NewTimelineScript";

        [MenuItem( "Assets/Create/Timeline Script", menuItem = "Assets/Create/Timeline Script", priority = 460 )]
        private static void Create() {

            var savePath = EditorUtility.SaveFilePanel( PANEL_TITLE, FileHelper.GetSelectAssetPath(), PANEL_DEFAULTNAME, "" );
            if( !string.IsNullOrEmpty( savePath ) ) {
                onCreate( FileHelper.SystemPathToAssetPath( savePath ) );
            }

        }

        //=======================================================
        // process
        //=======================================================

        private static void onCreate( string assetPath ) {
            var templateName = FileHelper.GetTemplateName( assetPath );

            try {
                AssetDatabase.CreateFolder( FileHelper.GetParentFolderName( assetPath ), templateName );
                AssetDatabase.CreateFolder( assetPath, "Editor" );
                generateScriptFromTemplate( templateName, assetPath );
            } catch( Exception ex ) {
                Debug.LogError( "TimelineScript: fail to save script file. :" + ex );
            }

            AssetDatabase.Refresh();
        }

        private static void generateScriptFromTemplate( string templateName, string assetPath ) {

            for( int i = 0; i < FileHelper.TEMPLATE_FILENAME.Length; i++ ) {
                var scriptTemplateName = FileHelper.TEMPLATE_FILENAME[ i ];
                var templateText       = FileHelper.GetTemplateFile( scriptTemplateName );

                var script     = setUpTemplate( templateName, templateText );
                var scriptName = templateName + scriptTemplateName;
                if( scriptTemplateName.Equals( "Drawer" ) )
                    scriptName = "Editor/" + scriptName;

                FileHelper.SaveScript( assetPath, script, scriptName );
            }

        }

        private readonly static string REPLACE_STRING = "#CLASS#";
        private static string setUpTemplate( string templateName, string text ) {
            return text.Replace( REPLACE_STRING, templateName );
        }

    }

}
