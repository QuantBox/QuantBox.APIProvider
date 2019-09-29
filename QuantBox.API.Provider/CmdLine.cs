using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuantBox.APIProvider
{
    public class Options
    {
        [Option('f', "file", Required = false, HelpText = "策略项目文件")]
        public string file { get; set; }

        [Option('i', "id", Required = false, HelpText = "实例数字ID，用于收到剪贴板事件时过滤使用")]
        public int id { get; set; }

        [Option('r', "run", Required = false, Default = false, HelpText = "运行策略")]
        public bool run { get; set; }

        [Option('s', "stop", Required = false, Default = false, HelpText = "停止策略")]
        public bool stop { get; set; }

        [Option('e', "exit", Required = false, Default = false, HelpText = "退出程序")]
        public bool exit { get; set; }
    }

    public class CmdLine
    {
        private int id;

        public void ParseForStart(ProviderHost host)
        {
            // 通过命令行启动策略
            //cd "C:\Program Files\SmartQuant Ltd\OpenQuant 2014"
            //C:
            //start OpenQuant.exe --file="D:\Users\Kan\Documents\OpenQuant 2014\Solutions\SMACrossover\SMACrossover.sln" --id=100 --run

            var args = System.Environment.GetCommandLineArgs();
            var text = System.Environment.CommandLine;
            Console.WriteLine($"命令行: {text}");
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptions(opts, host))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        public void ParseForStop(ProviderHost host)
        {
            //echo --id=100 --stop --exit | clip
            IDataObject ido = Clipboard.GetDataObject();

            if (!ido.GetDataPresent(DataFormats.Text))
                return;

            var text = ido.GetData(DataFormats.Text) as string;
            Console.WriteLine($"剪贴板: {text}");
            CommandLine.Parser.Default.ParseArguments<Options>(text.Split(' '))
                .WithParsed<Options>(opts => ExitOptions(opts, host))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        void RunOptions(Options opts, ProviderHost host)
        {
            // 记下ID，退出时使用
            id = opts.id;

            if (string.IsNullOrEmpty(opts.file))
                return;

            if (!(new FileInfo(opts.file).Exists))
                return;

            host.Solution_Start_Thread(opts);
        }

        void ExitOptions(Options opts, ProviderHost host)
        {
            if (id != opts.id)
                return;

            host.Solution_Stop_Thread(opts);
        }

        void HandleParseError(IEnumerable<Error> errs)
        {
            // Application.Exit();
        }
    }
}
