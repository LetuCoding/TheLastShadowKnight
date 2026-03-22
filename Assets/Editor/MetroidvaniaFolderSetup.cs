// ============================================================
//  MetroidvaniaFolderSetup.cs
//  Coloca este archivo en Assets/Editor/ de tu proyecto.
//  Uso: Tools → Metroidvania → Create Project Structure
// ============================================================

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MetroidvaniaFolderSetup : EditorWindow
{
    private static readonly Color COLOR_BG      = new Color(0.13f, 0.13f, 0.16f);
    private static readonly Color COLOR_ACCENT  = new Color(0.20f, 0.80f, 0.60f);
    private static readonly Color COLOR_WARN    = new Color(1.00f, 0.75f, 0.20f);
    private static readonly Color COLOR_HEADER  = new Color(0.10f, 0.10f, 0.13f);
    private static readonly Color COLOR_BTN     = new Color(0.18f, 0.72f, 0.52f);
    private static readonly Color COLOR_BTN_HOV = new Color(0.22f, 0.90f, 0.65f);

    private Vector2 _scroll;
    private bool    _alreadyExists;
    private string  _statusMessage = "";
    private bool    _showPreview   = true;

    // ── Estructura simplificada para proyecto en solitario ───────────
    private static readonly string[] FOLDERS = new string[]
    {
        // ── ART ──────────────────────────────────────────────────────
        "Art",
        "Art/Sprites",
        "Art/Sprites/Player",                 // todos los sprites del jugador aquí
        "Art/Sprites/Enemies",                // un sprite sheet por enemigo
        "Art/Sprites/Boss",
        "Art/Sprites/Environment",            // tilesets y props juntos
        "Art/Sprites/Items",
        "Art/Sprites/UI",
        "Art/Animations",
        "Art/Animations/Player",
        "Art/Animations/Enemies",
        "Art/Animations/Environment",
        "Art/Tilemaps",                       // RuleTiles y Palettes
        "Art/VFX",                            // Particle Systems
        "Art/Shaders",
        "Art/Materials",
        "Art/Fonts",

        // ── AUDIO ────────────────────────────────────────────────────
        "Audio",
        "Audio/Music",
        "Audio/SFX",
        "Audio/SFX/Player",
        "Audio/SFX/Enemies",
        "Audio/SFX/Environment",
        "Audio/SFX/UI",
        "Audio/AudioMixers",

        // ── SCRIPTS ──────────────────────────────────────────────────
        "Scripts",
        "Scripts/Core",                       // GameManager, SaveSystem, EventBus
        "Scripts/Player",                     // movimiento, combate, habilidades
        "Scripts/Player/StateMachine",
        "Scripts/Enemies",                    // AI y comportamiento base
        "Scripts/Enemies/Types",              // una clase por tipo de enemigo
        "Scripts/Boss",
        "Scripts/Environment",               // plataformas, trampas, interactuables
        "Scripts/Items",
        "Scripts/UI",
        "Scripts/Camera",
        "Scripts/Audio",
        "Scripts/Utilities",                  // helpers, extensiones, patrones
        "Scripts/ScriptableObjects",          // clases base de SOs

        // ── DATA ─────────────────────────────────────────────────────
        "Data",                               // instancias de ScriptableObjects
        "Data/Player",
        "Data/Enemies",
        "Data/Items",

        // ── PREFABS ──────────────────────────────────────────────────
        "Prefabs",
        "Prefabs/Player",
        "Prefabs/Enemies",
        "Prefabs/Boss",
        "Prefabs/Environment",
        "Prefabs/Items",
        "Prefabs/VFX",
        "Prefabs/UI",

        // ── SCENES ───────────────────────────────────────────────────
        "Scenes",
        "Scenes/Menus",
        "Scenes/World",                       // escenas del mapa principal
        "Scenes/Boss",
        "Scenes/Utilities",                   // Bootstrap, Loading

        // ── SETTINGS ─────────────────────────────────────────────────
        "Settings",                           // Input Actions, Rendering, Physics

        // ── PLUGINS ──────────────────────────────────────────────────
        "Plugins",                            // assets de terceros sin tocar

        // ── EDITOR ───────────────────────────────────────────────────
        "Editor",                             // este script y herramientas propias

        // ── DOCUMENTATION ────────────────────────────────────────────
        "Documentation",
    };

    private static readonly Dictionary<string, string> DESCRIPTIONS =
        new Dictionary<string, string>
    {
        { "Art",           "Sprites, animaciones, tilemaps, VFX, shaders y materiales." },
        { "Audio",         "Música, SFX por actor y AudioMixers." },
        { "Scripts",       "Código por dominio: Core, Player, Enemies, UI…" },
        { "Data",          "Instancias de ScriptableObjects (stats, ítems, enemigos)." },
        { "Prefabs",       "Prefabs listos para escena, organizados igual que Scripts/." },
        { "Scenes",        "Escenas del mundo, menús y escenas de utilidad." },
        { "Settings",      "Input Actions, Physics Materials, Render Pipeline." },
        { "Plugins",       "Assets de terceros. Nunca modificar directamente." },
        { "Editor",        "Herramientas y scripts solo para el Editor (no van al build)." },
        { "Documentation", "README, CHANGELOG y notas de diseño." },
    };

    // ─────────────────────────────────────────────────────────────────
    [MenuItem("Tools/Metroidvania/Create Project Structure")]
    public static void ShowWindow()
    {
        var w = GetWindow<MetroidvaniaFolderSetup>("Metroidvania Setup");
        w.minSize = new Vector2(500, 580);
        w.CheckExisting();
    }

    private void CheckExisting()
    {
        _alreadyExists =
            Directory.Exists(Path.Combine(Application.dataPath, "Art")) ||
            Directory.Exists(Path.Combine(Application.dataPath, "Scripts"));
    }

    // ── GUI ───────────────────────────────────────────────────────────
    private void OnGUI()
    {
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), COLOR_BG);
        DrawHeader();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        DrawWarning();
        DrawDescriptions();
        DrawPreviewToggle();
        if (_showPreview) DrawFolderPreview();
        EditorGUILayout.EndScrollView();

        DrawFooter();
    }

    private void DrawHeader()
    {
        EditorGUI.DrawRect(new Rect(0, 0, position.width, 70), COLOR_HEADER);

        var titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize  = 18,
            alignment = TextAnchor.MiddleCenter,
            normal    = { textColor = COLOR_ACCENT }
        };
        var subStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize  = 11,
            alignment = TextAnchor.MiddleCenter,
            normal    = { textColor = new Color(0.65f, 0.65f, 0.70f) }
        };

        GUILayout.Space(10);
        GUILayout.Label("⚔  METROIDVANIA PROJECT SETUP", titleStyle);
        GUILayout.Label("Solo dev · Portfolio · Unity 2022+", subStyle);
        GUILayout.Space(8);
    }

    private void DrawWarning()
    {
        if (!_alreadyExists) return;
        var style = new GUIStyle(EditorStyles.helpBox)
        {
            fontSize = 11,
            normal   = { textColor = COLOR_WARN }
        };
        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField(
            "⚠  Carpetas existentes detectadas. Solo se crearán las que falten. " +
            "Ningún archivo será eliminado.", style);
    }

    private void DrawDescriptions()
    {
        var headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            normal   = { textColor = COLOR_ACCENT }
        };
        var nameStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize   = 11,
            fixedWidth = 120,
            normal     = { textColor = Color.white }
        };
        var descStyle = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true,
            fontSize = 10,
            normal   = { textColor = new Color(0.75f, 0.75f, 0.80f) }
        };

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("MÓDULOS", headerStyle);
        EditorGUILayout.Space(4);

        foreach (var kv in DESCRIPTIONS)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("📁 " + kv.Key, nameStyle);
            GUILayout.Label(kv.Value, descStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
        }
    }

    private void DrawPreviewToggle()
    {
        EditorGUILayout.Space(8);
        var style = new GUIStyle(EditorStyles.foldoutHeader)
        {
            fontSize = 11,
            normal   = { textColor = new Color(0.60f, 0.60f, 0.65f) }
        };
        _showPreview = EditorGUILayout.Foldout(
            _showPreview,
            $"  Ver árbol completo ({FOLDERS.Length} carpetas)",
            true, style);
    }

    private void DrawFolderPreview()
    {
        var treeStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 10,
            normal   = { textColor = new Color(0.55f, 0.85f, 0.70f) },
            richText = true
        };

        Rect area = GUILayoutUtility.GetRect(0, FOLDERS.Length * 15 + 20);
        EditorGUI.DrawRect(area, new Color(0.08f, 0.10f, 0.10f));

        float y = area.y + 8;
        foreach (string folder in FOLDERS)
        {
            int    depth = folder.Split('/').Length - 1;
            string pad   = new string(' ', depth * 3);
            string name  = folder.Contains("/")
                ? folder.Substring(folder.LastIndexOf('/') + 1)
                : folder;

            GUI.Label(
                new Rect(area.x + 10, y, area.width - 10, 14),
                $"{pad}<color=#20cc88>📁</color> <color=#ccffee>{name}</color>",
                treeStyle);
            y += 14;
        }
    }

    private void DrawFooter()
    {
        EditorGUILayout.Space(6);

        if (!string.IsNullOrEmpty(_statusMessage))
        {
            var statusStyle = new GUIStyle(EditorStyles.helpBox)
            {
                fontSize  = 11,
                alignment = TextAnchor.MiddleCenter,
                normal    = { textColor = COLOR_ACCENT }
            };
            EditorGUILayout.LabelField(_statusMessage, statusStyle);
        }

        EditorGUILayout.Space(4);

        var btnStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize    = 14,
            fontStyle   = FontStyle.Bold,
            fixedHeight = 44,
            normal      = { textColor = Color.black, background = MakeTex(2, 2, COLOR_BTN) },
            hover       = { textColor = Color.black, background = MakeTex(2, 2, COLOR_BTN_HOV) }
        };

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("⚡  CREAR ESTRUCTURA DE CARPETAS", btnStyle))
            CreateFolderStructure();
        GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
    }

    // ── Lógica ────────────────────────────────────────────────────────
    private void CreateFolderStructure()
    {
        int created = 0, skipped = 0;

        AssetDatabase.StartAssetEditing();
        try
        {
            foreach (string folder in FOLDERS)
            {
                string fullPath = Path.Combine(Application.dataPath, folder);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    File.WriteAllText(
                        Path.Combine(fullPath, ".gitkeep"),
                        "# Carpeta vacía rastreada por Git\n");
                    created++;
                }
                else skipped++;
            }

            CreateReadme();
            CreateChangelog();
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }

        _statusMessage = $"✅  {created} creadas · {skipped} ya existían · README + CHANGELOG generados";
        _alreadyExists = true;

        Debug.Log($"[MetroidvaniaSetup] {created} nuevas, {skipped} existentes.");
        EditorUtility.DisplayDialog(
            "¡Listo!",
            $"{created} carpetas creadas.\n{skipped} ya existían.\n\nRevisa Assets/ en el Project panel.",
            "👍");
    }

    private void CreateReadme()
    {
        string dir  = Path.Combine(Application.dataPath, "Documentation");
        string path = Path.Combine(dir, "FOLDER_STRUCTURE.md");
        Directory.CreateDirectory(dir);
        if (File.Exists(path)) return;

        File.WriteAllText(path,
@"# 📁 Estructura de Carpetas — Metroidvania Pixel Art

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
");
    }

    private void CreateChangelog()
    {
        string dir  = Path.Combine(Application.dataPath, "Documentation");
        string path = Path.Combine(dir, "CHANGELOG.md");
        Directory.CreateDirectory(dir);
        if (File.Exists(path)) return;

        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        File.WriteAllText(path,
"# Changelog\n\n" +
"Formato: [Keep a Changelog](https://keepachangelog.com/es/1.0.0/) · " +
"Versiones: `MAJOR.MINOR.PATCH`\n\n" +
"- **MAJOR** — cambio de arquitectura o diseño importante\n" +
"- **MINOR** — sistema o feature nuevo\n" +
"- **PATCH** — bugfix o ajuste menor\n\n" +
"---\n\n" +
"## [Sin publicar]\n\n" +
"### Añadido\n- \n\n" +
"---\n\n" +
"## [0.1.0] — " + today + "\n\n" +
"### Añadido\n" +
"- Proyecto creado\n" +
"- Estructura de carpetas para Metroidvania Pixel Art\n\n" +
"---\n\n" +
"<!-- PLANTILLA\n" +
"## [X.Y.Z] — YYYY-MM-DD\n" +
"### Añadido\n- \n" +
"### Cambiado\n- \n" +
"### Arreglado\n- \n" +
"### Eliminado\n- \n" +
"-->\n");
    }

    private static Texture2D MakeTex(int w, int h, Color col)
    {
        var tex = new Texture2D(w, h);
        var px  = new Color[w * h];
        for (int i = 0; i < px.Length; i++) px[i] = col;
        tex.SetPixels(px);
        tex.Apply();
        return tex;
    }
}
#endif
