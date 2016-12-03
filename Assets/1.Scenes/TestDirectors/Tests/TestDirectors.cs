using UnityEngine;
using System.Collections.Generic;
using System;

public class TestDirectors : MonoBehaviour {

    public List<BaseTest> testList = new List<BaseTest>();
    public Test_1 test1;
    public Test_2 test2;
    public Test_3 test3;

    void Awake()
    {
        testList.Add(test1);
        testList.Add(test2);
        testList.Add(test3);
        //get component
        for (int i = 0; i < testList.Count; i ++)
        {
            Debug.Log(testList[i].GetType().ToString());
            //BaseTest tempTest = FindObjectOfType < testList[i].GetType() > ();
        }
    }	

    void Update () {

    }
}
