using UnityEngine;

namespace Wixot.AssetManagement
{
    public interface IAddressableTag
    {
        string AddressableLabel { get; set; }
    }

    public class AddressableLabelComponent : MonoBehaviour, IAddressableTag
    {
        public string AddressableLabel { get; set; }
    }
}