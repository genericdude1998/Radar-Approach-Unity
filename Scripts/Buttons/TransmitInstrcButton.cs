using UnityEngine;

public class TransmitInstrcButton : MonoBehaviour //  this is attached to the button to get it to allow to call the transmit info method
{
    public bool b_over = false;
   
    public void OnMouseEnter()
    {
         b_over = true; 
    }
   

}

