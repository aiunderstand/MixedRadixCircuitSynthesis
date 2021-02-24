using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BtnInput;
using UnityEngine.UI;
using static SaveCircuit;

public class ComponentGenerator : MonoBehaviour
{
    public GameObject terminalInputPrefab;
    public GameObject terminalOutputPrefab; //we can refactor and merge this with input to "terminal"
    public TextMeshProUGUI title;
    public RectTransform body;
    public GameObject infoBtn;
    public int Size;
    Color _colorTernary = new Color(255, 0, 211); //we should define it in 1 place instead of 2, see btninput
    Color _colorBinary = new Color(0, 214, 255);//we should define it in 1 place instead of 2, see btninput
    public void Generate(string name, List<RadixOptions> inputs, List<string> inputLabels, List<RadixOptions> outputs, List<string> outputLabels, AbstractionLevel level)
    {
        title.text = name;

        int count = inputs.Count > outputs.Count ? inputs.Count : outputs.Count;
        Size = 30 + (count-1) *20;

        body.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Size);
        title.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Size+10);

        if (level == AbstractionLevel.ComponentView)
        {
            infoBtn.transform.localPosition = new Vector3(0, Size / 2);
        }

        for (int i = 0; i < inputs.Count ; i++)
        {
            var input = GameObject.Instantiate(terminalInputPrefab);
            input.transform.SetParent(this.transform);
            input.transform.localScale = new Vector3(1, 1, 1);
            input.transform.localPosition = new Vector2(-65, (-10 * (inputs.Count-1)) + i*20);
            input.GetComponent<BtnInput>()._portIndex = i;
            input.GetComponent<BtnInput>().isOutput = false;



            if (inputs[i] == RadixOptions.Binary)
            {
                input.transform.GetChild(0).GetComponent<Image>().color = _colorBinary;

                if (level == AbstractionLevel.MenuView)
                    input.GetComponent<BtnInput>().label.text = "B";
                else
                    input.GetComponent<BtnInput>().label.text = inputLabels[i];
            }
            else
            {
                input.transform.GetChild(0).GetComponent<Image>().color = _colorTernary;

                if (level == AbstractionLevel.MenuView)
                    input.GetComponent<BtnInput>().label.text = "T";
                else
                    input.GetComponent<BtnInput>().label.text = inputLabels[i];
            }
        }

        for (int i = 0; i < outputs.Count; i++)
        {
            var output = GameObject.Instantiate(terminalOutputPrefab);
            output.transform.SetParent(this.transform);
            output.transform.localScale = new Vector3(1, 1, 1);
            output.transform.localPosition = new Vector2(65, (-10 * (outputs.Count - 1)) + i * 20);
            output.GetComponent<BtnInput>()._portIndex = i;
            output.GetComponent<BtnInput>().isOutput = true;

            if (outputs[i] == RadixOptions.Binary)
            {
                output.transform.GetChild(1).GetComponent<Image>().color = _colorBinary;

                if (level == AbstractionLevel.MenuView)
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "B";
                    output.GetComponent<BtnInput>().label.text = "B";
                }
                else
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = outputLabels[i];
                    output.GetComponent<BtnInput>().label.text = outputLabels[i];
                }
            }
            else
            {
                output.transform.GetChild(1).GetComponent<Image>().color = _colorTernary;

                if (level == AbstractionLevel.MenuView)
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "T";
                    output.GetComponent<BtnInput>().label.text = "T";
                }
                else
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = outputLabels[i];
                    output.GetComponent<BtnInput>().label.text = outputLabels[i];
                }
            }
        }
    }      
}
