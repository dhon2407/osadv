using UnityEngine;

namespace Utilities
{
    public static class InputHandler
    {
        public static bool GetTap(out int id)
        {
            id = -1;
            bool result = false;
#if (UNITY_EDITOR)
            result = Input.GetMouseButtonDown(0);
#elif(UNITY_ANDROID)
            //FOR IMPROVEMENT IN MULTI TOUCHES
            result = Input.touchCount > 0;
            if (result)
                id = Input.touches[0].fingerId;
#endif

            return result;
        }
    }
}
