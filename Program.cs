using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    class Program
    {
        // Configuration Constants
        private const int MaxInputLength = 100;
        private const int TypewriterDelay = 30;
        private const string DefaultUserName = "Guest";
        private const string AudioFilePath = "welcome.wav";

        // Cybersecurity Knowledge Base
        private static readonly Dictionary<string, string> CybersecurityResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Core Interactions
            { "how are you", "🤖 Cybersecurity Status Report:\n" +
                           "• Threat detection: Active\n" +
                           "• Security protocols: Enabled\n" +
                           "• Database updates: Current\n" +
                           "Ready to assist with all security inquiries!\n" +
                           "[TIP] Type 'exit' to end session" },

            { "help", "🆘 Available Commands:\n" +
                     "• Ask about: password safety, phishing, malware\n" +
                     "• Topics: ransomware, 2fa, vpn, encryption\n" +
                     "• Type 'exit' to end session\n" +
                     "• Type 'help' to show this message" },

            // Security Fundamentals
            { "purpose", "🔍 Primary Mission Objectives:\n" +
                        "1. Provide real-time cybersecurity guidance\n" +
                        "2. Educate users about digital threats\n" +
                        "3. Offer actionable protection strategies\n" +
                        "4. Promote awareness of safe online practices" },

            { "password safety", "🔑 Advanced Password Management:\n" +
                                "• Minimum 15 characters with mixed types\n" +
                                "• Use passphrases (e.g., 'PurpleTiger$RunsFast9')\n" +
                                "• Implement biometric authentication\n" +
                                "• Regular audits using HaveIBeenPwned\n" +
                                "• Enterprise password vault solutions" },

            // Threat Protection
            { "phishing", "🎣 Comprehensive Phishing Defense:\n" +
                         "Technical:\n" +
                         "• DMARC/DKIM/SPF records\n" +
                         "• Advanced email filtering\n" +
                         "Human:\n" +
                         "• Phishing simulations\n" +
                         "• Reporting procedures" },

            { "malware", "🦠 Malware Protection Strategies:\n" +
                        "• Application allowlisting\n" +
                        "• Vulnerability scans\n" +
                        "• Network segmentation\n" +
                        "• Privilege management\n" +
                        "• Isolated backups" },

            { "ransomware", "💣 Ransomware Prevention:\n" +
                           "• Air-gapped backups\n" +
                           "• Macro script disabling\n" +
                           "• Attachment filtering\n" +
                           "• Incident response plan\n" +
                           "• Endpoint detection" },

            // Security Technologies
            { "2fa", "🔐 Two-Factor Authentication Guide:\n" +
                     "1. Authenticator apps\n" +
                     "2. Hardware security keys\n" +
                     "3. Biometric verification\n" +
                     "4. Backup codes\n" +
                     "5. Mandatory for critical systems" },

            { "vpn", "🌐 VPN Best Practices:\n" +
                    "• Always-on connection\n" +
                    "• Disabled split tunneling\n" +
                    "• WireGuard/OpenVPN\n" +
                    "• MFA requirement\n" +
                    "• Connection audits" },

            // Data Protection
            { "encryption", "🔏 Data Encryption Standards:\n" +
                           "• AES-256 at rest\n" +
                           "• TLS 1.3+ in transit\n" +
                           "• E2E encryption\n" +
                           "• Quantum resistance\n" +
                           "• Key management" },

            { "social engineering", "🎭 Social Engineering Defense:\n" +
                                   "• Request verification\n" +
                                   "• Credential protection\n" +
                                   "• Security training\n" +
                                   "• Physical security\n" +
                                   "• Reporting channels" }
        };

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true;
                DisplayExitMessage();
                Environment.Exit(0);
            };

            InitializeChatbot();
            RunInteractionLoop();
            DisplayExitMessage();
        }

        static void InitializeChatbot()
        {
            PlayWelcomeAudio();
            DisplaySecurityHeader();
            RegisterUser();
        }

        static void PlayWelcomeAudio()
        {
            try
            {
                using var player = new SoundPlayer(AudioFilePath);
                player.PlaySync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔇 Audio Error: {ex.Message}");
            }
        }

        static void DisplaySecurityHeader()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
            ██████╗ ██╗   ██╗███████╗███████╗██████╗ 
            ██╔══██╗╚██╗ ██╔╝██╔════╝██╔════╝██╔══██╗
            ██████╔╝ ╚████╔╝ ███████╗█████╗  ██████╔╝
            ██╔══██╗  ╚██╔╝  ╚════██║██╔══╝  ██╔══██╗
            ██████╔╝   ██║   ███████║███████╗██║  ██║
            ╚═════╝    ╚═╝   ╚══════╝╚══════╝╚═╝  ╚═╝");
            Console.ResetColor();
        }

        static void RegisterUser()
        {
            Console.Write("\n[SYSTEM] Enter clearance identity: ");
            var userName = Console.ReadLine()?.Trim();
            ValidateUserIdentity(userName);
        }

        static void ValidateUserIdentity(string? input)
        {
            var sanitizedName = string.IsNullOrWhiteSpace(input)
                ? DefaultUserName
                : new string(input.Take(25)
                    .Where(c => char.IsLetterOrDigit(c) || c == '_')
                    .ToArray());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[STATUS] Access granted: {sanitizedName}");
            Console.WriteLine("[TIP] Type 'help' for commands");
            Console.ResetColor();
        }

        static void RunInteractionLoop()
        {
            while (true)
            {
                try
                {
                    var userInput = GetUserQuery();
                    if (IsExitCommand(userInput)) break;

                    var response = ProcessSecurityQuery(userInput);
                    PresentResponse(response);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR] System failure: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        static string GetUserQuery()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n[QUERY] Enter security question (type 'exit' to quit): ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        static bool IsExitCommand(string input)
        {
            return input.Equals("exit", StringComparison.OrdinalIgnoreCase)
                || input.Equals("quit", StringComparison.OrdinalIgnoreCase);
        }

        static string ProcessSecurityQuery(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "[ALERT] Null input detected";

            var cleanInput = new string(input
                .Where(c => !char.IsPunctuation(c))
                .ToArray())
                .Trim();

            if (cleanInput.Length > MaxInputLength)
                return $"[WARNING] Input exceeds {MaxInputLength} characters";

            if (IsExitCommand(cleanInput))
                return string.Empty;

            if (cleanInput.Contains("help", StringComparison.OrdinalIgnoreCase))
                return CybersecurityResponses["help"];

            var queryWords = cleanInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in CybersecurityResponses)
            {
                if (queryWords.Any(word =>
                    entry.Key.Split(' ').Any(keyWord =>
                        keyWord.Equals(word, StringComparison.OrdinalIgnoreCase))))
                {
                    return entry.Value;
                }
            }

            return "[NOTICE] Unrecognized query. Type 'help' for commands";
        }

        static void PresentResponse(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            int currentIndex = 0;
            while (currentIndex < message.Length)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Console.Write(message.Substring(currentIndex));
                    break;
                }

                Console.Write(message[currentIndex]);
                Thread.Sleep(TypewriterDelay);
                currentIndex++;
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        static void DisplayExitMessage()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n[SYSTEM] Security session terminated");
            Console.WriteLine("Reminder: Always verify SSL certificates!");
            Console.ResetColor();
        }
    }
}