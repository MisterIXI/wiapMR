Das Projekt "**Wiap_PUN_Tutorial**" auf oberster Ebene ist das fertige PUN Tutorial Unity Projekt als Referenz.

Dieses Tutorial soll eine Einführung in PUN bieten. Folgende Themen werden behandelt:

## 1. [Vorbereitung der Unity Umgebung mit dem PUN Package](1_PUN%20Installation.md)
## 2. [Bauen eines primitven Projekts als Grundstein für die späteren Punkte](2_Mini%20Projekt%20vorbereitung.md)
## 3. [Skripte für das lokale Projekt](3_Lokaler%20Ansatz.md)
1. Player controller
2. Input für "player in project" (altes input system)
3. "Destroy on collision" skript
## 4. ["PUNifikation" des Projekts - Ausbau auf Multiplayer](4_PUN%20Ansatz%20(2Player).md)
1. Erklärungen von Grundsätzen
	1. Grundlagen
	2. Kostenmodell
	3. Room/Server modell
	4. Objektpersistenz
	5. Photonview
	6. Ownership Konzept
	7. "RPC" Konzept
2. Methoden eines PUN Controllers
	1. Connection zum Master aufbauen
	2. Room createn/joinen
	3. Objekt via PhotonNetwork instantiieren
3. Spielerupgrade
	1. Skript-anpassung
	2. zusätzliche Komponenten
	3. (Tagging & Prefabbing)
4. "Destroy Skript"-Update auf PhotonNetwork-Basis
## 5. [Ownership management](5_Ownership%20übergeben.md)
1. Ownership-transfer
	1. Reagieren auf Ownership-request
	2. Reagieren auf Ownership-transfer
2. Bei Kollision die Ownerhip einer fremden Photonview anfragen