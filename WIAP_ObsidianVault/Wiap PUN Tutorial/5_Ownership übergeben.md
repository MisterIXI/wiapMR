# PhotonController handler
In PhotonController bauen wir den Ownership handler ein. Es muss über den PUNCallback auf eine Ownershiprequest reagiert werden. Das kann an beliebigen Orten passieren, aber die Empfehlung ist das in einer Klasse gesammlt zu handeln. Am einfachsten am gleichen ort wo die anderen Callbacks genutzt werden (also Raumjoin usw)
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
Wenn einer unserer Spieler mit einem der "gegner" collidiert fragen wir nach ownership. Dafür müssen wir folgendes tun:
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