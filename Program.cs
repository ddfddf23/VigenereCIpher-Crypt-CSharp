namespace VigenereCipher_Crypt
{
   

    public class VigenereCipher
    {
        const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string letters { get; }

        public VigenereCipher(string alphabet = null)
        {
            letters = string.IsNullOrEmpty(alphabet) ? defaultAlphabet : alphabet;
        }

        private string GetRepeatKey(string s, int n)
        {
            string? p = s;
            while (p.Length < n)
            {
                p += p;
            }

            return p[..n];
        }

        private string DoMagic(string text, string password, bool encrypting = true)
        {
            string? repeatKey = GetRepeatKey(password, text.Length);
            string? returnValue = "";
            int q = letters.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = letters.IndexOf(text[i]);
                var codeIndex = letters.IndexOf(repeatKey[i]);
                if (letterIndex < 0) //Заметка: в случае отсутствия буквы в алфавите просто не шифруем её
                {
                    returnValue += text[i].ToString();
                }
                else
                {
                    returnValue += letters[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                }
            }

            return returnValue;
        }

        public string Encrypt(string plainMessage, string password)
            => DoMagic(plainMessage, password);

        public string Decrypt(string encryptedMessage, string password)
            => DoMagic(encryptedMessage, password, false);
    }

    class Program
    {
        static void Main(string[] args)
        {
            VigenereCipher cipher = new VigenereCipher("АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ_");
            Console.Write("Открытый текст: ");
            string inputText = Console.ReadLine().ToUpper().Replace(' ', '_'); //Передаем в программу введенный текст, преобразуя его в КАПС + заменяя пробелы на _
            Console.Write("Ключ шифрования: ");
            string password = Console.ReadLine().ToUpper();
            string encryptedText = cipher.Encrypt(inputText, password);
            Console.WriteLine($"Зашифрованное сообщение: {encryptedText}");
            Console.WriteLine($"Расшифрованное сообщение: {cipher.Decrypt(encryptedText, password).Replace('_', ' ')}");
            Console.ReadLine();
        }
    }
}