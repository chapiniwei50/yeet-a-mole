# **Yeet-a-Mole 🐹🕳️🥊**
Cindy Wei, Zhexu Luo, Joshua Zhang
<br>
Development Period: 2025.11.12 – 2025.12.11
</br>
![yeet-a-mole-cover-ezgif com-optimize](https://github.com/user-attachments/assets/a4430dff-96c7-4934-b8bb-59a0ab6491c8)

---

## **📌 Overview**

**Yeet-a-Mole** is a fast-paced VR wave-survival action game built in **Unity for Meta Quest 3**. Players defend their farm from invading moles and monsters by turning moles into weapons through a physics-driven combat loop:

### **Whack → Bounce → Yeet**

Using three tools—**Bat**, **Shovel**, and **Racket**—players convert different mole types into mole-balls, bounce them to charge, and “Yeet” them back at incoming enemies.

The game features **three enemy types—Walkers, Spitters, and Tanks—followed by a multi-phase Final Boss**. Walkers apply steady pressure, Spitters attack from range with parry-able projectiles, and Tanks are armored enemies that require explosive damage. The final encounter escalates with teleportation, arena-control attacks, explosive mole rings, and summoned minions.

https://github.com/user-attachments/assets/ef12f287-4ea0-4023-ae7e-74f1f5412928

---

## 🕹️ Full Gameplay Video

[![Yeet-a-Mole Gameplay](https://img.youtube.com/vi/g-0qPlhva8I/0.jpg)](https://youtu.be/g-0qPlhva8I)

---

# **🎮 Key Features**

### **Three-Tool Combat System**

* **Bat** – Whack regular moles into mole-balls
* **Shovel** – Safely dig up explosive moles
* **Racket** – Yeet charged mole-balls at enemies

---

### **VR Physicality**

* All combat is driven by physical motion and real-time collisions
* Full arm-swing combat
* Natural throwing, batting, and digging motions
* Mole-balls respond to swing speed, impact angle, and timing

---

### **Enemies & Boss**

* **Walker:** Advances toward the defense line at moderate speed; weak to knockback
  

https://github.com/user-attachments/assets/ae0c6431-dc89-4ee6-aeb7-6f1f0da0e407


* **Spitter:** Slow-moving ranged enemy that fires **parry-able projectiles**


https://github.com/user-attachments/assets/d859c2fd-b176-4496-a03b-b758280b33f1


* **Tank:** Armored enemy with higher HP; resists knockback and is vulnerable to **explosive mole-balls only**


https://github.com/user-attachments/assets/a1c9db53-1c89-4898-b6cf-2d3612ee07be


**Final Boss (Multi-Phase):**


* Throws **non-redirectable boulders**
* Summons a **ring of explosive moles** around the player
* Spawns minion waves (Walkers, Spitters, Tanks)
* Temporarily accelerates enemy actions

---

# **🧪 Tutorial Rooms**

Before entering the main level, players complete **four guided tutorial rooms** designed to gradually introduce core mechanics in a controlled environment. Each room focuses on a single concept to build confidence and muscle memory in VR.

* **Room 1 – Bat & Regular Moles**
  Learn to identify and **whack regular moles** using the Bat to create normal mole-balls.

* **Room 2 – Explosive Moles & Shovel**
  Learn the danger of **red (explosive) moles** and practice using the Shovel instead of the Bat to safely convert them into explosive ammo.

* **Room 3 – Mole-Balls & Racket**
  Practice **bouncing mole-balls** to charge them and **Yeeting** them with the Racket at a target dummy.

* **Room 4 – Enemy Projectiles & Parrying**
  Learn how to **time racket swings** to parry incoming projectiles and turn enemy attacks back against them.

Players must complete each room’s objective before progressing
---

# **📝 Gameplay Summary**

* **Whack** brown moles with the **Bat** → normal mole-ball
* **Dig** red (explosive) moles with the **Shovel** → explosive mole-ball
* **Bounce** mole-ball on the ground → turns yellow when bounced(charged)
* **Yeet** charged mole-ball with the **Racket** at enemies

Enemies that cross the fence reduce player HP.
When HP reaches **0**, the game ends.

---

# **📖 How to Play**

## **Objective**

Defend your farm from the mole invasion. Survive enemy waves and defeat the final boss by correctly matching tools to mole types and mastering the Whack → Bounce → Yeet loop.

---

## **Controls (Meta Quest 3)**

### **Movement & View**

* All movement and camera control are based on the player’s **physical movement** in the real world

---

### **Left Hand – Mole-Ball Interaction**

* **Trigger (Hold):** Pick up and hold a mole-ball
* **Joystick Pull (while holding trigger):** Pull mole-ball closer
* **Trigger (Release):** Throw mole-ball

  > *You must be holding the ball before releasing to throw*
* **Button X:** Proceed to the next tutorial room

---

### **Right Hand – Tool Switching & Use**

* **Button A:** Cycle tools
  **Bat → Shovel → Racket → Bat**
* **Trigger:** Confirm tool usage 

---

## **Gameplay Rules**

* **Bat:** Hit brown moles only
* **Shovel:** Dig red (explosive) moles
  ⚠️ *Hitting red moles with the Bat damages the player*
* **Bounce:** Bounce mole-balls once to charge them
* **Yeet:** Hit glowing yellow balls with the Racket
* **Win/Loss:**

  * Survive through all waves of enemies → Win Game
  * Enemies crossing the fence → lose HP
  * HP reaches 0 → Game Over

---

# **⚙️ Setup & Configuration**

* **Platform:** Meta Quest 3
* **Installation:**

  * Sideload `Yeet-a-Mole.apk` using **Meta Quest Developer Hub** or **SideQuest**
* **Unity Version:** Unity 6 (6000.0.x)
* **Development Setup:**

  1. Open the project in Unity
  2. Set **Build Target: Android**
  3. Enable **OpenXR** under
     `Project Settings → XR Plug-in Management`

---


# **🛠️ Technical Issues Encountered**

### **High-Velocity Collision (Tunneling)**

Fast VR swings caused missed hits due to colliders passing through objects between physics updates.
**Solution:** Switched from discrete collision detection to **continuous dynamic sweep-volume collision checks**.

---

### **Difficulty Balancing**

Early builds overwhelmed players with fast enemies and frequent explosive moles.
**Fix:** Reduced explosive mole frequency and slowed enemy pacing to emphasize skill and recognition.

---

### **Physics Predictability**

Racket impacts are sensitive to collision angles. An unresolved edge case remains where multiple collisions in a single frame can launch the ball unpredictably. Attempts included cooldown timers and temporary collision ignores.

---

# **📦 Assets & Plugins Used**

* **Unity XR Interaction Toolkit (XRI)** – Core VR interaction framework
* **ProBuilder** – Arena and fence block-outs
* **Fantasy Skybox FREE** – Stylized skybox
* **Stylized Dirt/Ground Textures** – Poly Haven / AmbientCG
* **Kenney Assets (Nature/Town)** – Background environment
* **TextMeshPro** – UI text rendering

**Models & Audio**

* Practice Dummy (Low Poly)
* Stylized Shovel
* Baseball Bat
* Tennis Racket
* Mole Models
* Sound Effects Pack


* Write a **postmortem / what we learned** section
* Or format it **exactly** to CIS 5680 final submission standards

