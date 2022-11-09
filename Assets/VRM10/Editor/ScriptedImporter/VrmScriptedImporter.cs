using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif


namespace UniVRM10
{
    [ScriptedImporter(1, "vrm")]
    public class VrmScriptedImporter : ScriptedImporter
    {
        [SerializeField]
        public bool MigrateToVrm1 = default;

        [SerializeField]
        public UniGLTF.RenderPipelineTypes RenderPipeline = default;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            VrmScriptedImporterImpl.Import(this, ctx, MigrateToVrm1, RenderPipeline);
        }

        void OnValidate()
        {
            if (RenderPipeline == UniGLTF.RenderPipelineTypes.UniversalRenderPipeline)
            {
#if VRM_EXTERNAL_ASSETS
                if (ExternalAssets.ShaderHelper.Find(UniGLTF.GltfPbrUrpMaterialImporter.ShaderName) == null)
#else
                if (Shader.Find(UniGLTF.GltfPbrUrpMaterialImporter.ShaderName) == null)
#endif
                {
                    Debug.LogWarning("URP is not installed. Force to BuiltinRenderPipeline");
                    RenderPipeline = UniGLTF.RenderPipelineTypes.BuiltinRenderPipeline;
                }
            }
        }
    }
}
