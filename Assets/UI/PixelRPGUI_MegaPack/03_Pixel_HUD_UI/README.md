# Pixel UI HUD: Fantasy RPG Kit

A handcrafted pixel art HUD/UI asset pack designed for fantasy RPG games.
Drop it into your Unity project and build a beautiful retro-style game interface in minutes.

---

## What's Included

| Category | Count | Description |
|----------|-------|-------------|
| **Sprites** | 82 | Pixel art UI elements (PNG, lossless) |
| **Prefabs** | 2 | Ready-to-use HUD & Component prefabs |
| **Fonts** | 3 weights | Noto Serif (Regular / Medium / Bold) with SDF assets |
| **Animation** | 1 | Open animation with Animator Controller |
| **Demo Scene** | 1 | Fully assembled HUD preview scene |
| **Preview Images** | 2 | High-resolution reference images |

---

## Features

### Status & Progress Bars
- **2 progress bar styles** — each with 9 color variations (Blue, BlueGreen, Green, Orange, Pink, Purple, Red, SkyBlue, Yellow)
- **Status bars** — HP bar + 9 color fills for MP, stamina, XP, and more
- **Bar icons** — HP heart icon + 9 potion icons matching each color

### Icons (18 icons)
- **Consumable** — Apple, Carrot, Pumpkin
- **General** — Compass, Hourglass, Map
- **System** — Alarm, Attack, Chat, Defense, Download, Gem, Gold, Lock, Menu
- **Tool** — Logging Axe, Repair Hammer
- **Material** — Branches

### UI Components
- **Panels** — Currency, SlotPane (2 variants), Text, Thumbnail, Title
- **Slots** — Normal, Selected, Dim (regular & small sizes)
- **Buttons** — System button
- **Frames** — Map frame, Map demo, Map view
- **Indicators** — Heart, Heart Dim, Red Dot, Tag
- **Background** — Tiling background image
- **Thumbnail** — Character thumbnail frame

### Prefabs
- **HUD.prefab** — Complete HUD layout with health bars, mini-map, inventory slots, currency display, and quick-action buttons. Drag & drop ready.
- **Component.prefab** — Individual UI components organized for custom layout assembly.

### Fonts
- Noto Serif — 3 weights (Regular, Medium, Bold)
- TTF + TextMesh Pro SDF assets included
- Licensed under SIL Open Font License v1.1 (see `Font/LICENSE_NotoSerif.txt`)

---

## Quick Start

1. Import the package into your Unity project.
2. Open `Demo/Demo.unity` to see the full HUD in action.
3. Drag `Prefabs/HUD.prefab` into your scene Canvas to use the complete HUD layout.
4. Or use `Prefabs/Component.prefab` to pick individual UI pieces and build your own layout.
5. Customize colors by swapping bar fill sprites — 9 colors available for each bar style.

---

## Folder Structure

```
Assets/IndigoLay/Pixel_HUD_UI/
├── Animation/              Open animation & Animator Controller
├── Demo/                   Demo scene
├── Font/                   TTF + SDF font assets & license
├── Prefabs/                HUD.prefab, Component.prefab
├── Preview/                Reference images
└── Sprites/
    ├── Icons/
    │   ├── Consumable/     Food item icons
    │   ├── General/        Navigation & utility icons
    │   ├── Material/       Crafting material icons
    │   ├── System/         UI system icons
    │   └── Tool/           Tool icons
    └── UI Elements/
        ├── Background/     Tiling background
        ├── Bars/           Progress, Status, BarIcon
        ├── Buttons/        Button sprites
        ├── Frame/          Map frames
        ├── Indicator/      Notification & status indicators
        ├── Panel/          Panel backgrounds
        ├── Slot/           Inventory slot sprites
        └── Thumbnail/      Thumbnail frame
```

---

## Technical Details

- **Unity version**: 2021.3 LTS or newer
- **Render pipeline**: Compatible with Built-in, URP, and HDRP
- **Sprite format**: PNG (lossless)
- **TextMesh Pro**: SDF font assets included for crisp text at any resolution
- **Dependencies**: None — fully self-contained

---

## Third-Party Notices

This package includes **Noto Serif** font by Google Fonts (Noto Project Authors).
Licensed under the **SIL Open Font License, Version 1.1**.
Full license text is located at `Font/LICENSE_NotoSerif.txt`.

---

## Support

If you have any questions, issues, or feature requests, please reach out:

**Email**: lattemongling@gmail.com

Thank you for your purchase!
