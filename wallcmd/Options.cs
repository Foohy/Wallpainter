using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace wallcmd
{

    class SetOptions
    {
        [Option('n', "name", HelpText = "Window name to set as the desktop background")]
        public string windowName { get; set; }

        [Option('p', "pid", HelpText = "Process ID holding the hwnd handle to set")]
        public string pid { get; set; }
    }

    class StartOptions
    {
        [Option('p', "path", Required = true, HelpText = "Path to the executable to start")]
        public string path { get; set; }

        [Option('a', "args", Required = false, HelpText = "Command line arguments for the process to be created")]
        public List<string> args { get; set; }
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
