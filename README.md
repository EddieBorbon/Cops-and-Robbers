# 🚓 Cops and Robbers  
### 🤖 *Autonomous Agent Simulation*

 ![Cover](covercoprobbers.jpg)
---

## 📖 Description

*Cops and Robbers* is a Unity-based game that demonstrates the use of **autonomous agents** powered by Unity's **NavMesh system**. The game involves two AI-controlled roles: **Cops** and **Robbers**, dynamically interacting within a defined environment. The bots showcase intelligent behaviors such as hiding, pursuing, and evading.

---

## ✨ Features

### 🧠 Autonomous Agents
- **Cops**: Pursue and capture Robbers.
- **Robbers**: Hide and evade Cops dynamically based on visibility and proximity.

### 🎲 Dynamic Spawning
- Agents spawn dynamically at random locations within a defined area.
- Configurable spawn counts and areas for level customization.

### 🎭 Role Assignment
- Each bot is assigned a role (**Cop** or **Robber**) that dictates its behavior and interactions.

### 🌍 Randomized Placement
- Agents are placed randomly within a customizable area for varied gameplay.

---

## 🛠️ Setup Instructions

1. **Unity Version**: Use Unity 2020.3 or later (with NavMesh system enabled).
2. **Assets Required**:
   - Prefabs for Cops and Robbers (assigned in the `BotManager` script).
   - Ensure prefabs have proper 3D models and NavMesh compatibility.
3. **Configure the Scene**:
   - Attach the `BotManager` script to an empty GameObject.
   - Set the **spawn area center** and **size** via the Inspector.
   - Assign the **Cop** and **Robber** prefabs in the script's public fields.

---

## 📝 Script Overview

### `BotManager.cs`

#### 🎯 Responsibilities:
1. **Spawn Autonomous Agents**:
   - `SpawnBots(int robbersPerLevel, int copsPerLevel)`: Spawns agents dynamically at random locations.
2. **Role Assignment**:
   - Automatically assigns the `Bot` script and sets the role (Cop or Robber).
3. **Randomized Spawn Locations**:
   - Agents are placed randomly within a customizable area using `GetRandomPositionOnPlane`.

#### ⚙️ Key Methods:
- **`SpawnBots`**:
  - Spawns the specified number of Cops and Robbers per level.
- **`AssignBotScript`**:
  - Attaches the `Bot` script and ensures each bot is equipped with a `NavMeshAgent` for navigation.
- **`GetRandomPositionOnPlane`**:
  - Calculates a random position within the defined spawn area.

---

### `Bot.cs`

#### 🎯 Responsibilities:
Defines the core behavior of **Cops** and **Robbers** based on their roles. Both agents use **NavMeshAgent** for movement and perform role-specific tasks like pursuing, evading, and hiding.

#### ⚙️ Key Methods and Behaviors:
- **`RobberBehaviour()`**: Executes behaviors for Robbers:
  - **Hide**: Seeks a hiding spot when seen by the player.
  - **Evade**: Moves away from the player if in proximity.
  - **Wander**: Moves randomly if there’s no immediate threat.
- **`CopBehaviour()`**: Executes behaviors for Cops:
  - **Pursue**: Follows the player when visible.
  - **Seek**: Moves to the player's last known position when nearby but not visible.
  - **Wander**: Patrols randomly if the player is not detected.
- **`CanSeeTarget()`**: Detects if the bot has a clear line of sight to the player using raycasting.
- **`TargetCanSeeMe()`**: Determines if the player can see the bot, based on the player's field of view.
- **`Wander()`**: Implements random movement within a specific range to simulate patrol behavior.

#### ✨ Advanced Features:
- **Clever Hiding**:
  - Robbers find the best hiding spots, taking into account obstacles and the player's position.
- **Dynamic Decision-Making**:
  - Role-based behaviors adjust based on proximity, visibility, and cooldowns.

---

## 🔧 How to Customize

### Adding New Roles
1. Extend the `Bot` script to include new roles (e.g., "Guard," "Civilian").
2. Update the `BotManager` to handle additional role-specific logic.

### Enhancing AI Behavior
- Customize the `Bot` script to define unique behaviors for each role.
- Use Unity's **NavMesh Obstacles** to add complexity to pathfinding.

### Visual and Gameplay Improvements
- Add animations to Cops and Robbers for immersive interactions.
- Introduce obstacles, power-ups, or level-specific features to enhance gameplay.

---

## 🎮 Example Usage

1. Attach the `BotManager` to a GameObject in your Unity scene.
2. Assign prefabs for Cops and Robbers in the Inspector.
3. Configure the **spawn area** and set the desired number of agents.
4. Call `SpawnBots()` to initialize the level and watch the agents interact dynamically!

---

## 🙌 Credits

Developed using Unity's **NavMesh System** for autonomous agent simulation.  
A fun and interactive way to explore AI-driven gameplay mechanics!  
🎉 Happy Coding!
