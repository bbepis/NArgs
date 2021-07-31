# NArgs
Simple single-file argument parser for C#

Just copy NArgs.cs into your project and you're ready to go.

### Usage

```cs
using NArgs;

class MyArguments : IArgumentCollection
{
    public IList<string> Values { get; set; }

    [CommandDefinition("r", "required", Description = "This is required", Required = true)]
    public string RequiredArg { get; set; }

    [CommandDefinition("collection", Description = "A collection of strings", Required = true)]
    public IList<string> StringCollection { get; set; }

    [CommandDefinition("h", "help", Description = "Prints help text")]
    public bool Help { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        MyArguments arguments = Arguments.Parse<MyArguments>(args);

        if (arguments.Values.Count == 0 || arguments.Help)
        {
            Console.WriteLine(Arguments.PrintLongHelp<MyArguments>(
                "MyApp v1.0.0, by me",
                "Usage: MyApp [options] <values>"));
            return;
        }
        
        ...
    }
}
```

`PrintLongHelp` in the example writes this to console:

```
MyApp v1.0.0, by me
Usage: MyApp [options] <values>


  -h, --help                                 Prints help text

  -r <value>, --required <value>             This is required

  --collection <value>                       A collection of strings
```

An argument class must inherit from `IArgumentCollection`. There are three property types supported:

- `bool`: Is `true` if the argument was provided, `false` if not. `Required` is not applicable to this argument type.
- `string`: Is set to the value of `--stringArg <value>`, otherwise `null` if not provided.
- `IList<string>`: Similar to the above, however collects multiple values into a single list. Does *not* split the value on comma or anything else, instead appends multiple usages (i.e. `--listArg value1 --listArg value2`) into a single list. List is empty if not provided.
