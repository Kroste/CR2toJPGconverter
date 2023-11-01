using CommandLine;

public class Options
{
  [Value(0, MetaName = "directory", HelpText = "The path to the directory that contains the CR2 files.", Required = true)]
  public string? Directory { get; set; }

  [Option('r', "recursive", Default = false, HelpText = "Specifies whether subfolders should be searched recursively.")]
  public bool Recursive { get; set; }

  [Option('d', "delete", Default = false, HelpText = "Specifies whether the CR2 file should be deleted after conversion.")]
  public bool Delete { get; set; }
}
