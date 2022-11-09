using UnityEngine;

namespace VRMShaders
{
    public static class NormalConverter
    {
        private static Material _exporter;
        private static Material Exporter
        {
            get
            {
                if (_exporter == null)
                {
#if VRM_EXTERNAL_ASSETS
                    _exporter = new Material(ExternalAssets.ShaderHelper.Find("Hidden/UniGLTF/NormalMapExporter"));
#else
                    _exporter = new Material(Shader.Find("Hidden/UniGLTF/NormalMapExporter"));
#endif
                }
                return _exporter;
            }
        }

        // Unity texture to GLTF data
        public static Texture2D Export(Texture texture)
        {
            return TextureConverter.CopyTexture(texture, ColorSpace.Linear, false, Exporter);
        }
    }
}
