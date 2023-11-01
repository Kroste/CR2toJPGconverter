using CommandLine;
using ImageMagick;
using Serilog;

class Program
{
  static void Main(string[] args)
  {
    ConfigureLogging();

    Parser.Default.ParseArguments<Options>(args)
        .WithParsed(opts => ConvertAndDeleteCR2(opts.Directory, opts.Recursive, opts.Delete))
        .WithNotParsed<Options>((errs) => HandleParseError(errs));
  }

  static void ConfigureLogging()
  {
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("conversion_log.txt")
        .CreateLogger();
  }

  static void ConvertAndDeleteCR2(string? directory, bool recursive, bool delete)
  {
    if (string.IsNullOrEmpty(directory))
    {
      Log.Error("The directory cannot be null or empty.");
      return;
    }

    if (recursive)
    {
      foreach (string subDirectory in Directory.GetDirectories(directory))
      {
        ConvertAndDeleteCR2(subDirectory, recursive, delete);
      }
    }

    foreach (string file in Directory.GetFiles(directory, "*.cr2"))
    {
      string jpegFilePath = Path.ChangeExtension(file, ".jpg");
      try
      {
        using (MagickImage image = new MagickImage(file))
        {
          image.Write(jpegFilePath);
        }
        if (delete)
        {
          File.Delete(file);
          Log.Information($"Successfully converted and deleted {file}");
        }
        else
        {
          Log.Information($"Successfully converted {file}");
        }
      }
      catch (Exception ex)
      {
        Log.Error($"Error converting {file}: {ex.Message}");
      }
    }
  }
  static void HandleParseError(IEnumerable<Error> errs)
  {
    // Optional: Handhabung von Parse-Fehlern, falls erforderlich
  }
}
