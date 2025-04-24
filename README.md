# 🦓 Zoo Tycoon Idle (Unity 2022.3.37f1)

A low-poly 3D idle tycoon game prototype built in Unity. The player manages a zoo-style business empire, upgrading various facilities to increase revenue over time. The project showcases core gameplay systems, custom UI, saving/loading, and scene management using native Unity tools only.

![Gameplay Screenshot](screenshot.png) <!-- Add your screenshot here -->

---

## 🎮 Features

### 🎯 Core Gameplay
- **Idle income**: Each zoo facility generates income per minute.
- **Upgradable buildings**: Increase facility levels to boost revenue and visually upgrade the scene.
- **Progression system**: Unlock new levels and improvements by investing your profits.

### 🧠 Systems
- **Building & Upgrade System**: Add and level up businesses with visual changes per level.
- **Player Balance System**: Tracks total money and calculates income per minute.
- **Custom Save System**: JSON-based autosave (no PlayerPrefs, no third-party assets).
- **Inventory & Crafting** *(Expandable)*: Basic structure to support future extensions.
- **Scene Management**: Uses an `Init` scene for deserialization and bootstrapping the `Gameplay` scene via Unity Addressables.
- **UI/UX**:
  - Main Menu
  - Settings
  - In-game HUD with balance & income display

---

## 📦 Architecture

- **Scenes**:
  - `Init` – Handles data deserialization and loading the main gameplay scene (Addressable).
  - `Gameplay` – Main interactive environment.
- **Addressables**: Used for loading the gameplay scene, allowing remote deployment later.
- **Serialization**: Game state saved in custom JSON files with autosave support.
- **Universal Render Pipeline** (URP): For optimized cross-platform rendering.
- **No third-party dependencies**: 100% native Unity API usage.

---

## 🚀 Getting Started

1. Clone the repository.
2. Open with **Unity 2022.3.37f1**.
3. Build Addressables: `Window > Asset Management > Addressables > Build > Build Player Content`.
4. Play from the `Init` scene.

---

## 🎨 Visual Style

This project uses a **low poly** art style for clarity and performance. All models and UI elements are consistent in design and scale, with a clean, stylized aesthetic.

---


---

## 🛠️ Tools & Technologies

- Unity 2022.3.37f1
- Universal Render Pipeline (URP)
- Unity Addressables
- Custom JSON I/O for saving
- Native Unity UI (no DOTween, Zenject, etc.)

---

## ✨ Possible Extensions

- Dialog or negotiation system
- Multi-currency economy
- AI visitor behaviors
- Marketplace or trade mechanics

---

## 📃 License

This project is for demonstration and testing purposes. You are free to explore and build upon it.
