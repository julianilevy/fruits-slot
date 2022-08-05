using UnityEngine;

namespace WinSystemsSlotTest
{
    public class OutOfSlotDetectorForReelContent : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == Layer.ReelContent)
                collision.GetComponent<ReelContent>().reel.UpdateReel();
        }
    }
}