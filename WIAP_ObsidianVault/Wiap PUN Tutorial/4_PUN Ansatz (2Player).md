# Erklärungen
### Aufbau
PUN funktioniert so dass sich jeder Client mit dem PUN server verbindet. Der Server kümmert sich ausschließlich um das verbinden der Spieler und verteilen von nachrichten. Die eignetliche Rechenarbeit läuft verteilt auf allen clients.
Da man lokal gerne "echtzeit" physics hätte wird das standardmäßig lokal auch berechnet und dann mit online daten "korrigiert". Daher muss man aufpassen was wie und von wem bewegt wird.

### Kostenlos?
Man darf kostenlos 20 CCUs (Concurrent Users) in Photon haben und beliebige Server. Man zahlt Lizenzen wenn man mehr User haben will.

### Room oder Server?
In PUN hat man zunächst nur eine Verbindung zum Server. Da gibt es auch vorgefertigte Dinge wie Lobby, Raumlisten usw.
Das heißt wenn man sich verbindet müsste man auch noch einem Raum erstellen bzw einem beitreten damit "normaler" Ablauf stattfindet.

### Objekt persistenz
Wenn man Objekte spawnt über das PhotonNetwork werden standardmäßig alle Objekte gelöscht wenn der Ersteller den Raum verlässt. Das kann man aber auch anpassen.
Wenn alle Spieler den Raum verlassen wird dieser ebenfalls geschlossen und alle Objekte gelöscht.

### "Photonview"
Was ist diese "Photonview"?: Diese komponente markiert jedes Spielobjekt in Unity als PUN objekt und mit dem skript passieren alle PUN funktionen. Wenn man ein Objekt synchronisieren will braucht man dafür irgenwo eine photonview.
Wenn man auch zB position/rotation/skalierung synchronisieren will brauchtm man zusätzlich zB ein "Photon Transform view". Diese Komponenten setzen alle eine photonview auf dem gleichen Objekt voraus.
Die Objekte werden identifiziert anhand der "viewID" auf der photonview. Wenn eine ID mehrfach vergeben wird werden Fehler geworfen. 

### Ownership
Durch die Verteilte Natur von PUN existiert jedes synchronisierte Objekte bei jedem Spieler separat. Jeder hat seine eigene Instanz. Nur über die viewID werden die in Relation gebracht.
Nun was passiert wenn mehrere Player ihre Instanz gleichzeitig versuchen zu bewegen? Wer gewinnt?
In PUN hat jedes Objekt einen "Owner" und nur der darf änderungen vornehmen. Wenn ein nicht-owner ein gesynctes Objekt bewegt wird das bei ihm lokal zwar sichtbar, aber bei niemand anderem. Sobald dann vom Owner updates bezüglich des Objektes kommt wird das bei der lokalen Änderung auch korrigiert und es springt zurück. Diesen Effekt nennt man auch "Rubberbanding".
Das heißt jedes Objekt das wir bewegen wollen, aber nicht uns gehört müssen wir die Ownership bekommen. Dafür bietet PUN auch standardmethoden an und ein ganzes Konzept dahinter. Den lokalen Ansatz von Seite 3 muss man auch modifizieren da eben dieses Ownership konzept nicht integriert ist. Würde man bspw nur lokal sein Objekt löschen würde das nicht automatisch bei anderen auch gelöscht werden. Bei Erstellen und Löschen von Objekten haben wir auch funktionen vom "PhotonNetwork" damit das repliziert wird.
Wollen wir jetzt eigene Änderungen auf andere Clients verteilen brauchen wir die "RPC" methodik.

### RPC "Remote Procedure Call"
RPC in PUN gibt einem die Möglichkeit Methoden übers netzwerk bei anderen Spielern auszuführen. Damit kann man beispielsweise bei jedem Spieler nicht synchronisierte Objekte verschicken.
Man muss hierbei beachten dass man nicht alles als methodenparamater mitgeben darf. Als vorraussetzung muss es serialisierbar sein und von PUN unterstützt sein. Alternativ legt man einen Custom type an.
[Serialization in Photon | Photon Engine](https://doc.photonengine.com/en-us/realtime/current/reference/serialization-in-photon)
Ansonsten kann man daten via RPC zwischen Clients syncen. Man muss aber auch ein maximum von 512kb pro message beachten: [Any way to send large data via rpc's without it kicking us offline? — Photon Engine](https://forum.photonengine.com/discussion/13276/any-way-to-send-large-data-via-rpcs-without-it-kicking-us-offline)


# PUN Controller
Wir müssen zuerst ein neues Skript erstellen namens "PUN Controller" (name frei wählbar).
Den nutzen wir als beispiel wie man raum und ownership management betreibt.
Als Übersicht hier die noch leeren Methoden:
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PUNController : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public GameObject playerPrefab;
    
    private void Start()
    {
        Debug.Log("Attempting to connect to Master Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        // called when we join a room
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered room: " + newPlayer.NickName);
        // called when a player enters the room
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
        // called when we leave the room
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("Ownership request: " + targetView.ViewID + " from " + requestingPlayer.NickName);
    }
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership transfered: " + targetView.ViewID + " from " + previousOwner.NickName);
    }
    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("Ownership transfer failed: " + targetView.ViewID + " from " + senderOfFailedRequest.NickName);
    }

    private void Update()
    {
    }
}

