[Zurück zur Übersicht](0_Tutorial%20Intro.md)
# PhotonController handler
In PhotonController bauen wir den Ownership-Handler ein. Es muss über den PUNCallback auf eine Ownershiprequest reagiert werden. Das kann an beliebigen Orten passieren, aber die Empfehlung ist, das in einer Klasse gesammelt zu behandeln. Am einfachsten am gleichen Ort, wo die anderen Callbacks genutzt werden (also Raumjoin usw.).
```cs
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("Ownership request: " + targetView.ViewID + " from " + requestingPlayer.NickName);
        if (targetView.IsMine)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }
```
Um die Übergaben zu visualiesieren färben wir die "gestohlenen Objekte" beim vorigen Owner blau und beim neuen Besitzer rot ein:
```cs
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (previousOwner == PhotonNetwork.LocalPlayer)
        {
            targetView.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else if (targetView.IsMine)
        {
            targetView.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        }
        Debug.Log("Ownership transfered: " + targetView.ViewID + " from " + previousOwner.NickName);
    }
```
# Player collision
Wenn einer unserer Spieler mit einem der "Gegner" collidiert, fragen wir nach Ownership. Dafür müssen wir folgendes tun:
```cs
    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.tag == "Player")
            {
                if(!other.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("Trying to request ownership from: " + other.gameObject.name);
                    other.gameObject.GetComponent<PhotonView>().RequestOwnership();
                }
            }
        }
    }
```