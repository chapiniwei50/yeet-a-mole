
# **Yeet-a-Mole 🐹🕳️🥊**

**Cindy Wei, Zhexu Luo, Joshua Zhang**
<br>
**Development Period:** 2025.11.12 – 2025.12.11
<br/>

<img src="https://github.com/user-attachments/assets/a4430dff-96c7-4934-b8bb-59a0ab6491c8" width="720" />

---

## **📌 Overview**

**Yeet-a-Mole** is a fast-paced VR wave-survival action game built in **Unity for Meta Quest 3**. Players defend their farm from invading moles and monsters by turning moles into weapons through a physics-driven combat loop:

### **Whack → Bounce → Yeet**

Using three tools—**Bat**, **Shovel**, and **Racket**—players convert different mole types into mole-balls, bounce them to charge, and “Yeet” them back at incoming enemies.

The game features **three enemy types—Walkers, Spitters, and Tanks—followed by a multi-phase Final Boss**. Walkers apply steady pressure, Spitters attack from range with parry-able projectiles, and Tanks are armored enemies that require explosive damage. The final encounter escalates with teleportation, arena-control attacks, explosive mole rings, and summoned minions.
![mole_spawning-ezgif com-optimize](https://github.com/user-attachments/assets/c89db522-e82c-4ce3-abab-bb0ae9515c88)


---

## 🕹️ Full Gameplay Video

<a href="https://youtu.be/g-0qPlhva8I">
  <img src="https://img.youtube.com/vi/g-0qPlhva8I/0.jpg" width="720" />
</a>

---

# **🎮 Key Features**

## **Three-Tool Combat System**

* **Bat** – Whack regular moles into mole-balls
* **Shovel** – Safely dig up explosive moles
* **Racket** – Yeet charged mole-balls at enemies

---

## **VR Physicality**

* Physics-driven combat using real-world motion
* Full arm-swing interactions
* Natural throwing, batting, and digging
* Mole-balls respond to swing speed, impact angle, and timing

---

## **Enemies & Boss**

### **Walker**

![walker-ezgif com-optimize](https://github.com/user-attachments/assets/ee289298-8c15-41f2-b223-ad2ae513c697)

* Moderate speed
* Weak to knockback

---

### **Spitter**

![spitter-ezgif com-optimize](https://github.com/user-attachments/assets/8b2bd8e0-fe41-409c-b815-1eaeaed2d403)

* Slow-moving ranged enemy
* Fires **parry-able projectiles**

---

### **Tank**

![tank-ezgif com-optimize](https://github.com/user-attachments/assets/a3992bac-519a-48f4-82ec-e76f7017e63e)

* Armored enemy with higher HP
* Vulnerable to **explosive mole-balls only**

---

### **Final Boss (Multi-Phase)**

* Teleports around the arena
* Throws **non-redirectable boulders**
* Summons a **ring of explosive moles** around the player
* Spawns mixed enemy waves
* Temporarily accelerates enemy actions

---

# **🧪 Tutorial Rooms**

Before entering the main level, players complete **four guided tutorial rooms** that introduce mechanics step-by-step in a controlled environment.

1. **Bat & Regular Moles** – Whack brown moles to create normal mole-balls
2. **Explosive Moles & Shovel** – Safely dig red moles to create explosive ammo
3. **Mole-Balls & Racket** – Bounce and Yeet mole-balls at target dummies
4. **Projectile Parrying** – Reflect enemy projectiles with precise racket timing

Players must complete each room before progressing.

---

# **📝 Gameplay Summary**

* **Whack** brown moles with the **Bat** → normal mole-ball
* **Dig** red (explosive) moles with the **Shovel** → explosive mole-ball
* **Bounce** mole-ball once → charged (yellow)
* **Yeet** charged mole-ball with the **Racket**

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
* **Joystick Pull:** Pull mole-ball closer
* **Trigger (Release):** Throw mole-ball
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
* **Bounce:** Bounce mole-balls once to charge
* **Yeet:** Hit glowing yellow balls with the Racket

**Win / Loss Conditions**

* Survive all waves → **Win**
* Enemies crossing the fence → lose HP
* HP reaches 0 → **Game Over**

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

---

