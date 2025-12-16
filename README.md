# **Yeet a Mole 🐹🕳️🥊**

**Cindy Wei, Zhexu Luo, Joshua Zhang**  

**Development Period:** 2025.11.12 – 2025.12.11  
<br/>

<img src="https://github.com/user-attachments/assets/a4430dff-96c7-4934-b8bb-59a0ab6491c8" width="720" />

---

## 📚 Table of Contents
1. [Overview](#-overview)
2. [Gameplay Preview](#-gameplay-preview)
3. [Full Gameplay Video](#-full-gameplay-video)
4. [Key Features](#-key-features)
5. [Enemies & Boss](#-enemies--boss)
6. [Tutorial Rooms](#-tutorial-rooms)
7. [Gameplay Summary](#-gameplay-summary)
8. [How to Play](#-how-to-play)
9. [Setup & Configuration](#-setup--configuration)
10. [Technical Issues Encountered](#-technical-issues-encountered)
11. [Assets & Plugins Used](#-assets--plugins-used)
---

## **📌 Overview**

**Yeet-a-Mole** is a fast-paced VR wave-survival action game built in **Unity for Meta Quest 3**. Players defend their farm from invading moles and monsters by turning moles into weapons through a physics-driven combat loop:

### **Whack → Bounce → Yeet**

Using three tools—**Bat**, **Shovel**, and **Racket**—players convert different mole types into mole-balls, bounce them to charge, and “Yeet” them back at incoming enemies.

The game features **three enemy types—Walkers, Spitters, and Tanks—followed by a multi-phase Final Boss** with teleportation, arena-control attacks, and minion summons.


https://github.com/user-attachments/assets/ef12f287-4ea0-4023-ae7e-74f1f5412928

---

## 🕹️ Full Gameplay Video

<a href="https://youtu.be/g-0qPlhva8I">
  <img src="https://img.youtube.com/vi/g-0qPlhva8I/0.jpg" width="720" />
</a>

---

# **🎮 Key Features**

## **Three-Tool Combat System**
- **Bat** – Whack regular moles into mole-balls  
- **Shovel** – Safely dig up explosive moles  
- **Racket** – Yeet charged mole-balls at enemies  

## **VR Physicality**
- Physics-driven combat using real-world motion  
- Full arm-swing interactions  
- Natural throwing, batting, and digging  

---

# **👾 Enemies & Boss**

### **Enemy Roster**

| Walker | Spitter |
|:--:|:--:|
| <a href="https://github.com/user-attachments/assets/ae0c6431-dc89-4ee6-aeb7-6f1f0da0e407"><img src="https://github.com/user-attachments/assets/d6bcb459-f38b-4ad6-83be-a2bbbcf3e27a" width="340"/></a> | <a href="https://github.com/user-attachments/assets/d859c2fd-b176-4496-a03b-b758280b33f1"><img src="https://github.com/user-attachments/assets/6f5d4596-d0e9-4b8a-8203-90e9e652f30e" width="340"/></a> |
| Moderate speed<br/>Weak to knockback | Ranged enemy<br/>Parry-able projectiles |

| Tank | Final Boss |
|:--:|:--:|
| <a href="https://github.com/user-attachments/assets/a1c9db53-1c89-4898-b6cf-2d3612ee07be"><img src="https://github.com/user-attachments/assets/be6f3cb5-1d6c-4a27-8420-0e7bee81189e" width="340"/></a> | <a href="https://github.com/user-attachments/assets/ef12f287-4ea0-4023-ae7e-74f1f5412928"><img src="https://github.com/user-attachments/assets/e0588d58-457e-46fe-947b-979fefe69827" width="340"/></a> |
| Armored<br/>Explosive damage only | Multi-phase boss<br/>Arena-control mechanics |

**Final Boss Abilities** 
- Non-redirectable boulder throws  
- Explosive mole ring summon  
- Minion waves (Walkers, Spitters, Tanks)  
 


---


# **🧪 Tutorial Rooms**

Before entering the main level, players complete **four guided tutorial rooms**:

**ROOM1: Bat & Regular Moles** – Whack brown moles into normal ammo  

https://github.com/user-attachments/assets/dd807269-f6f7-434b-bf29-8d43b852f819

**ROOM2: Explosive Moles & Shovel** – Learn safe handling of red moles  

https://github.com/user-attachments/assets/b7b15d05-a63a-4f6a-9556-d567a8662e42

**ROOM3: Mole-Balls & Racket** – Bounce and Yeet charged balls  

https://github.com/user-attachments/assets/c238f258-7371-4515-8b3b-ee5d11a3d4af

**ROOM4: Projectile Parrying** – Reflect enemy projectiles  

https://github.com/user-attachments/assets/f2175bec-a307-41bc-8517-82ef9ff4dca2

Each room must be completed before progressing.

---

# **📝 Gameplay Summary**

- **Whack** → normal mole-ball  
- **Dig** → explosive mole-ball  
- **Bounce** → charged (yellow)  
- **Yeet** → damage enemies  

Enemies crossing the fence reduce HP.  
HP reaches **0** → Game Over.

---

# **📖 How to Play**

## **Objective**
Survive enemy waves and defeat the final boss by mastering the Whack → Bounce → Yeet loop.

## **Controls (Meta Quest 3)**

**Left Hand**
- Trigger (Hold): Pick up mole-ball  
- Trigger (Release): Throw  
- Button X: Next tutorial room  

**Right Hand**
- Button A: Cycle tools  
- Trigger: Confirm usage  

---

# **⚙️ Setup & Configuration**

- **Platform:** Meta Quest 3  
- **Installation:** Sideload via MQDH or SideQuest  
- **Unity Version:** Unity 6 (6000.0.x)  
- **XR:** OpenXR enabled  

---

# **🛠️ Technical Issues Encountered**

**High-Velocity Collision (Tunneling)**  
Solved using continuous dynamic collision checks.

**Difficulty Balancing**  
Enemy pacing and explosive mole frequency adjusted.

**Physics Predictability**  
Edge cases remain with multi-collision frames.

---

# **📦 Assets & Plugins Used**

- Unity XR Interaction Toolkit  
- ProBuilder  
- Fantasy Skybox FREE  
- Poly Haven / AmbientCG  
- Kenney Assets  
- TextMeshPro  

**Models & Audio**
- Practice Dummy  
- Stylized Shovel  
- Baseball Bat  
- Tennis Racket  
- Mole Models  
- Sound Effects Pack  


