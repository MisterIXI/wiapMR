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

    public override void OnConnectedToMaster()

    {

    }

    public override void OnJoinedRoom()

    {

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)

    {

    }

    public override void OnLeftRoom()

    {

    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)

    {

    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)

    {

    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)

    {

    }

}
```

Beachte die Imports und Parentklasse & Interface.

Wie oben erwähnt brauchen wir einen Raum um tatsächlich verbunden zu sein. Daher verbinden wir uns immer den gleichen Raum direkt nach Verbindung zum Master. Das ist nur für simple Projekte empfohlen, ansonsten sollte man das über Hauptmenü, auswahl usw zu machen.
```cs
public override void OnConnectedToMaster()

{

    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);

}

public override void OnJoinedRoom()

{

    // called when we join a room

}

public override void OnPlayerEnteredRoom(Player newPlayer)

{

    // called when a player enters the room

}

public override void OnLeftRoom()

{

    // called when we leave the room

}
```
# Spielerkonvertierung
