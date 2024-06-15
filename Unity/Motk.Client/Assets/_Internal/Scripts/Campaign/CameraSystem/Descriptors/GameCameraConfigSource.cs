using com.karabaev.camera.unity.Descriptors;
using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using UnityEngine;

namespace Motk.Client.Campaign.CameraSystem.Descriptors
{
  [DescriptorSource("DR_CameraConfig", typeof(ResourcesDescriptorSourceProvider))]
  [CreateAssetMenu(menuName = "Motk/CameraConfigRegistry")]
  public class GameCameraConfigSource : GameCameraConfigSourceBase { }
}