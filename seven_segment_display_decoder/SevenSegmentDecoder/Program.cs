namespace SevenSegmentEEPROMGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            // 7 segment için 0–9 arasındaki hex karşılıkları (a–g)
            byte[] sevenSegmentDecoder = new byte[10]
            {
                0x7E, // 0
                0x30, // 1
                0x6D, // 2
                0x79, // 3
                0x33, // 4
                0x5B, // 5
                0x5F, // 6
                0x70, // 7
                0x7F, // 8
                0x7B  // 9
            };

            // Dosya yolları
            string pathOnes = @"C:\Users\yusuf\OneDrive\Belgeler\8bit-cpu-simulator\seven_segment_display_decoder\seven_segment_ones.hex";
            string pathTens = @"C:\Users\yusuf\OneDrive\Belgeler\8bit-cpu-simulator\seven_segment_display_decoder\seven_segment_tens.hex";
            string pathHundreds = @"C:\Users\yusuf\OneDrive\Belgeler\8bit-cpu-simulator\seven_segment_display_decoder\seven_segment_hundreds.hex";
            string pathSign = @"C:\Users\yusuf\OneDrive\Belgeler\8bit-cpu-simulator\seven_segment_display_decoder\seven_segment_sign.hex";

            // Her dosyayı sıfırla ve "v2.0 raw" başlığı ekle
            File.WriteAllText(pathOnes, "v2.0 raw\n");
            File.WriteAllText(pathTens, "v2.0 raw\n");
            File.WriteAllText(pathHundreds, "v2.0 raw\n");
            File.WriteAllText(pathSign, "v2.0 raw\n");

            // EEPROM adresleri 0–511 (9-bit adres)
            for (int addr = 0; addr < 512; addr++)
            {
                bool useSigned = (addr & 0x100) != 0; // A8
                int value = addr & 0xFF;              // A0–A7

                int number;
                if (useSigned && value >= 128)
                    number = value - 256;
                else
                    number = value;

                int absVal = Math.Abs(number);

                int ones = absVal % 10;
                int tens = (absVal / 10) % 10;
                int hundreds = (absVal / 100) % 10;

                byte segOnes = sevenSegmentDecoder[ones];
                byte segTens = sevenSegmentDecoder[tens];
                byte segHundreds = sevenSegmentDecoder[hundreds];
                byte segSign = (number < 0) ? (byte)0x01 : (byte)0x00;

                // Dosyalara satır satır hex olarak yaz
                File.AppendAllLines(pathOnes, new[] { segOnes.ToString("X2") });
                File.AppendAllLines(pathTens, new[] { segTens.ToString("X2") });
                File.AppendAllLines(pathHundreds, new[] { segHundreds.ToString("X2") });
                File.AppendAllLines(pathSign, new[] { segSign.ToString("X2") });
            }

            Console.WriteLine("Tüm EEPROM dosyaları başarıyla oluşturuldu!");
        }
    }
}
