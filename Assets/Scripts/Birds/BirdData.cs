using UnityEngine;

namespace Birds
{
    public class BirdData : MonoBehaviour
    {
        public Transform BirdTransform => this.transform;
        
        private static readonly int FrameRate = Shader.PropertyToID("_FrameRate");
        private static readonly int MixAnimation = Shader.PropertyToID("_MixAnimation");

        public void SetAnimation(int frame, float mix)
        {
            var materials = GetComponentInChildren<SkinnedMeshRenderer>().material;
        
            materials.SetFloat(FrameRate, frame);
            materials.SetFloat(MixAnimation, mix);
        }
    }
}
