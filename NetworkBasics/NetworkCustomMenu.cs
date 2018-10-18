using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkCustomMenu : NetworkBehaviour {

    public Text ipTextBox;
    public Text portTextBox;
    
    public void StartServer()
    {
        //note that you can do this directly from hooking the button to the network manager if you want
        NetworkManager.singleton.StartHost();
        Debug.Log(NetworkManager.singleton.networkAddress);

    }

    public void JoinAsClient()
    {
        //You'll probably want to do a more robust check that the ip is either localhost or an ip format here
        if (ipTextBox.text != null && ipTextBox.text.Length > 0)
        {
            NetworkManager.singleton.networkAddress = ipTextBox.text;
            //again, we need a more careful check that we have a valid value for port here ideally
            int x;
            int.TryParse(portTextBox.text, out x); //usually the port will just be 7777 so this part isn't really nescessary unless you're specifying the port on a server
            
            NetworkManager.singleton.networkPort = x;
            //this is the actual code to start the client
            NetworkManager.singleton.StartClient();
        }
    }
}
