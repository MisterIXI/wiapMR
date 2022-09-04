# Erstellung von "Arena"
### Boden
Neuer "Cube" 3D primitive:
![](attachments/Pasted%20image%2020220831165710.png)
Wir nennen diesen "Floor".

Nun skalieren wir den auf 20,1,20:
![](attachments/Pasted%20image%2020220831165835.png)

### Wände
Nun erstellen wir noch einen Cube den wir "Wall" nennen.
Den nun markieren und `CTRL + D` 3x betätigen. Nun müssten wir insgesamt Folgende Objekte haben:
- Main Camera
- Directional Light
- Floor
- Wall
- Wall (1)
- Wall (2)
- Wall (3)

Den Walls geben wir jetzt nacheinander folgende Werte im Inspector wie oben zu sehen:
Wall:
```
Position: 0   1   10
Rotation: 0   0   0
Scale:    20  1   1
```
Wall (1):
```
Position: 0   1   -10
Rotation: 0   0   0
Scale:    20  1   1
```
Wall (2):
```
Position: 10  1   0
Rotation: 0   90  0
Scale:    20  1   1
```
Wall (3):
```
Position: -10 1   0
Rotation: 0   90  0
Scale:    20  1   1
```


Damit müsste es ungefähr so aussehen:
![](attachments/Pasted%20image%2020220831170823.png)

Die Kamera passen wir auch direkt an:
```
Position: 0   10  -12
Rotation: 50  0   0
Scale:    1   1   1
```
### Ordnerstruktur
Wir erstellen nun zwei neue Ordner in unserer Projektstruktur:
![](attachments/Pasted%20image%2020220831170931.png)
Rechtsklick auf "Assets"; Hier auf Create->Folder
Dann einmal den namen "\_Material" und "\_Scripts" erstellen.

### Materialien
Wir erstellen jetzt Zwei Materialien die wir "Rot" und "Blau" nennen:
![](attachments/Pasted%20image%2020220831171202.png)

Danach geben wir denen direkt eine simple Albedo Farbe von Blau und Rot jeweils:
![](attachments/Pasted%20image%2020220831171440.png)

Mit drag and drop dann den 4 Wänden einzeln zuweisen:
![](attachments/Pasted%20image%2020220831171527.png)

(Optional geben wir dem Boden noch ein extra Material namens "Boden" das leicht gräulich ist)

### SpielerObjekt erstellung
Wir legen ein leeres GameObject an: 
![](attachments/Pasted%20image%2020220831171722.png)
Das nennen wir Spieler und geben über RMB->3D Object->Cylinder einen Cylinder und einen Cube als Body und positionieren den über die Gizmos folgendermaßen:
![](attachments/Pasted%20image%2020220831171826.png)

Dem Spieler geben wir nun einen Rigibody und frieren die Rotation der X, Y und  Z Achsen ein:
![](attachments/Pasted%20image%2020220831172253.png)
![](attachments/Pasted%20image%2020220831173948.png)
Die kann kurz getestet werden indem die Szene kurz in den Play modus gesetzt wird. Wenn der Spieler über dem Spielfeld schwebt sollte er runterfallen und vom Boden gestoppt werden. (Außerdem nicht umfallen)