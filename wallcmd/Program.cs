using System;
using System.Collections.Generic;
using System.Text;
using Wallpainter;

namespace wallcmd
{
    class Program
    {
        static int Main(string[] args)
        {
            string invokedVerb = "";
            object invokedVerbOptions = null;
            if (!CommandLine.Parser.Default.ParseArguments(args, new Options(),
                (verb, verbOptions) =>
                {
                    invokedVerb = verb;
                    invokedVerbOptions = verbOptions;
                }))
            {
                return CommandLine.Parser.DefaultExitCodeFail;
            }

            switch (invokedVerb)
            {
                case Options.VERB_SET:
                    Set((SetOptions)invokedVerbOptions);
                    break;

                case Options.VERB_START:
                    Start((StartOptions)invokedVerbOptions);
                    break;

                case Options.VERB_RESET:
                    Reset((ResetOptions)invokedVerbOptions);
                    break;

                default:
                    return 1;
            }

            return 0;
        }

        public static void Set(SetOptions op)
        {
            throw new NotImplementedException();
        }

        public static void Start(StartOptions op)
        {
            throw new NotImplementedException();
        }

        public static void Reset(ResetOptions op)
        {
            WindowUtils.ClearWallpaper(op.killProcess);
        }
    }
}