```

Beachte das Playerprefab, die Imports und Parentklasse & Interface.

Nun Füllen wir die meisten Methoden mit Leben:

Zunächst müssen wir uns überhaupt mit dem Masterserver verbinden:
```cs
    private void Start()
    {
        Debug.Log("Attempting to connect to Master Server...");
        PhotonNetwork.ConnectUsingSettings();
    }
```

Nach der Serververbindung müssen wir uns noch mit einem Raum verbinden:
```cs
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }
```
Hier verbinden wir uns mit einem Raum namens "Room" und begrenzen den direkt auf maximal 4 Spieler. Durch den hardcoded namen kommen alle clients immer in denselben Raum. (Der raum wird resetted sobald kein Spieler mehr in dem Raum verbunden ist)

Nun bauen wir noch eine möglichkeit ein weitere Objekte zu spawnen:
```cs
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 2, Quaternion.identity);
            }
        }
    }
```

# Spielerkonvertierung
Damit wir nicht versuchen "fremde" Spieler zu steuern müssen wir zunächst die Steuerung von der Ownership abhängig machen.
Wir stellen unsere "Monobehaviour" Ableitung auf "MonobehaviourPun" um für bequeme zusatzfunktionen. Dies sollte nur gemacht werden wenn das gleiche Gameobject auch eine photonview besitzt.

(nur die geänderten Zeilen)
```cs
[...]

using Photon.Pun;

public class PlayerController : MonoBehaviourPun

{

	[...]
    void FixedUpdate()

    {

        if (photonView.IsMine)

        {

			[...]

        }

    }

}
```

# Spielerprefab
### Komponenten
Damit der Spieler gesynct wird brauchen wir eine photonview für den generellen sync und ein "photon transform view" für den Position/rotations sync.
![](attachments/Pasted%20image%2020220904151346.png)
![](attachments/Pasted%20image%2020220904151349.png)
Wir stellen die Photonview noch auf "Ownershiptransfer: Request" ein:
![](attachments/Pasted%20image%2020220904153923.png)


### Tag
Für später wollen wir die Objekte identifizieren können. Daher vergeben wir dem Objekt das Tag "Player".
![](attachments/Pasted%20image%2020220904153718.png)

### Prefab
Jetzt ziehen wir den Spieler in den Inspector um ein Prefab anzulegen und löschen ihn danach aus der Szene:
![](attachments/Pasted%20image%2020220904154244.png)
Wir legen dafür noch einen "\_Prefab\\Resources" Ordner an. Alle photon prefabs müssen in einem "Resources" Ordner liegen. Sonst können sie nicht live geladen werden.

# Photoncontroller
Wir legen ein leeres Gameobject an, geben dem das PhotonControllerscript und ziehen das erstellte Playerprefab rein:
![](attachments/Pasted%20image%2020220904154052.png)


# Destroy
Wir passen noch unser "DeathCollider" an dass er auch über das Netzwerk die Gameobjects richtig zerstört. "Normales" lokales zerstören hinterlässt Probleme bei anderen Spielern da diese dann die Spieler besitzen.
Dazu verwenden wir einfach das PhotonNetwork.Destroy statt dem normalen:
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DeathCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        PhotonNetwork.Destroy(other.gameObject);
    }
}
```

# Testen
Nun kann man diesen sanften Multiplayer ansatz testen:
`CTRL + P` für build & run und play mode aktivieren (oder zwei mal builden). Damit kann man zwei spieler einfach simulieren.

Mit WASD kann man seinen eigenen Spieler bewegen, mit Q/E rotieren und mit Leertaste einen neuen Spawnen.

