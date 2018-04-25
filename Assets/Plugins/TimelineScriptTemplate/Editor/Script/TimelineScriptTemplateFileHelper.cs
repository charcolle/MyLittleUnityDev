using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace charcolle.Utility.TimelineScriptTemplate {

    internal static class FileHelper {

        private readonly static string SEARCH_ROOT           = "TimelineScriptTemplateCreator";
        private readonly static string RELATIVEPATH_TEMPLATE = "Editor/Template/";

        internal readonly static string[] TEMPLATE_FILENAME = new string[] { "Clip",
                                                                            "Behaviour",
                                                                            "MixerBehaviour",
                                                                            "Track",
                                                                            "Drawer" };

        //=======================================================
        // path
        //=======================================================

        internal static string ScriptTemplatePath {
            get {
                return pathSlashFix( Path.Combine( TimelineScriptRootPath, RELATIVEPATH_TEMPLATE ) );
            }
        }

        internal static string TimelineScriptRootPath {
            get {
                var guid = getAssetGUID( SEARCH_ROOT );

                if( string.IsNullOrEmpty( guid ) ) {
                    Debug.LogError( "fatal error." );
                    return null;
                }
                var scriptPath = Path.GetDirectoryName( AssetDatabase.GUIDToAssetPath( guid ) );
                var editorPath = Path.GetDirectoryName( scriptPath );
                var rootPath   = Path.GetDirectoryName( editorPath );

                return pathSlashFix( rootPath );
            }
        }

        //=======================================================
        // public
        //=======================================================

        internal static string GetParentFolderName( string assetPath ) {
            return Path.GetDirectoryName( assetPath );
        }

        internal static string GetTemplateName( string assetPath ) {
            return Path.GetFileNameWithoutExtension( assetPath );
        }

        internal static string GetSelectAssetPath() {
            if( Selection.assetGUIDs == null || Selection.assetGUIDs.Length == 0 )
                return Application.dataPath;

            return AssetDatabase.GUIDToAssetPath( Selection.assetGUIDs[ 0 ] );
        }

        internal static string GetTemplateFile( string templateName ) {
            var templatePath = AssetPathToSystemPath( Path.Combine( ScriptTemplatePath, templateName + ".template" ) );

            try {
                StreamReader sr  = new StreamReader( templatePath, Encoding.UTF8 );
                var templateText = sr.ReadToEnd();
                sr.Close();
                return templateText;
            } catch( Exception ex ) {
                Debug.Log( "TimelineScript: fail to load template file. :" + ex );
            }

            return null;
        }

        internal static void SaveScript( string savePath, string script, string scriptName ) {
            var scriptSavePath = pathSlashFix( Path.Combine( savePath, scriptName + ".cs" ) );

            try {
                StreamWriter sw = new StreamWriter( scriptSavePath, false, Encoding.UTF8 );
                sw.Write( script );
                sw.Close();
            }
            catch( Exception ex ) {
                Debug.Log( "TimelineScript: fail to save script file. :" + ex );
            }
        }

        internal static string AssetPathToSystemPath( string path ) {
            return pathSlashFix( Path.Combine( dataPathWithoutAssets, path ) );
        }

        internal static string SystemPathToAssetPath( string path ) {
            return pathSlashFix( path ).Replace( Application.dataPath, "Assets" );
        }


        //=======================================================
        // private
        //=======================================================

        private static string getAssetGUID( string searchFilter ) {
            var guids = AssetDatabase.FindAssets( searchFilter );
            if( guids == null || guids.Length == 0 ) {
                return null;
            }

            if( guids.Length > 1 ) {
                Debug.LogWarning( "more than one file was found." );
            }
            return guids[ 0 ];
        }

        private const string forwardSlash = "/";
        private const string backSlash = "\\";
        private static string pathSlashFix( string path ) {
            return path.Replace( backSlash, forwardSlash );
        }

        private static string dataPathWithoutAssets {
            get {
                return pathSlashFix( Application.dataPath.Replace( "Assets", "" ) );
            }
        }

    }

}