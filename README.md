# Compact Graphics V2
A re-write from the original V2 Includes Mouse support, faster drawing and frame buffering.
## What Is This Libary?
Compact graphics is a display libary for the C# console,allowing for advanced GUI and Enviroment based interactions.
- Display animations at high framerates without flicker.
- Full 16 color support.
- Dual layer drawing with the background and forground.
- track and respond to mouse based events.
- Keyboard input.
- Includes basic GUI framework widget based system.

## Usage
1. Add a refrence to CompactGraphics.dll
2. Declare a `Graphics` object to use. Do not make more than one.
3. `Draw();` somthing.
4. `pushFrame();`
5. repeat.

## Bugs
Known Bugs
- Re-sizing the window does bad things.
- Trying to draw at the edge of the screen sometimes has issues.
- Frame buffering and frame capping is weird.
**Found somthing else? Do tell.**
