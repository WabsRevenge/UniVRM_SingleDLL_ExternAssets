using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace ExternalAssets
{
    public class ExternalAssetsHelper
    {
        static public void Initialise()
        {
            AssetBundleCreateRequest shadersBundleCreateRequest = AssetBundle.LoadFromStreamAsync(Assembly.GetExecutingAssembly().GetManifestResourceStream("CombinedVRMDll.Assets.shaders"));
            AssetBundle assetBundle = shadersBundleCreateRequest.assetBundle;

            if (!assetBundle)
            {
                Debug.LogWarning("Failed to load asset bundle");
            }
            else
            {
                LoadExternalAssets(assetBundle);
                assetBundle.Unload(false);
            }
        }

        static public void LoadExternalAssets(AssetBundle myLoadedAssetBundle)
        {
            if (myLoadedAssetBundle != null)
            {
                var names = myLoadedAssetBundle.GetAllAssetNames();
                foreach(string name in names)
                {
                    object obj = myLoadedAssetBundle.LoadAsset(name);

                    if(obj is Shader)
                    {
                        Shader shader = obj as Shader;
                        ShaderHelper.AddExternalShader(shader.name, shader); //function logs which shader is added.
                    }
                    else
                    {
                        Debug.LogWarning($"Ignoring Asset from Assetbundle: {obj.ToString()}.");
                    }
                }
            }
        }
    }

	public class ShaderHelper
	{
        public static Dictionary<String, Shader> m_defaultShaders = null;
        public static Dictionary<String, Shader> m_externalShaders = null;
        public static void AddExternalShader(String str, Shader shader, bool bDefaultShader = false)
        {
            if (bDefaultShader)
            {
                if (m_defaultShaders == null)
                    m_defaultShaders = new Dictionary<String, Shader>();

                Debug.Log($"Adding Default Shader: {str}.");
                m_defaultShaders[str] = shader; //NOTE: If shader exists by label 'str', it will be replaced.

            }
            else
            {
                if (m_externalShaders == null)
                    m_externalShaders = new Dictionary<String, Shader>();

                Debug.Log($"Adding External Shader: {str}.");
                m_externalShaders[str] = shader; //NOTE: If shader exists by label 'str', it will be replaced.
            }
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

            if (resultingShader == null && (m_defaultShaders == null || !m_defaultShaders.TryGetValue(strShaderName, out resultingShader)))
            {
                if (m_defaultShaders == null)
                    Debug.Log($"Default Shaders Null.");
                else
                    Debug.Log($"Failed to find Shader in Default Shaders: {strShaderName}.");
            }
            else
                Debug.Log($"Found Shader in Default Shaders: {strShaderName}.");

            if (resultingShader == null)
            {
                resultingShader = Shader.Find(strShaderName);
                Debug.Log($"Found Shader in Asset Database: {strShaderName}.");
            }

            return resultingShader;
        }
	}
}