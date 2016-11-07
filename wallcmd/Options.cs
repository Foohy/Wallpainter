using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace wallcmd
{
    public struct Tuple
    {
        float x;
        float y;
    }

    class SetOptions
    {
        [Option('n', "name", MutuallyExclusiveSet = "name", HelpText = "Window name to set as the desktop background")]
        public string windowName { get; set; }

        [Option('c', "class", MutuallyExclusiveSet = "class", HelpText = "Class name of the window to be set as the background")]
        public string className { get; set; }

        [OptionList('b', "bounds", Separator = ' ', HelpText = "Specify the bounds of the window. Ordered as x, y, width, height, and is measured in pixels")]
        public IList<string> bounds { get; set; }

    }

    class StartOptions
    {
        [Option('p', "path", Required = true, HelpText = "Path to the executable to start")]
        public string path { get; set; }

        [Option('a', "args", Required = false, HelpText = "Command line arguments for the process to be created. (Wrap in quotes)")]
        public string args { get; set; }

        [OptionList('b', "bounds", Separator = ' ', HelpText = "Specify the bounds of the window. Ordered as x, y, width, height, and is measured in pixels. (Wrap in quotes)")]
        public IList<string> bounds { get; set; }
    }

    class ResetOptions
    {
        [Option('k', "kill", Required = false, HelpText = "Kill the processes after removing them from the wallpaper")]
        public bool killProcess { get; set; }
    }

    class Options
    {
        public const string VERB_SET = "set";
        public const string VERB_START = "start";
        public const string VERB_RESET = "reset";

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return CommandLine.Text.HelpText.AutoBuild(this, verb);
        }

        [VerbOption(VERB_RESET, HelpText = "Reset the desktop background to its default state")]
        public ResetOptions ResetVerb { get; set; }

        [VerbOption(VERB_SET, HelpText = "Set the desktop wallpaper from an existing window.")]
        public SetOptions SetVerb { get; set; }

        [VerbOption(VERB_START, HelpText = "Start a new process and set it as the desktop background")]
        public StartOptions StartVerb { get; set; }

    }
}
