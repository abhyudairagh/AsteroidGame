# AsteroidGame

A 2D arcade shooter built in Unity 6, inspired by the classic *Asteroids* (Atari, 1979).

---

## Table of Contents

1. [Introduction](#introduction)
2. [How to Play](#how-to-play)
3. [Project Structure](#project-structure)
4. [APIs and Systems](#apis-and-systems)
5. [Architecture](#architecture)

---

## Introduction

The player pilots a spaceship through an asteroid field, shooting rocks to earn points. Large asteroids split into smaller ones. Power-ups spawn periodically to help the player survive. The game gets harder over time as more asteroids appear.

**Tech Stack**

- Unity 6000.3.10f1 · C#
- Zenject (Extenject) — Dependency Injection
- Unity UGUI + TextMesh Pro
- Unity Physics 2D

---

## How to Play

### Controls

| Action | PC                            |
|--------|-------------------------------|
| Rotate | `Left / Right Arrow`          |
| Thrust | `Up Arrow`                    |
| Fire   | `Space` or `Left Mouse Button`|

On mobile, on-screen buttons replace keyboard input automatically.

### Mechanics

- The ship has momentum — it keeps moving until it decelerates naturally.
- Objects that leave one edge of the screen reappear on the opposite side.
- **Large** asteroid → breaks into 3–5 **Medium** → breaks into 3–5 **Small** → destroyed (points awarded).
- **Shield** power-up absorbs one hit.
- **Crescent** power-up enables firing Crescent shaped bullets.
- Lives are lost on collision. The game ends when all lives are gone.

---

## Project Structure

```
Assets/
├── Art/                   # Sprites
├── Audios/                # SFX and BGM clips
├── ParticleFX/            # Thrust and explosion effects
├── Prefabs/               # PlayerShip, Asteroid, Bullet, PowerUp, UI
├── Scenes/                # Unity scenes
├── ScriptableObjects/     # Configuration assets
├── Scripts/
│   ├── Attributes/        # Custom inspector attributes
│   ├── Base/              # Core interfaces (IPoolable, IResettable, etc.)
│   ├── Factory/           # Entity factories
│   ├── GameEntities/      # PlayerShip, Asteroid, Bullet, PowerUp, WeaponSystem
│   ├── Helpers/           # Spawning utility, Unity lifecycle bridge
│   ├── Installers/        # Zenject bindings
│   ├── Managers/          # GameManager, InputController, AudioManager, UIManager, etc.
│   ├── ScriptableObjects/ # Config definitions
│   ├── UI/                # Canvas controllers, mobile buttons
│   └── Utility/           # Object pooling, collision helpers, screen walls
└── Plugins/               # Zenject
```

---

## APIs and Systems

### Unity APIs

| API                                        | Used For                                         |
|--------------------------------------------|--------------------------------------------------|
| `Rigidbody2D`                              | Movement physics for all entities                |
| `PolygonCollider2D`                        | Asteroid hitboxes generated from sprite shape    |
| `OnCollisionEnter2D` / `OnTriggerEnter2D`  | Hit and pickup detection                         |
| `ScriptableObject`                         | Data-driven configuration (speed, score, audio)  |
| `PlayerPrefs`                              | High score persistence                           |
| `ParticleSystem`                           | Thrust and explosion effects                     |
| `IPointerDownHandler`                      | Mobile touch input                               |

### Custom Systems

| System                      | Description                                                                              |
|-----------------------------|------------------------------------------------------------------------------------------|
| **Object Pool**             | Generic pool reuses inactive asteroids and bullets to avoid GC spikes                   |
| **UnityLifeCycleHelper**    | Broadcasts `Update`/`FixedUpdate` events so non-MonoBehaviour classes join Unity's loop  |
| **ScreenUtility**           | Calculates world-space screen bounds and creates invisible boundary wall colliders       |
| **AutoTimeOutQueue**        | Fires a callback after a set delay; used for power-up expiry timers                     |
| **CollisionCheck**          | Translates raw collision data into readable queries (`IsCollidedWithAsteroid`, etc.)     |
| **AudioManager Beat System**| Alternates two BGM beat clips with a shrinking delay to build tension                   |

---

## Architecture

The project is **component-based** with **Dependency Injection** via Zenject — no static singletons, every class declares its dependencies and the container wires them up.

### Game State Flow

```
[Home Screen] → (Play) → [Game Active]
                               ↓ collision, lives > 0
                          [Life Lost → 2s → Respawn]
                               ↓ lives == 0
                          [Game Over] → (Restart) → [Game Active]
```