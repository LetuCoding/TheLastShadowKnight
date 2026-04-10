# Changelog

## [Sin publicar] — 2026-04-10

### Documentación

- update CHANGELOG for v0.2.0


- include commit body in CHANGELOG template


## [0.2.0] — 2026-04-10

### Arreglado

- correct wall-jump arc and state transition logic

- JumpState.Enter() only calls Jump() for normal jumps; wall-jump
  impulse is already applied by Player.WallJump() before transition
  immediate re-attachment to the wall during the lockout window
- WallState delegates jump impulse to Player.WallJump() before calling
  ChangeState so IsWallJumping is true when JumpState.Enter() runs
- IdleState: removed duplicate using directive



### Añadido

- add FallState and DashState to FSM

- FallState handles airborne downward movement with transitions
  to Idle (landing), WallState (wall contact) and DashState
- DashState drives a fixed-duration horizontal dash at gravity 0,
  restores velocity on exit and routes to Idle or Fall on finish
- DashState reads dashSpeed and dashDuration from Player blackboard
  so values are tunable from the Inspector without touching code



### Cambiado

- centralise all Rigidbody writes in MovementComponent

- Move(input, speed, isGrounded) encapsulates the three-rule horizontal
  logic: full control with input, air no-op, ground brake
- WallJump(forceX, forceY, direction) moved from Player to MovementComponent
  so Player never accesses Rigidbody directly
- Player.WallJump() now only resolves direction and delegates to
  MovementComponent, keeping physics and game-logic concerns separate
- WallJumpLockCoroutine zeroes MoveInput each frame, triggering the air
  no-op path in Move() automatically — no extra flags required
- Removed dead no-op velocity assignment and dangling semicolon



### Documentación

- changelog 0.1.6


## [0.1.6] — 2026-04-10

### Arreglado

- Jump state not changing the right way



### Documentación

- changelog updated


## [0.1.5] — 2026-04-10

### Añadido

- FSM and PlayerState created, idle state and walk state added


- idle state, movement and jump state added


## [0.1.4] — 2026-04-10

### Añadido

- BGM manager working on main menu


## [0.1.3] — 2026-04-10

### Añadido

- extenject used for audiomanager, still in work


## [0.1.1] — 2026-04-10

### Añadido

- Bootstrap scene added, main menu background added


- Bootstrap scene added, main menu background added



### Documentación

- changelog updated v0.1.0


- cheatsheet added


## [0.1.0] — 2026-04-10

### Añadido

- project folder structure added


