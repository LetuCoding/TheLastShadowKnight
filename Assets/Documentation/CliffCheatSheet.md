# git-cliff cheatsheet

## Commits — formato

| Tipo | Uso | Sección en CHANGELOG |
|------|-----|----------------------|
| `feat: ...` | Feature o sistema nuevo | Añadido |
| `fix: ...` | Bugfix o corrección | Arreglado |
| `refactor: ...` | Reestructura sin cambiar comportamiento | Cambiado |
| `art: ...` | Sprites, animaciones, VFX | Arte |
| `docs: ...` | README, CHANGELOG, comentarios | Documentación |

> Un commit = un cambio = un tipo. Nunca mezcles tipos en el mismo mensaje.

---

## Flujo diario

1. Trabajas y haces commits normales
   ```
   git commit -m "feat: añadir salto del jugador"
   ```

2. Cuando terminas un bloque, etiquetas la versión
   ```
   git tag 0.2.0
   ```

3. Generas el CHANGELOG automáticamente
   ```
   git cliff --output Assets/Documentation/CHANGELOG.md
   ```

4. Commiteas y subes todo
   ```
   git add Assets/Documentation/CHANGELOG.md
   git commit -m "docs: actualizar changelog v0.2.0"
   git push
   git push --tags
   ```

---

## Comandos útiles

| Comando | Para qué |
|---------|----------|
| `git cliff --output ...` | Genera o actualiza el CHANGELOG |
| `git cliff -vv --output ...` | Verbose — muestra qué commits saltó y por qué |
| `git tag X.Y.Z` | Etiqueta la versión actual |
| `git push --tags` | Sube las etiquetas a GitHub |
| `git log --oneline` | Ver historial de commits resumido |
| `git tag` | Ver todas las versiones etiquetadas |

---

## Versiones — cuándo subir cada número

| Número | Cuándo |
|--------|--------|
| `MAJOR` | Cambio de arquitectura importante — rehaces un sistema entero |
| `MINOR` | Sistema o feature nuevo — doble salto, mapa, guardado… |
| `PATCH` | Bugfix o ajuste pequeño — colisión, valor de velocidad… |

> Empieza en `0.1.0`. El 0 inicial indica que aún estás en desarrollo.
> Sube a `1.0.0` cuando el juego sea jugable y presentable.

---

## Ejemplo de historial

```
0.1.0 — proyecto creado
0.2.0 — movimiento del jugador completo
0.2.1 — corregido salto fantasma en bordes de plataforma
0.3.0 — sistema de combate
0.3.1 — corregido hitbox del ataque
0.4.0 — primer enemigo con IA
1.0.0 — primera versión jugable
```
