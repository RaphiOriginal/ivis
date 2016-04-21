using UnityEngine;
using System.Collections;

public class MouseWalk : MonoBehaviour {
    public float m_headingSpeed;
    public float m_pitchSpeed;
    public float m_translationSpeed;
	public Camera m_camera;		// the camera from which the window size is taken

    Vector3 m_currentEuler;

    // Use this for initialization
    void Start () {
        m_currentEuler = GetComponent<Transform>().localEulerAngles;
        m_camera = GetComponent<Camera>();
    }

    void Update()
    {
		if (m_camera != null) {
			Transform theTransform = GetComponent<Transform> ();

			Vector3 mouseNormalised;
            mouseNormalised.z = 1.0f;

            if (Input.touchCount > 0)
            {
                Vector3 touchPosition = Input.touches[0].position;

                mouseNormalised.x = touchPosition.x / (float)m_camera.pixelWidth;
                mouseNormalised.y = touchPosition.y / (float)m_camera.pixelHeight;
            } else {
                mouseNormalised.x = Input.mousePosition.x / (float)m_camera.pixelWidth;
                mouseNormalised.y = Input.mousePosition.y / (float)m_camera.pixelHeight;
            }


			float dt = Time.deltaTime;
			if (Input.GetMouseButton (0) && (Input.touchCount < 2)) {
                // left mouse button / single touch:
                // X -> rotation speed around Y axis (heading),
                // Y -> translation speed in view direction (camera-local Z axis)
				m_currentEuler.y += (mouseNormalised.x * 2.0f - 1.0f) * dt * m_headingSpeed;

				Vector3 translation;
				translation.x = translation.y = 0.0f;
				translation.z = (mouseNormalised.y * 2.0f - 1.0f) * dt * m_translationSpeed;

				theTransform.Translate (translation);
                theTransform.localEulerAngles = m_currentEuler;
            } else if (Input.GetMouseButton (1) || (Input.GetMouseButton(0) && Input.touchCount == 2)) {
                // right mouse button / two finger touch:
                // X -> rotation speed around Y axis (heading),
                // Y -> translation speed in view direction (camera-local Z axis)
                m_currentEuler.y += (mouseNormalised.x * 2.0f - 1.0f) * dt * m_headingSpeed;
				m_currentEuler.x += (mouseNormalised.y * 2.0f - 1.0f) * dt * -m_pitchSpeed;
                theTransform.localEulerAngles = m_currentEuler;
            } else if (Input.GetMouseButton(2))
            {
                // middle mouse button / three finger touch: "Slide"
                // X -> translation speed in camera-local X axis
                // Y -> translation speed in camera-local Y axis
                Vector3 translation;
                translation.x = (mouseNormalised.x * 2.0f - 1.0f) * dt * m_translationSpeed;
                translation.y = (mouseNormalised.y * 2.0f - 1.0f) * dt * m_translationSpeed;
                translation.z = 0.0f;
                theTransform.Translate (translation);
            }

 
		}

    }
}
