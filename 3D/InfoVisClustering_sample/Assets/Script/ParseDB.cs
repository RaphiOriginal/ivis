﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ParseDB : MonoBehaviour {
    public string filePath;
    public int numRows;
    public int numColumns;

    float[,] m_dataArray;
    bool m_dataValid = false;
	List<Dropdown.OptionData> options;

	// Use this for initialization
	void Start () {
        if (filePath.Length > 0)
        {
            if (ReadFileToArray(filePath))
            {
                numRows = m_dataArray.GetLength(0);
                numColumns = m_dataArray.GetLength(1);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool GetDataValid()
    {
        return m_dataValid;
    }

	public List<Dropdown.OptionData> GetOptionData() {
		return options;
	}

    public int GetRowCount()
    {
        return m_dataArray.GetLength(0);
    }

    public int GetColumnCount()
    {
        return m_dataArray.GetLength(1);
    }

    public bool GetColumnMinMaxValues(ref float minVal, ref float maxVal, int col)
    {
        if (m_dataValid && (col >= 0) && (col < numRows) )
        {
            minVal = float.MaxValue;
            maxVal = float.MinValue;

            for (int i=0; i<numRows; i++)
            {
                if (m_dataArray[i, col] < minVal)
                    minVal = m_dataArray[i, col];

                if (m_dataArray[i, col] > maxVal)
                    maxVal = m_dataArray[i, col];
            }
        }
        return m_dataValid;
    }

    public bool GetArrayValue(ref float value, int row, int col)
    {
        if (m_dataValid)
            value = m_dataArray[row, col];

        return m_dataValid;
    }

    bool ReadFileToArray(string fp)
    {
        int lineCount = 0;
        string line;

        m_dataValid = false;

        // construct StreamReader for the file given
        System.IO.StreamReader file = new System.IO.StreamReader(fp);

        // first pass: determine number of lines in source file
        while (file.ReadLine() != null)
            lineCount++;

		int rowCount = lineCount - 1;
        int columnCount = 0;
        lineCount = 0;

        // reset file stream to beginning for actual reading of data
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

		string title;
		if((title = file.ReadLine()) != null){
			string[] names = title.Split (',');
			options = new List<Dropdown.OptionData> ();
			foreach (var n in names) {
				options.Add (new Dropdown.OptionData(n));
			}

			//TODO Get Titles for Menu
		}

        while ((line = file.ReadLine()) != null)
        {
			string[] items = line.Split (',');

            if (items.Length > 0)
            {
                if (lineCount == 0)
                {
                    columnCount = items.Length;
                    m_dataArray = new float[rowCount, columnCount];
                }

                for (int i=0;i<columnCount;i++)
                {
                    m_dataArray[lineCount, i] = float.Parse(items[i], System.Globalization.NumberStyles.Any);
                }

                lineCount++;
            }
        }

        if ((columnCount > 0) && (rowCount > 0))
            m_dataValid = true;

        file.Close();

        return m_dataValid;
    }

    

}
