using System;

using SmartQuant;
using System.IO;
using System.Reflection;

using CommandLine;

using System.Threading;

#if NET48
using ClipboardMonitor;
using System.Windows.Forms;
#endif

namespace QuantBox.APIProvider
{
    /// <summary>
    /// Provder宿主
    /// 由它进行其它Provder的初始创建，以及订单的路由
    /// </summary>
    public partial class ProviderHost
    {
        private CmdLine cmdLine = null;

        private void ClipboardNotifications_ClipboardUpdate(object sender, EventArgs e)
        {
            cmdLine.ParseForStop(this);
        }

        private object GetSolutionManager()
        {
            // OpenQuant.Global.SolutionManager是静态属性，可以通过Get方式获得
            var g = Assembly.GetEntryAssembly().GetType("OpenQuant.Global");
            var sm = g.GetProperty("SolutionManager");
            return sm.GetGetMethod().Invoke(null, null);
        }

        private void LoadSolution(object solutionManager, string filename)
        {
            var type = solutionManager.GetType();
            var m = type.GetMethod("LoadSolution", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(solutionManager, new object[] { new FileInfo(filename) });
        }

        public void Solution_Start_Thread(Options opts)
        {
#if NET48
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                DateTime dt = DateTime.Now;
                // 检查界面是否正常启动
                var mainForm = GetMainForm();
                while (mainForm == null)
                {
                    Thread.Sleep(1000);
                    mainForm = GetMainForm();

                    // 如果1分钟找不到就退出循环
                    var ts = DateTime.Now - dt;
                    if (ts.TotalSeconds > 60)
                    {
                        return;
                    }
                }

                var sm = GetSolutionManager();
                Thread.Sleep(1000);
                mainForm.SafeInvoke(() =>
                {
                    LoadSolution(sm, opts.file);
                });
                if (opts.run)
                {
                    Thread.Sleep(3000);
                    mainForm.SafeInvoke(() =>
                    {
                        Solution_Start(mainForm);
                    });
                }
            });
#endif
        }

        public void Solution_Stop_Thread(Options opts)
        {
#if NET48
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                var mainForm = GetMainForm();
                if (mainForm == null)
                {
                    return;
                }
                var sm = GetSolutionManager();
                if (opts.stop)
                {
                    // 没有停止的需要停止才能退出
                    if (framework.StrategyManager.Status != StrategyStatus.Stopped)
                    {
                        Thread.Sleep(1000);
                        mainForm.SafeInvoke(() =>
                        {
                            Solution_Stop(mainForm);
                        });
                    }
                }
                if (opts.exit)
                {
                    Thread.Sleep(3000);
                    mainForm.SafeInvoke(() =>
                    {
                        File_Exit(mainForm);
                    });
                }
            });
#endif
        }


#if NET48

        private Form GetMainForm()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "MainForm")
                    return f;
            }
            return null;
        }
        private void Solution_Start(Form from)
        {
            Type type = from.GetType();
            var m = type.GetMethod("menuSolution_Start_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(from, new object[] { null, null });
        }

        private void Solution_Stop(Form from)
        {
            Type type = from.GetType();
            var m = type.GetMethod("menuSolution_Stop_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(from, new object[] { null, null });
        }

        private void File_Exit(Form from)
        {
            Type type = from.GetType();
            var m = type.GetMethod("menuFile_Exit_Click", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(from, new object[] { null, null });
        }
#endif
    }
}
