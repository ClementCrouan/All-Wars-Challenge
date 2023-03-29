using UnityEngine;

public class MotorDeplacement : MonoBehaviour
{
	[Header("Rotation")]
	float rotationX;
	public float sensitivityX = 5f;
	public float rotationXLimit = 85f;
	float currentRotationX = 0f;

    // Update is called once per frame
    void Update()
	{ 
		if (!isMouseOffScreen())
		{
			rotationX = Input.GetAxisRaw("Mouse Y") * sensitivityX;

			currentRotationX -= rotationX;
			currentRotationX = Mathf.Clamp(currentRotationX, -rotationXLimit, rotationXLimit);

			transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
		}
	}
	private bool isMouseOffScreen()
	{
		if (Input.mousePosition.y <= 2)
			return true;
		return false;
	}
}