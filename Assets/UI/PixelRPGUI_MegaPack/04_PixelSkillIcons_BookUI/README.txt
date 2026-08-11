===============================================================
  Pixel Skill Icons & Book UI
  Unity Asset Package
===============================================================

Pixel art skill icons and a medieval book-style UI system
for RPG games.


---------------------------------------------------------------
  OVERVIEW
---------------------------------------------------------------

- 2,168+ carefully crafted pixel skill icons
- Book-style skill detail UI
- Skill Level Up & Grade Level Up popup prefabs
- Particle FX materials for UI effects
- 4 icon sizes: 32x32, 64x64, 102x102, 204x204
- Normal & Disabled icon states
- Demo scene included


---------------------------------------------------------------
  SUPPORTED UNITY VERSIONS
---------------------------------------------------------------

- Unity 2021.3 LTS or later
- Requires: TextMeshPro (included in Unity)
- Requires: Input System package (com.unity.inputsystem)


---------------------------------------------------------------
  FOLDER STRUCTURE
---------------------------------------------------------------

PixelSkillIcons_BookUI/
|
|-- Demo/                    Demo scene
|
|-- Font/                    Noto Serif fonts + license
|
|-- FX_Materials/            Particle effect materials
|
|-- Prefabs/
|   |-- SkillBook.prefab         Main skill book UI
|   |-- SkillItem.prefab         Individual skill icon item
|   |-- SkillPreview.prefab      Skill preview card UI
|   |-- SkillResult.prefab       Skill level up popup
|   |-- GradeResult.prefab       Grade level up popup
|   |-- TabGroupItem.prefab      Tab group item UI
|
|-- Preview/                 Preview images
|
|-- Resources/
|   |-- ScriptableObjects/
|       |-- 01_Warrior_Berserker/    Skill data assets (Warrior/Berserker)
|       |-- 02_Mage_Sorcerer/        Skill data assets (Mage/Sorcerer)
|       |-- 03_Archer_Assassin/      Skill data assets (Archer/Assassin)
|       |-- 04_Priest_Paladin/       Skill data assets (Priest/Paladin)
|
|-- Scripts/                 C# source scripts
|
|-- Sprites/
    |-- Bonus/               Grade icons (Iron~Master)
    |-- SkillIcon/
    |   |-- 01_Normal/       Active icons (32/64/102/204)
    |   |-- 02_Disabled/     Disabled icons (32/64/102/204)
    |
    |-- UI Elements/         Bookmarks, badges, book images


---------------------------------------------------------------
  SKILL ICON CLASSES
---------------------------------------------------------------

Each size folder contains 4 character classes:

  01_Warrior_Berserker   - Melee & physical skills
  02_Mage_Sorcerer       - Magic & elemental skills
  03_Archer_Assassin     - Ranged & stealth skills
  04_Priest_Paladin      - Holy & support skills


---------------------------------------------------------------
  GRADE ICONS (Bonus)
---------------------------------------------------------------

  Iron > Bronze > Silver > Gold > Platinum > Diamond > Master


---------------------------------------------------------------
  HOW TO USE
---------------------------------------------------------------

1. Import the package into your Unity project.

2. Open a demo scene to see the skill book UI in action.
   - Demo/Demo1_Skill_Main.unity  - Main skill book UI demo
   - Demo/Demo2_Skill_View.unity  - Skill view UI demo

3. Skill Book UI:
   - Drag "Prefabs/SkillBook.prefab" into your Canvas.
   - Assign skill icons from Sprites/SkillIcon/ to the
     icon slots.

4. Skill Icons standalone:
   - Navigate to Sprites/SkillIcon/01_Normal/ and choose
     the desired size folder (32, 64, 102, or 204).
   - Select the character class subfolder.
   - Assign the icon sprites to your UI Image components.

5. Disabled/locked states:
   - Use icons from Sprites/SkillIcon/02_Disabled/ with
     the matching size and class.

6. Level Up Popups:
   - Use "Prefabs/SkillResult.prefab" for skill upgrades.
   - Use "Prefabs/GradeResult.prefab" for grade promotions.


---------------------------------------------------------------
  THIRD-PARTY LICENSES
---------------------------------------------------------------

Font: Noto Serif (Regular, Medium, Bold)
  - Copyright 2022 The Noto Project Authors
  - License: SIL Open Font License v1.1
  - See Font/LICENSE_NotoSerif.txt for full license text


---------------------------------------------------------------
  SUPPORT
---------------------------------------------------------------

For any questions or issues, please contact us at:
lattemongling@gmail.com

We appreciate your review and rating on the Asset Store!


===============================================================
  (c) All Rights Reserved
===============================================================
