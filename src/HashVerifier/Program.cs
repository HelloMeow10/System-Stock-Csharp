using System.Security.Cryptography;
using System.Text;

var password = "admin123";
var username = "admin";
var legacyComposite = password + username;
using var sha = SHA256.Create();
var legacyHash = sha.ComputeHash(Encoding.UTF8.GetBytes(legacyComposite));
Console.WriteLine($"Legacy Hash (Hex): {BitConverter.ToString(legacyHash).Replace("-", "")}");
