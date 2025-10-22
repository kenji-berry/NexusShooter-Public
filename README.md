# NexusShooter

**NexusShooter** is a fast‑paced, Doom/Quake‑inspired **PvE first‑person shooter** built in Unity. Players blast through enemy‑filled rooms, chaining movement and combat together in a fluid, high‑speed loop. Designed as a **university group project**, it showcases advanced FPS mechanics, roguelike‑style upgrade systems, and collaborative development workflows.

---

## ✨ Features

- **Roguelike‑Inspired Upgrade Tree**
  - Earn points through levels to unlock upgrades like increased speed, damage, or jump height.
  - Each playthrough feels unique, with different builds and difficulty scaling.

- **Combat & Weapons**
  - Multiple weapons with distinct behaviours.
  - Difficulty levels that adjust enemy strength and pacing.
  - Bunny‑hopping and fast movement mechanics inspired by Quake.

- **Level Design**
  - Each team member designed their own level, resulting in a diverse set of arenas.
  - Levels reflect individual design styles, creating variety across playthroughs.

- **Player Systems & Settings**
  - Menus for crosshair customisation, audio settings, and upgrades.
  - Inventory and upgrade menus integrated into gameplay flow.

- **Testing & Iteration**
  - User testing with course peers led to new features: additional guns, enemy types, and more dynamic levels.

---

## 🛠 Tech Stack

- **Engine:** Unity (2023.1)  
- **Language:** C#  
- **Unity Features:** Prefabs, ScriptableObjects, modular scene design  
- **Version Control:** GitHub with collaborative branching, code reviews, and conflict resolution  

---

## 🎮 Controls

**Movement**
- `WASD` – Move  
- `Spacebar` – Jump  
- `Shift` – Walk  
- `C` – Crouch  

**General**
- `E` – Use / Interact  

**Combat**
- `Mouse 1` – Shoot  
- `1/2/3` – Select weapons  

**UI**
- `Q` – Weapon inventory  
- `Tab` – Item inventory  
- `K` – Upgrade menu  

---

## 📂 Project Structure

The project follows a standard Unity layout with a few key folders:

- **Assets/** – Main Unity folder containing all game content  
  - **Scripts/** – C# scripts for gameplay (player, enemies, weapons, upgrades, UI)  
  - **Prefabs/** – Reusable objects like enemies, pickups, and weapons  
  - **Scenes/** – Game levels and menus  
  - **Audio/** – Sound effects and music  
  - **Materials/** – Materials and shaders  
  - **Animations/** – Animation controllers and clips  
  - **UI/** – Menus, HUD, and interface elements  
  - **...** – Additional Unity folders (art, input, post‑processing, resources, etc.)

- **Packages/** – Unity packages and third‑party assets (e.g. TextMesh Pro)  
- **README.md, .gitignore** – Documentation and version control setup  

---

## 👥 Team & Collaboration

- Shared project management between **Kenji Berry** and **Armin Shahnami**, balancing gameplay systems and supporting features.  
- Collaboration through GitHub with **code reviews**, **task division**, and resolution of Unity merge conflicts.  
- Each member contributed a unique level, ensuring variety and personal design signatures.  

## Creators
- [Kenji Berry](https://github.com/kenji-berry)  
- [Armin Shahnami](https://github.com/ashahnami)  
- [Ivan Konakotin](https://github.com/IvanSeagull)  
- [Ralph Zaatar](https://github.com/Ralphzaatar)  
---
## 👤 My Personal Contributions

- Designed and implemented the **upgrade tree system** (speed, damage, jump height, etc.).  
- Built the **difficulty scaling system**.  
- Developed the **enemy behaviours** and **weapon system**.  
- Created one of the game’s levels.  
- Designed and implemented **menus and settings** (crosshair customisation, audio controls, upgrade menus).  
- Shared project management responsibilities, coordinating features and supporting teammates.  

---
## 🚀 Getting Started

Here’s how to set up and run **NexusShooter** locally.

### 1. Prerequisites
Make sure you have the following installed:
- [Unity Hub](https://unity.com/download)
- Unity Editor **2023.1.x** (LTS recommended)
- Git

### 2. Clone the Repository
Run this command in your terminal:
```bash
git clone https://github.com/kenji-berry/NexusShooter-Public.git
```

### 3. Open the Project in Unity
- Open **Unity Hub**  
- Click **Add Project from Disk**  
- Select the cloned `NexusShooter-Public` folder  
- Open it with **Unity version 2023.1.x**  
- ⚠️ If Unity prompts you to upgrade project settings, stick with **2023.1** for stability  

### 4. Run the Game
- In Unity, open the **MainMenu** scene (found in the **Scenes/** folder)  
- Press the **Play** button in the Unity Editor  
- Use the controls listed below to move, fight, and explore  

### 5. Build an Executable
To create a standalone build:
- In Unity, go to **File → Build Settings**  
- Select your target platform (e.g. Windows)  
- Add the main scenes (MainMenu, Levels, etc.) to the build list  
- Click **Build and Run** to generate a `.exe`  
---
## 📌 Notes

- Developed as a **university project** to demonstrate FPS mechanics, roguelike systems, and collaborative workflows.  
- Not intended as a commercial release, but as a showcase of Unity development and teamwork.  
- Assets and tutorials credited below.  

---

## 🎨 Third‑Party Assets & References

### Weapons & Items
- [Oldschool AFPS Weapons](https://opengameart.org/content/oldschool-afps-weapons)  
- [PSX Ammo Boxes](https://doctor-sci3nce.itch.io/psx-ammo-boxes)  
- [Medical Stuff](https://opengameart.org/content/medical-stuff)  

### UI
- [Grenade icons](https://mtk.itch.io/grenades-16x16)  
- [2D Health & Ammo Pickups](https://fightswithbears.itch.io/2d-health-and-ammo-pickups)  
- [Wrench icon](https://www.flaticon.com/free-icon/wrench_4415248)  

### Sounds
- [Button clicks](https://opengameart.org/content/16-button-clicks)  
- [Door sounds](https://www.moddb.com/games/quake-space/videos/door-sounds)  
- [Melee sounds](https://opengameart.org/content/3-melee-sounds)  

### Tutorials Referenced
- [Sliding Doors](https://youtu.be/cPltQK5LlGE)  
- [Inventory System](https://youtu.be/OzvKBW4FvWg)  
- [Game Over Screen](https://youtu.be/K4uOjb5p3Io)  
- [Save/Load System](https://youtu.be/XOjd_qU2Ido)  
- [Enemy AI 1](https://youtu.be/UjkSFoLxesw)  
- [Enemy AI 2](https://youtu.be/rs7xUi9BqjE)  
- [Bullet Tracers](https://youtu.be/cI3E7_f74MA)  
