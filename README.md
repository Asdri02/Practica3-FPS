# Práctica 3 - FPS en Unity

## Descripción

En esta práctica he desarrollado un prototipo de juego en primera persona (FPS) utilizando Unity. La idea principal era implementar las mecánicas básicas de este tipo de juegos: movimiento del jugador, control de cámara con el ratón, salto, apuntado, disparo, enemigos con comportamiento simple y un sistema de vida.

El resultado es un pequeño escenario en el que el jugador puede moverse, apuntar, disparar a enemigos y ganar la partida al eliminarlos todos.

---

## Jugador

El jugador está implementado con un `Rigidbody`, tal y como se pedía en la práctica.

El movimiento se hace con WASD y se calcula en función de hacia dónde está mirando el jugador. Esto hace que el control sea más natural, ya que siempre te mueves en la dirección en la que estás mirando.

El salto funciona comprobando si el jugador está tocando el suelo mediante un `GroundCheck`. Si está en el suelo y se pulsa espacio, se aplica una fuerza vertical.

La cámara se controla con el ratón:
- El movimiento horizontal rota el jugador.
- El movimiento vertical solo rota la cámara (a través de un objeto intermedio llamado `CameraPivot`).

Además, he limitado la rotación vertical para evitar que la cámara se gire completamente.

---

## Cámara

La cámara está separada en dos partes:
- El Player (rotación horizontal)
- El CameraPivot (rotación vertical)

Esto permite tener un control más limpio y evitar problemas típicos de rotación.

También he ajustado la sensibilidad del ratón para que el movimiento sea más cómodo y no tan brusco.

---

## Apuntado

El sistema de apuntado lo he implementado con el script `GunAim`.

El arma tiene dos posiciones:
- Posición normal (hip)
- Posición de apuntado (aim)

Cuando mantienes click derecho, el arma se mueve suavemente hacia el centro de la pantalla usando interpolación (`Lerp`). Al soltarlo, vuelve a su posición inicial.

También se reduce la sensibilidad del ratón al apuntar, para tener más precisión.

---

## Disparo

El disparo está hecho con `Raycast`, que es lo que se pedía.

En lugar de usar balas físicas, lanzo un rayo desde la cámara hacia delante. Si ese rayo impacta contra algo, compruebo si tiene el script `Health` y le aplico daño.

También he añadido un pequeño control de cadencia (`fireRate`) para que no se pueda disparar sin límite.

---

## Enemigos

Los enemigos utilizan `NavMeshAgent` para moverse por el escenario.

El comportamiento básico es:
- Detectan al jugador si está dentro de un rango.
- Lo persiguen.
- Se paran cuando están cerca.
- Le hacen daño cada cierto tiempo.

Además, giran hacia el jugador cuando están cerca para que el comportamiento sea más realista.

---

## Herencia de enemigos

Para la parte de herencia, he creado dos tipos de enemigos:

### EnemyFast
Es igual que el enemigo base, pero:
- Se mueve más rápido
- Detecta al jugador desde más lejos

### EnemyPatrol
Este enemigo no persigue siempre. En lugar de eso:
- Se mueve entre varios puntos de patrulla
- Si el jugador entra en su rango, empieza a perseguirlo

Para hacer esto he sobrescrito el método `UpdateBehaviour()` del enemigo base.

---

## Sistema de vida

El sistema de vida está implementado en el script `Health`.

Es reutilizable tanto para el jugador como para los enemigos.

Cuando un objeto recibe daño:
- Se reduce su vida
- Si llega a 0:
  - Si es un enemigo → se destruye
  - Si es el jugador → se reinicia la escena

---

## Interfaz (UI)

En la UI muestro:
- La vida del jugador
- El número de enemigos que quedan

Además, cuando eliminas a todos los enemigos aparece una pantalla de victoria.

---

## Pantalla de victoria

Cuando se eliminan todos los enemigos, se activa un panel con un mensaje de “Has ganado”.

He añadido un botón de reinicio que permite volver a empezar la partida. El botón realmente es invisible y está colocado encima del botón de la imagen para que funcione correctamente.

---

## Problemas y mejoras

Durante el desarrollo he tenido varios problemas, sobre todo con:
- La cámara, que a veces giraba sola
- El sistema de disparo
- El salto (detección del suelo)
- La UI (botón y superposición de elementos)

He ido corrigiendo estos problemas ajustando el código y la configuración en Unity.

Como mejora futura, se podría añadir:
- Animaciones
- Sonidos
- Mejores modelos para el arma y enemigos
- Sistema de daño más avanzado

---

## Conclusión

Esta práctica me ha servido para entender mejor cómo funcionan los FPS en Unity, especialmente el uso de Rigidbody, Raycast, NavMesh y la organización del código mediante scripts reutilizables y herencia.

Además, me ha ayudado a mejorar en la parte de debugging, ya que muchos de los problemas no eran solo de código, sino también de configuración en el Inspector.
