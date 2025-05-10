
using BrookFlasher;

bool noChecksum = false;
string filePath = "";
string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());

var s = Environment.GetCommandLineArgs();
if (s.Length == 2)
{
    Console.WriteLine("WARNING!!!!!!!!!");
    Console.WriteLine("You are running BrookFlasher in NoChecksum Mode!!!!");
    Console.WriteLine("This will not check if the Firmware you added is save to use!");
    Console.Write("By Continuing you agree that I am not liable for any damages!\nContinue anyways? (y/N): ");
    var enteredKey2 = Console.ReadKey();
    if (enteredKey2.KeyChar.ToString().ToLower() != "y")
    {
        Environment.Exit(0);
    }
}
Console.Clear();
Console.WriteLine("BrookFlasher v1 by NicoAICP");
Console.WriteLine("----------------------------------------");
foreach (string file in files)
    {
        if (file.EndsWith(".hex"))
        {
            filePath = file;
        }
    }
if(!File.Exists(filePath))
{
    Console.WriteLine("No Firmware in the .hex format found!");
    Console.WriteLine("Press Enter to exit.");
    Console.ReadLine();
    Environment.Exit(0);
}
Console.Write("Found a firmware!" + $"\nFirmware found: {filePath}\nDo you want to Install this Firmware?\nThis may break your Device!\nBy Continuing you agree that I am not liable for any damages!\nContinue with the Installation? (y/N): ");
var enteredKey = Console.ReadKey();
if(enteredKey.KeyChar.ToString().ToLower() == "y")
{
    Console.Clear();
    Console.WriteLine("Installing new Firmware to device!");
    if (!BrookFlash.Connect_To_DFU(500))
    {
        Console.Clear();
        string error = string.Format("Could not find compatible Brook Device.");
        Console.WriteLine(error);
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
        Environment.Exit(0);
    }
    if (!BrookFlash.FirmwareFlashing(filePath, noChecksum))
    {
        Console.Clear();
        string error2 = string.Format("Flashing the Firmware has failed.");
        Console.WriteLine(error2);
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
        Environment.Exit(0);
    }
    BrookFlash.FinishFirmwareFlashing();
    Console.WriteLine("Flashing Finished!");
    Console.WriteLine("Press Enter to exit.");
    Console.ReadLine();
    Environment.Exit(0);
}
else
{
    Console.Clear();
    Console.WriteLine("You aborted the Installation.");
    Console.WriteLine("Press Enter to exit.");
    Console.ReadLine();
    Environment.Exit(0);
}



