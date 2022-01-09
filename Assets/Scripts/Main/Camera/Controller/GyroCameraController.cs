using System.Runtime.CompilerServices;
using UnityEngine;

public class GyroCameraController : MonoBehaviour
{

    #region PRIVATE VARIABLES

    private Gyroscope gyroscope;

    #endregion

    #region UNITY MONOBEHAVIOURS

    private void Start()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
    }

    private void Update()
    {
        RotateView();
    }

	#endregion

	#region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RotateView()
	{
        Vector3 previousEulerAngles = transform.eulerAngles;
        Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;

        Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
        targetEulerAngles.z = 0.0f;

        transform.eulerAngles = targetEulerAngles;
	}

    #endregion

}