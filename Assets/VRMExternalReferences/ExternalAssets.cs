using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets
{
    public class ExternalAssetsHelper
    {
        static public void LoadExternalAssets(string strPath)
        {

            AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(strPath);
            if (myLoadedAssetBundle != null)
            {
                var names = myLoadedAssetBundle.GetAllAssetNames();
                foreach(string name in names)
                {
                    object obj = myLoadedAssetBundle.LoadAsset(name);

                    if(obj is Shader)
                    {
                        Shader shader = obj as Shader;
                        ShaderHelper.AddExternalShader(shader.name, shader);
                    }
                    else
                    {
                        Debug.LogWarning($"Ignoring Asset from Assetbundle: {obj.ToString()}.");
                    }
                }
            
                myLoadedAssetBundle.LoadAllAssets();
            }
        }
    }

	public class ShaderHelper
	{
        public static Dictionary<String, Shader> m_externalShaders = null;
        public static void AddExternalShader(String str, Shader shader)
        {
            if (m_externalShaders == null)
                m_externalShaders = new Dictionary<String, Shader>();

            Debug.LogWarning($"Adding External Shader: {str}.");
            m_externalShaders.Add(str, shader);
        }

        public static Shader Find(string strShaderName)
        {
            Debug.Log($"Find Shader: {strShaderName}.");

            Shader resultingShader = null;

            if (resultingShader == null && (m_externalShaders == null || !m_externalShaders.TryGetValue(strShaderName, out resultingShader)))
            {
                if(m_externalShaders == null)
                    Debug.Log($"External Shaders Null.");
                else
                    Debug.Log($"Failed to find Shader in External Shaders: {strShaderName}.");
            }
            else
                Debug.Log($"Found Shader in External Shaders: {strShaderName}.");

            if (resultingShader == null)
            {
                resultingShader = Shader.Find(strShaderName);
                Debug.Log($"Found Shader in Asset Database: {strShaderName}.");
            }

            return resultingShader;
        }
	}
}