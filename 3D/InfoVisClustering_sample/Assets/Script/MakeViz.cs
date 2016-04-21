using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MakeViz : MonoBehaviour {
    public float instanceScale = 1.0f;

    public int dataColumnX = 0;
    public int dataColumnY = 1;
    public int dataColumnZ = 2;
    public int dataColumnColour = 2;

	public float posScaleX = 10f;
	public float posScaleY = 10f;
	public float posScaleZ = 10f;

	private float scaleCheck = 1f;
	private float scaleCheckOld = 1f;

	public Slider sX;
	public Slider sY;
	public Slider sZ;

	public Dropdown dX;
	public Dropdown dY;
	public Dropdown dZ;
	public Dropdown dColor;

    // These cache the corresponding public dataColumn fields
    // so that we can detect changes made in Editor in Update()
    int m_dcX, m_dcY, m_dcZ, m_dcColour;

    ArrayList m_objectArray;
    ParseDB m_dataSource;

	// Use this for initialization
	void Start () {
        m_dataSource = GetComponent<ParseDB>();
        if (m_dataSource != null)
        {
            make3DCloud();
            update3DCloud(dataColumnX, dataColumnY, dataColumnZ,dataColumnColour);
			List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
			for (int i = 0; i < m_dataSource.GetColumnCount (); i++) {
				options.Add (new Dropdown.OptionData(i.ToString()));
			}
			dX.ClearOptions ();
			dY.ClearOptions ();
			dZ.ClearOptions ();
			dColor.ClearOptions ();

			dX.AddOptions (options);
			dY.AddOptions (options);
			dZ.AddOptions (options);
			dColor.AddOptions (options);
        }
	}
	
	// Update is called once per frame
	void Update () {
		posScaleX = sX.value;
		posScaleY = sY.value;
		posScaleZ = sZ.value;
		scaleCheckOld = scaleCheck;
		scaleCheck = posScaleX + posScaleY + posScaleZ;

		dataColumnX = dX.value;
		dataColumnY = dY.value;
		dataColumnZ = dZ.value;
		dataColumnColour = dColor.value;

        // Update coloumn assignment if one or more fields have changed
		if ( (dataColumnX != m_dcX) || (dataColumnY != m_dcY) || (dataColumnZ != m_dcZ) || (dataColumnColour != m_dcColour) || (scaleCheck != scaleCheckOld))
            update3DCloud(dataColumnX, dataColumnY, dataColumnZ, dataColumnColour);
    }

    void update3DCloud(int colX, int colY, int colZ,int colColour)
    {
        m_dcX = colX;
        m_dcY = colY;
        m_dcZ = colZ;
        m_dcColour = colColour;

        float minX, maxX, minY, maxY, minZ, maxZ, minColour, maxColour, scaleX, scaleY, scaleZ, scaleColour;
        minX = maxX = minY = maxY = minZ = maxZ = minColour = maxColour = 0.0f;
        scaleX = scaleY = scaleZ = scaleColour = 1.0f;

        if (m_dataSource.GetDataValid())
        {
            m_dataSource.GetColumnMinMaxValues(ref minX, ref maxX, colX);
            m_dataSource.GetColumnMinMaxValues(ref minY, ref maxY, colY);
            m_dataSource.GetColumnMinMaxValues(ref minZ, ref maxZ, colZ);
            m_dataSource.GetColumnMinMaxValues(ref minColour, ref maxColour, colColour);

            scaleX = 1.0f / (maxX - minX);
            scaleY = 1.0f / (maxY - minY);
            scaleZ = 1.0f / (maxZ - minZ);
            scaleColour = 1.0f / (maxColour - minColour);

            // Update position and colour of the 3D cloud objects
            for (int i = 0; i < m_dataSource.numRows; i++)
            {
                float newPosX = 0.0f;
                float newPosY = 0.0f;
                float newPosZ = 0.0f;
                float newColour = 0.0f;
                m_dataSource.GetArrayValue(ref newPosX, i, colX);
                newPosX = (newPosX - minX) * scaleX * posScaleX;
                m_dataSource.GetArrayValue(ref newPosY, i, colY);
                newPosY = (newPosY - minY) * scaleY * posScaleY;
                m_dataSource.GetArrayValue(ref newPosZ, i, colZ);
                newPosZ = (newPosZ - minZ) * scaleZ * posScaleZ;
                m_dataSource.GetArrayValue(ref newColour, i, colColour);
                newColour = (newColour - minColour) * scaleColour;

                // Compute RGB colour by interpolating between red and green
                Color newRGB;
                newRGB.r = 1.0f - newColour;
                newRGB.g = newColour;
                newRGB.b = 0.0f;
                newRGB.a = 1.0f;

                GameObject obj = (GameObject)m_objectArray[i];

                obj.transform.position = new Vector3(newPosX, newPosY, newPosZ);
                obj.transform.localScale = new Vector3(instanceScale, instanceScale, instanceScale);

                // Set colour of object
                MeshRenderer rdr = obj.GetComponent<MeshRenderer>();
                if (rdr != null)
                    rdr.material.color = newRGB;
            }
        }
    }

    // construct a point cloud from database entries
    void make3DCloud ()
    {
        int rowCount = m_dataSource.GetRowCount();
        m_objectArray = new ArrayList();

        for (int i=0;i<rowCount;i++)
        {
            GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            string objectName = string.Format("Instance_{0}", i);
            newObject.name = objectName;
            newObject.transform.parent = gameObject.transform;
            m_objectArray.Add(newObject);
        }
    }

}
