# PlayerController
Wir erstellen jetzt einen Controller für die lokale und "normale" Steuerung des Players.

```cs
using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour

{

    private Rigidbody _rb;

    public float speed = 10f;

    void Start()

    {

        _rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate()

    {

        // horizontal movement

        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        _rb.AddForce(movement * speed);

        //rotate around y axis

        if (Input.GetAxis("Fire1") > 0)

        {

            transform.RotateAround(transform.position, Vector3.up, 10 * speed * Time.deltaTime);

        }

        if (Input.GetAxis("Fire2") > 0)

        {

            transform.RotateAround(transform.position, Vector3.down, 10 * speed * Time.deltaTime);

        }

    }

}
```
Hier ein simpler physik basierter bewegunsansatz.
# Input anpassung
Damit die Fire Knöpfe richtig reagieren müssen wir noch in den Projectsettings die beiden Tasten definieren (wir nehmen mal Q und E damit die gut zu den WASD passen):
![](attachments/Pasted%20image%2020220831174856.png)
![](attachments/Pasted%20image%2020220831174958.png)

# Kill skript für Todeszonen (zB Wände)
Um die Wände als Todeszonen zu deklarieren können wir ein simples Skript schreiben wie folgt:
```cs
using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class DeathCollider : MonoBehaviour

{

    private void OnCollisionEnter(Collision other) {

        Destroy(other.gameObject);

    }

}
```
Damit löschen wir alles was mit uns kollidiert

### Zuweisung
Wir weisen den PlayerController auf das "Player" objekt zu und das Killskript an alle 4 wände.
![](attachments/Pasted%20image%2020220831174332.png)


# Playmode & Quickbuild
Um die App zu testen brauchen wir später zwei Instanzen, daher hier auch zum testen direkt beide:
Der normale Unity play mode -> einfach mit dem Play button oben das Spiel starten.
Automatisch build & run: `CTRL + B`
Der build fragt nach einem Ordner. Dafür legen wir einen neuen Ordner "Build" an und wählen diesen aus:
![](attachments/Pasted%20image%2020220831174634.png)


