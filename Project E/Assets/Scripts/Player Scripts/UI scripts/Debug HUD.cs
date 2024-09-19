using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] Transform canvas;
    List<TextMeshProUGUI> ui_elements = new();
    StateFactory sf;
    string currentState = "first state";
    string lastState = "second state";
    string lasterState = "third state";
    void Start()
    {
        foreach (Transform t in canvas)
        {
            ui_elements.Add(t.GetComponent<TextMeshProUGUI>());

        }
        sf = GetComponent<PlayerStateMachine>().stateFactory;
        currentState = sf._currentState.ToString();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (currentState != sf._currentState.ToString())
        {
            lasterState = lastState;
            lastState = currentState;
            currentState = sf._currentState.ToString();
        }
        //ui_elements[0].text = (psm._getPCC.GetCurrentHorizontal()).ToString();


        //ui_elements[0].text = $@"H = {((int)psm._getPCC.GetCurrentHorizontal()).ToString()}     V = {((int)psm._getPCC.GetCurrentVertical()).ToString()}";
        ui_elements[0].text = lasterState + "  =>  " + lastState + "  =>  " + currentState;
        //ui_elements[1].text = (psm.isGrounded)?"is Grounded":"not grounded";
        //ui_elements[2].text = $@"H  =  {psm.moveDirectionX}     V  =  {psm.moveDirectionY}";
        //ui_elements[2].text = psm._getPCC._Direction.normalized.ToString() + "     ";



        //ui_elements[1].text = (psm._getPCC.GetCurrentVertical()).ToString();
        //ui_elements[2].text = psm._getPCC._TGTvelvocity.ToString() +"     "+ psm._getPCC._acceleration.ToString();

        //Debug.Log(ui_elements[0].name);
    }
}
