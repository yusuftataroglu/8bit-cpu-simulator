namespace ControlLogic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Kontrol sinyalleri
            const int ZFI = 0x200000, CFI = 0x100000,
                    DMAI = 0x080000, DMI = 0x040000, DMO = 0x020000, NXT = 0x010000,
                    HLT = 0x008000, IMAI = 0x004000, IMI = 0x002000, IMO = 0x001000,
                    IRO = 0x000800, IRI = 0x000400, AI = 0x000200, AO = 0x000100,
                    SUMO = 0x000080, SUB = 0x000040, BO = 0x00020, BI = 0x000010,
                    OUT = 0x000008, PCE = 0x000004, PCO = 0x000002, J = 0x000001;

            // Instruction tanımları
            var instructions = new Dictionary<int, int[]>
            {
                [0x0] = new[] // LDA 
                {
                    IRO | DMAI,
                    DMO | AI,
                    NXT
                },
                [0x1] = new[] // LDB
                {
                    IRO | DMAI,
                    DMO | BI,
                    NXT
                },
                [0x2] = new[] // LDAI
                {
                    PCO | IMAI,
                    IMO | AI | PCE,
                    NXT
                },
                [0x3] = new[] // LDBI
                {
                    PCO | IMAI,
                    IMO | BI | PCE,
                    NXT
                },
                [0x4] = new[] // STRA
                {
                    IMO | DMAI,
                    AO | DMI,
                    NXT
                },
                [0x5] = new[] // STRB
                {
                    IMO | DMAI,
                    BO | DMI,
                    NXT
                },
                [0x6] = new[] // STRI
                {
                    IMO | DMAI,
                    PCO | IMAI,
                    IMO | DMI | PCE
                },
                [0x7] = new[] // ADD
                {
                    SUMO | AI | ZFI | CFI,
                    NXT
                },
                [0x8] = new[] // SUB
                {
                    SUMO | AI | ZFI | CFI | SUB,
                    NXT
                },
                [0x9] = new[] // ADDI
                {
                    PCO | IMAI,
                    IMO | BI,
                    SUMO | AI | ZFI | CFI | PCE
                },
                [0xA] = new[] // SUBI
                {
                    PCO | IMAI,
                    IMO | BI,
                    SUMO | AI | ZFI | CFI | SUB | PCE
                },
                [0xB] = new[] // JMP
                {
                    IMO | J,
                    NXT
                },
                [0xC] = new[] // JZ
                {
                    IMO | J,
                    NXT
                },
                [0xD] = new[] // JC
                {
                    IMO | J,
                    NXT
                },
                [0xE] = new[] // OUT
                {
                    OUT | AO,
                    NXT
                },
                [0xF] = new[] // HLT
                {
                    HLT,
                    NXT
                }
            };

            string path = @"C:\Users\yusuf\OneDrive\Belgeler\8bit-cpu-simulator\instruction_decoder\control_logic_eeprom.hex";
            File.WriteAllText(path, "v2.0 raw\n");
            for (int addr = 0; addr < 1024; addr++)
            {
                int cf = (addr >> 9) & 0x1;
                int zf = (addr >> 8) & 0x1;
                int opcode = (addr >> 4) & 0xF;   // Instruction kodu
                int step = addr & 0xF;            // Mikrocode adımı (0–15)

                int T0 = PCO | IMAI;
                int T1 = IMO | IRI | PCE;

                if (step == 0)
                {
                    File.AppendAllText(path, T0.ToString("x") + "\n"); // Her zaman T0'da: PC -> MAR
                }
                else if (step == 1)
                {
                    File.AppendAllText(path, T1.ToString("x") + "\n"); // Her zaman T1'de: RAM[PC] -> IR, PC++
                }
                else if (instructions.ContainsKey(opcode) && (step - 2) < instructions[opcode].Length)
                {
                    // JZ ve JC aktif olmadığı durumda sonraki buyruğa geç
                    if (opcode == 0xC && zf == 0)
                    {
                        File.AppendAllText(path, NXT.ToString("x") + "\n");
                    }
                    else if (opcode == 0xD && cf == 0)
                    {
                        File.AppendAllText(path, NXT.ToString("x") + "\n");
                    }
                    // JZ için sadece ZF = 1 olduğunda çalıştır
                    else if (opcode == 0xC && zf == 1 && (step - 2) < instructions[opcode].Length)
                    {
                        File.AppendAllText(path, instructions[opcode][step - 2].ToString("x") + "\n");
                    }
                    // JC için sadece CF = 1 olduğunda çalıştır
                    else if (opcode == 0xD && cf == 1 && (step - 2) < instructions[opcode].Length)
                    {
                        File.AppendAllText(path, instructions[opcode][step - 2].ToString("x") + "\n");
                    }
                    // Diğer buyruklar ZF ve CF fark etmeksizin çalışmalı
                    else if (opcode != 0xC && opcode != 0xD && (step - 2) < instructions[opcode].Length)
                    {
                        File.AppendAllText(path, instructions[opcode][step - 2].ToString("x") + "\n");
                    }
                    else
                    {
                        File.AppendAllText(path, "0\n");
                    }
                }
                else
                {
                    File.AppendAllText(path, "0\n");
                }
            }

        }
    }
}
