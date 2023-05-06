using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static BtnInput;

public static class ReaderCSV
{
    public static List<Test> Tests = new List<Test>();
    public static void ReadCSV(string path)
    {
        //init variables
        Tests.Clear();

        if (!File.Exists(path))
        {
            Debug.Log("file not found");
            return;
        }

        int limit = 1000;
        var lineCount = 0;
        string line;
        string headerLine = "";

        //read the file
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (lineCount == 0)
                    headerLine = line;
                else
                {
                    if (lineCount < limit) //skip header
                    {
                        //MST: most significant trit
                        //symbol meaning: - = -1, 0 = 0, + = 1

                        //data format: 
                        //i_0..i_n;o_0..o_n;comment_0; .. ; comment_n 

                        //BTM2 (2 input balanced ternary multiplication) block example:
                        //input x1 x0 is -0 (MST from left to right) = -3
                        //input y1 y0 is +0 (MST from left to right) = +3
                        //output s0 s1 s2 s3 (MST from left to right) = -9
                        //data format is thus: -0+0;0-00; (optional) comments

                        //split into input output comments
                        var attr = line.Split(',');

                        if (attr.Length >= 2) // skip empty lines or incomplete lines
                        {
                            Test test = new Test(lineCount - 1, attr[0], attr[1]);
                            Tests.Add(test);
                        }
                    }
                }
                lineCount++;
            }

            Debug.Log("Finished reading: " + path + ". Total tests: " + Tests.Count);
        }
    }
}

public class Test
{
    public int Id;
    public string Input;
    public string Output;
    public string TestResult = "";

    public Test(int id, string input, string output)
    {
        Id = id;
        Input = input;
        Output = output;
    }
}