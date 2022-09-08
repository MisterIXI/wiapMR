# Installation: Unity
Mithilfe des [Unity hubs](https://unity3d.com/get-unity/download) muss erstmal Unity installiert werden.

# Unity Projekt erstellen
![](attachments/Pasted%20image%2020220831162402.png)
-> 
![](attachments/Pasted%20image%2020220831162915.png)
# Installation: PUN
Tutorial zur Referenz: [Introduction to the Multi-user capabilities tutorials - Mixed Reality | Microsoft Docs](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/tutorials/mr-learning-sharing-01)

### Schritt 1: Photon download
Jetzt laden wir PUN 2 über den Assetstore runter:
[Photon Unity 3D Networking Framework SDKs and Game Backend | Photon Engine](https://www.photonengine.com/pun)

![](attachments/Pasted%20image%2020220831163238.png)
![](attachments/Pasted%20image%2020220831163331.png)
![](attachments/Pasted%20image%2020220831163455.png)
![](attachments/Pasted%20image%2020220831163423.png)
![](attachments/Pasted%20image%2020220831163540.png)
![](attachments/Pasted%20image%2020220831163605.png)

### Schritt 2: Photon Account & Server einrichten
Nun müssen wir einen Account bei Photon-engine anlegen, um dort einen Server anzulegen.
Die Registrierung erfolgt hier: [Your Photon Cloud | Photon Engine](https://dashboard.photonengine.com/en-US/)

Nach der Anmeldung erstellen wir eine neue "App" (das ist der Server):
![](attachments/Pasted%20image%2020220831164136.png)

Beim Erstellen muss "PUN" ausgewählt und ein Name festgelegt werden.
![](attachments/Pasted%20image%2020220831164335.png)

Nun bei der neu erstellten App die ID kopieren:
![](attachments/Pasted%20image%2020220831164450.png)
![](attachments/Pasted%20image%2020220831164522.png)

Diese ID ist (ähnlich wie ein Streamkey bei Twitch und co) die komplette Authentifizierung. Dieser sollte also bei ernsten Projekten nicht öffentlich gemacht werden.


### Schritt 3: PUN setup wizard
Nun nutzen wir den PUN Setup Wizard. Falls dieser NICHT aufgegangen ist, oder anders verloren gegangen ist, findet man ihn folgenderweise: (Normalerweise öffnet sich das Fenster automatisch nach der PUN Installation, also können die nächsten zwei Bilder übersprungen werden)
![](attachments/Pasted%20image%2020220831165031.png)
![](attachments/Pasted%20image%2020220831165102.png)

Nun muss die oben kopierte ID in das automatisch geöffnete PUN Setup Fenster eingefügt werden:
![](attachments/Pasted%20image%2020220831165229.png)


### GESCHAFFT! Nun ist PUN bereit benutzt zu werden

