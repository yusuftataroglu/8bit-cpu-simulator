# 8-bit Multi-cycle CPU Simulation

This is a custom-built 8-bit CPU designed and simulated using the [Digital](https://github.com/hneemann/Digital) logic simulator.

### Features
- 16-instruction custom instruction set
- Supports variable-length instructions:
    8-bit: 4-bit opcode + 4-bit operand
    16-bit (2 bytes): 4-bit opcode + 8-bit immediate value
- Microcoded control using EEPROM
- ZF (Zero) and CF (Carry) flags
- Separate instruction and data memory

### Sample Programs
- Fibonacci sequence generator (0–255)
- Simple counter (0–255-0)

### Screenshots
![circuit_overview](media/circuit_overview.png)

### Demo
![fibonacci_run](media/fibonacci_run.gif)

---
### Architecture Details
- Data Bus Width: 8-bit
- Address Bus Width: 4-bit (Program Counter & MAR)
- Registers:
    - Register A: 8-bit (Accumulator)
    - Register B: 8-bit
    - Program Counter Register: 4-bit
    - Memory Address Register: 4-bit
    - Instruction Address Register: 4-bit
- ALU:
    - 8-bit Adder / Subtractor
- Flags:
    - ZF (Zero Flag)
    - CF (Carry Flag)
- Memory:
    - Instruction Memory: 32-bit (4 × 8-bit instructions)
    - Data Memory: 32-bit (4 × 8-bit locations)
- Control Logic:
    - Microcode-based control using EEPROM
    - Instruction decoder reacts to Microcode Counter, Opcode, ZF, and CF
    - Each instruction runs in multiple microsteps
- Output:
    - Four 7-segment displays connected to OUT instruction
- Custom Instruction Set:
    - 16 custom instructions including LDA, ADD, SUB, JMP, JC, JZ, etc.
    - Immediate and memory-based data operations

### File Structure

- `project/`: Main `.dig` simulation files (full CPU circuit)
- `programs/`: Instruction memory programs (e.g., Fibonacci, counter)
- `instruction_decoder/`: Microcode EEPROM generator
- `seven_segment_display_decoder/`: Optional decoders for output display
- `media/`: Circuit overview and simulation screenshots or GIFs

---

### Requirements
- [Digital Logic Simulator](https://github.com/hneemann/Digital)

