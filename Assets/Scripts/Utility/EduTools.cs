using UnityEngine;
using Unity.Mathematics;


namespace EduLabs.Utility {

  public class EduTools {

    public static float2 ToFloat2(Vector2 vec) {
      return new float2(vec.x, vec.y);
    }

    public static float3 ToFloat3(Vector3 vec) {
      return new float3(vec.x, vec.y, vec.z);
    }

    public static Vector2 ToVector2(float2 vec) {
      return new Vector2(vec.x, vec.y);
    }

    public static Vector3 ToVector3(float3 vec) {
      return new Vector3(vec.x, vec.y, vec.z);
    }

    public static quaternion ToQuaternion(Quaternion quaternion) {
      return new quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }

    public static Quaternion ToQuaternion(quaternion quaternion) {
      float4 qValues = quaternion.value;
      return new Quaternion(qValues.x, qValues.y, qValues.z, qValues.w);
    }

  }
}