# 📁 Estructura de Carpetas — Metroidvania Pixel Art

Generado por **MetroidvaniaFolderSetup.cs**

---

## Convenciones de naming

| Regla | Ejemplo |
|-------|---------|
| PascalCase para carpetas y scripts | `PlayerController.cs` |
| snake_case para assets de arte | `player_idle_01.png` |
| Prefijo `PFB_` para prefabs | `PFB_Player`, `PFB_EnemySlime` |
| Prefijo `SO_` para ScriptableObjects | `SO_SwordData`, `SO_EnemyCrawler` |
| Prefijo `AC_` para AnimatorControllers | `AC_Player`, `AC_Boss` |

---

## Módulos

| Carpeta | Contenido |
|---------|-----------|
| `Art/` | Sprites, animaciones, tilemaps, VFX, shaders, materiales |
| `Audio/` | Música, SFX por actor, AudioMixers |
| `Scripts/` | Código organizado por dominio |
| `Data/` | Instancias de ScriptableObjects |
| `Prefabs/` | Prefabs listos para escena |
| `Scenes/` | Escenas del mundo, menús y utilidad |
| `Settings/` | Input Actions, Rendering, Physics |
| `Plugins/` | Assets de terceros (no modificar) |
| `Editor/` | Herramientas del editor (no van al build) |
| `Documentation/` | Este README y el CHANGELOG |

---

## Scripts/ en detalle

Organizado por **dominio**, no por tipo de archivo.

- `Core/` — GameManager, SaveSystem, EventBus
- `Player/` — movimiento, combate, habilidades, máquina de estados
- `Enemies/Types/` — una clase por cada tipo de enemigo
- `Boss/` — fases y transiciones del jefe
- `Environment/` — plataformas, trampas, interactuables
- `Items/` — pickups, equipamiento
- `UI/` — HUD, menús, mapa, diálogo
- `Camera/` — lógica de cámara y Cinemachine
- `Audio/` — AudioManager
- `Utilities/` — helpers, extensiones, patrones (Singleton, ObjectPool)
- `ScriptableObjects/` — clases base de SO (las instancias van en Data/)

---

## Flujo rápido por tarea

| Tarea | Dónde tocar |
|-------|-------------|
| Nuevo enemigo | `Scripts/Enemies/Types/`, `Data/Enemies/`, `Prefabs/Enemies/` |
| Nueva habilidad | `Scripts/Player/`, `Data/Player/`, `Art/Sprites/Player/` |
| Nuevo ítem | `Scripts/Items/`, `Data/Items/`, `Art/Sprites/Items/` |
| Nueva zona | `Scenes/World/`, `Art/Sprites/Environment/`, `Art/Tilemaps/` |
| Nuevo jefe | `Scripts/Boss/`, `Scenes/Boss/`, `Art/Sprites/Boss/` |
