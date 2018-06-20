using CSVDataCheck.Models;
using CSVDataCheck.Models.Command;
using CSVDataCheck.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSVDataCheck.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public TextBox TextBoxCmd;
        public Button queryBtn, checkBtn;

        List<string> OtherPaths = new List<string>();

        private delegate void AddStringToListBoxDelegate(string str);
        private delegate void SetBtnDelegate(bool status);

        /// <summary>
        /// MahApps.Metro 风格dialog
        /// </summary>
        private readonly IDialogCoordinator _dialogCoordinator;

        private static readonly string 不成对文件 = @"不成对文件\";
        private static readonly string CSV行数不一致 = @"CSV行数不一致\";
        private static readonly string 设备缺失或不一致 = @"设备缺失或不一致\";
        private static readonly string 工程缺失或不一致 = @"工程缺失或不一致\";
        private static readonly string 构成品品番缺失或不一致 = @"构成品品番缺失或不一致\";
        private static readonly string 构成品箱缺失或不一致 = @"构成品箱缺失或不一致\";
        private static readonly string 构成品单品不一致 = @"构成品单品不一致\";
        private static readonly string 投入数量 = @"投入数量\";
        private static readonly string 实绩数量 = @"实绩数量\";
        private static readonly string 完成品品番 = @"完成品品番\";
        private static readonly string 完成品箱 = @"完成品箱\";
        private static readonly string 完成品仓库 = @"完成品仓库\";
        private static readonly string 完成日时 = @"完成日时\";

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            OtherPaths.Add("BACKUP");
            OtherPaths.Add("ERROR_BACKUP");
            OtherPaths.Add("ERROR_LOGS");

            ProgressVisibility = Visibility.Hidden;
        }


        private async void ShowProgress()
        {
            var lockDialog = new CustomDialog();
            var dataContext = new ProgressContent(x =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, lockDialog);
            });
            lockDialog.Content = new ProgressControl { DataContext = dataContext };
            await _dialogCoordinator.ShowMetroDialogAsync(this, lockDialog);
        }

        #region 属性

        #region progress
        private Visibility progressVisibility;
        public Visibility ProgressVisibility
        {
            get { return progressVisibility; }
            set
            {
                progressVisibility = value;
                RaisePropertyChanged("ProgressVisibility");
            }
        }
        #endregion

        #region FolderPath
        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                RaisePropertyChanged("FolderPath");
            }
        }
        #endregion

        #region BadgeCount
        private string error1 = "0";
        public string Error1
        {
            get { return error1; }
            set
            {
                error1 = value;
                RaisePropertyChanged("Error1");
            }
        }

        private string error2 = "0";
        public string Error2
        {
            get { return error2; }
            set
            {
                error2 = value;
                RaisePropertyChanged("Error2");
            }
        }

        private string error3 = "0";
        public string Error3
        {
            get { return error3; }
            set
            {
                error3 = value;
                RaisePropertyChanged("Error3");
            }
        }

        private string error4 = "0";
        public string Error4
        {
            get { return error4; }
            set
            {
                error4 = value;
                RaisePropertyChanged("Error4");
            }
        }

        private string error5 = "0";
        public string Error5
        {
            get { return error5; }
            set
            {
                error5 = value;
                RaisePropertyChanged("Error5");
            }
        }

        private string error6 = "0";
        public string Error6
        {
            get { return error6; }
            set
            {
                error6 = value;
                RaisePropertyChanged("Error6");
            }
        }

        private string error7 = "0";
        public string Error7
        {
            get { return error7; }
            set
            {
                error7 = value;
                RaisePropertyChanged("Error7");
            }
        }

        private string error8 = "0";
        public string Error8
        {
            get { return error8; }
            set
            {
                error8 = value;
                RaisePropertyChanged("Error8");
            }
        }

        private string error9 = "0";
        public string Error9
        {
            get { return error9; }
            set
            {
                error9 = value;
                RaisePropertyChanged("Error9");
            }
        }

        private string error10 = "0";
        public string Error10
        {
            get { return error10; }
            set
            {
                error10 = value;
                RaisePropertyChanged("Error10");
            }
        }

        private string error11 = "0";
        public string Error11
        {
            get { return error11; }
            set
            {
                error11 = value;
                RaisePropertyChanged("Error11");
            }
        }

        private string error12 = "0";
        public string Error12
        {
            get { return error12; }
            set
            {
                error12 = value;
                RaisePropertyChanged("Error12");
            }
        }

        private string error13 = "0";
        public string Error13
        {
            get { return error13; }
            set
            {
                error13 = value;
                RaisePropertyChanged("Error13");
            }
        }

        #endregion

        #region 错误叠加
        
        private bool errorStatus = false;
        public bool ErrorStatus
        {
            get { return errorStatus; }
            set
            {
                errorStatus = value;
                RaisePropertyChanged("ErrorStatus");
            }
        }
        #endregion

        #endregion

        #region 命令

        #region 加载完毕事件
        private ICommand windowsLoaded;
        public ICommand WindowsLoaded { get { return windowsLoaded ?? (windowsLoaded = new RelayCommand(p => Loaded((Main)p))); } }
        #endregion

        #region 检索路径
        private ICommand startQuery;
        public ICommand StartQuery { get { return startQuery ?? (startQuery = new RelayCommand(p => QueryPath())); } }
        #endregion

        #region 开始检测
        private ICommand startCheck;

        public ICommand StartCheck { get { return startCheck ?? (startCheck = new RelayCommand(p => Check())); } }
        #endregion

        #region ErrorClick
        private ICommand click1;

        public ICommand Click1 { get { return click1 ?? (click1 = new RelayCommand(p => Click1Path())); } }

        private ICommand click2;

        public ICommand Click2 { get { return click2 ?? (click2 = new RelayCommand(p => Click2Path())); } }

        private ICommand click3;

        public ICommand Click3 { get { return click3 ?? (click3 = new RelayCommand(p => Click3Path())); } }

        private ICommand click4;

        public ICommand Click4 { get { return click4 ?? (click4 = new RelayCommand(p => Click4Path())); } }

        private ICommand click5;

        public ICommand Click5 { get { return click5 ?? (click5 = new RelayCommand(p => Click5Path())); } }

        private ICommand click6;

        public ICommand Click6 { get { return click6 ?? (click6 = new RelayCommand(p => Click6Path())); } }

        private ICommand click7;

        public ICommand Click7 { get { return click7 ?? (click7 = new RelayCommand(p => Click7Path())); } }

        private ICommand click8;

        public ICommand Click8 { get { return click8 ?? (click8 = new RelayCommand(p => Click8Path())); } }

        private ICommand click9;

        public ICommand Click9 { get { return click9 ?? (click9 = new RelayCommand(p => Click9Path())); } }

        private ICommand click10;

        public ICommand Click10 { get { return click10 ?? (click10 = new RelayCommand(p => Click10Path())); } }

        private ICommand click11;

        public ICommand Click11 { get { return click11 ?? (click11 = new RelayCommand(p => Click11Path())); } }

        private ICommand click12;

        public ICommand Click12 { get { return click12 ?? (click12 = new RelayCommand(p => Click12Path())); } }

        private ICommand click13;

        public ICommand Click13 { get { return click13 ?? (click13 = new RelayCommand(p => Click13Path())); } }

        #endregion

        #endregion

        #region ClickMethod
        private void Click1Path()
        {
            if (error1 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 不成对文件);
        }

        private void Click2Path()
        {
            if (error2 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + CSV行数不一致);
        }

        private void Click3Path()
        {
            if (error3 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 设备缺失或不一致);
        }

        private void Click4Path()
        {
            if (error4 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 工程缺失或不一致);
        }

        private void Click5Path()
        {
            if (error5 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 构成品品番缺失或不一致);
        }

        private void Click6Path()
        {
            if (error6 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 构成品箱缺失或不一致);
        }

        private void Click7Path()
        {
            if (error7 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 构成品单品不一致);
        }

        private void Click8Path()
        {
            if (error8 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 投入数量);
        }

        private void Click9Path()
        {
            if (error9 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 实绩数量);
        }

        private void Click10Path()
        {
            if (error10 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 完成品品番);
        }

        private void Click11Path()
        {
            if (error11 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 完成品箱);
        }

        private void Click12Path()
        {
            if (error12 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 完成品仓库);
        }

        private void Click13Path()
        {
            if (error13 == "0") return;
            System.Diagnostics.Process.Start("explorer.exe", @"D:\error\" + 完成日时); 
        }
        #endregion 


        #region Loaded
        private void Loaded(Main mv)
        {
            //TextBoxCmd = mvtextBox;// textBox;
            TextBoxCmd = mv.TextCmd;
            queryBtn = mv.QueryBtn;
            checkBtn = mv.CheckBtn;
        }
        #endregion

        protected async Task<int> TryTaskRun(Func<int> action)
        {
            return await Task.Run(() =>
            {
                ProgressVisibility = Visibility.Visible;
                SetBtn(false);
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ProgressVisibility = Visibility.Hidden;
                    SetBtn(true);
                }
            });
        }

        private void QueryPath()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = @"D:\Work\DENSO\客户提供资料\20180531_DMTT-PLCテストデータ\PLC-Data-BACKUP_20180531";
            dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.SelectedPath))
            {
                FolderPath = dialog.SelectedPath;
            }
        }

        private async void Check()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "提示", "请先填入检索路径!");
                return;
            }
            await TryTaskRun(() =>
            {
                Error1 = Error2 = Error3 = Error4 = Error5 = Error6 = Error7 = Error8 = Error9 = Error10 = Error11 = Error12 = Error13 = "0";
                int errCnt = 0;
                UICmd("开始检测请等待...");
                DirectoryInfo TheFolder = new DirectoryInfo(FolderPath);
                //遍历文件夹
                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories().Where(x => !OtherPaths.Contains(x.Name)))
                {
                    UICmd(string.Format("开始遍历文件夹【{0}】...", NextFolder.Name));
                    // 排序
                    List<FileInfo> file1Infos = NextFolder.GetFiles().Where(x => x.Name.Contains("File1")).OrderBy(x => x.Name).ToList();
                    List<FileInfo> file2Infos = NextFolder.GetFiles().Where(x => x.Name.Contains("File2")).OrderBy(x => x.Name).ToList();

                    List<FileInfo> file1MissList = new List<FileInfo>();
                    List<FileInfo> file2MissList = new List<FileInfo>();

                    // [xxx]File1对应的File2是否缺少
                    foreach (FileInfo f1 in file1Infos)
                    {
                        if (file2Infos.Count(x => x.Name.Replace("File2", "").Contains(f1.Name.Replace("File1", ""))) == 0)
                        {
                            file1MissList.Add(f1);
                            CopyFileTo(f1, 不成对文件 + NextFolder.Name);
                            errCnt++;
                            Error1 = (1 + Int32.Parse(Error1)).ToString();
                            //UICmd(string.Format("出现不成对文件【{0}】...", f1.Name));
                        }
                    }

                    // [xxx]File2对应的File1是否缺少
                    foreach (FileInfo f2 in file2Infos)
                    {
                        if (file1Infos.Count(x => x.Name.Replace("File1", "").Contains(f2.Name.Replace("File2", ""))) == 0)
                        {
                            file2MissList.Add(f2);
                            CopyFileTo(f2, 不成对文件 + NextFolder.Name);
                            errCnt++;
                            Error1 = (1 + Int32.Parse(Error1)).ToString();
                            //UICmd(string.Format("出现不成对文件【{0}】...", f2.Name));
                        }
                    }

                    // 对于不是成对出现的csv不进行读取
                    foreach (FileInfo f in file1MissList)
                    {
                        file1Infos.Remove(f);
                    }
                    foreach (FileInfo f in file2MissList)
                    {
                        file2Infos.Remove(f);
                    }

                    // 读取csv文件
                    for (int i = 0; i < file1Infos.Count; i++)
                    {
                        // 读取 file1、file2
                        List<string[]> csvFile1 = ReadCSV(file1Infos[i].FullName);
                        List<string[]> csvFile2 = ReadCSV(file2Infos[i].FullName);

                        List<CSVFile1> listFile1 = new List<CSVFile1>();
                        List<CSVFile2> listFile2 = new List<CSVFile2>();
                        // 行数验证
                        if (csvFile1.Count != csvFile2.Count)
                        {
                            // "file1[file1Infos[i].Name]与对应的file2[file2Infos[i].Name]的行数不一致"
                            errCnt++;
                            CopyFileTo(file1Infos[i], CSV行数不一致 + NextFolder.Name);
                            CopyFileTo(file2Infos[i], CSV行数不一致 + NextFolder.Name);
                            Error2 = (1 + Int32.Parse(Error2)).ToString();
                            //UICmd(string.Format("出现行数不一致的csv文件【{0}】与【{1}】...", file1Infos[i].Name, file2Infos[i].Name));
                            continue;
                        }
                        foreach (var rowData in csvFile1)
                        {
                            //UICmd(string.Format("开始遍历【{0}】csv文件行数据...", file1Infos[i].Name));

                            CSVFile1 cSVFile1 = new CSVFile1
                            {
                                // 設備コード
                                EquipmentNum = rowData[0].Trim(),
                                // 工程コード
                                OperNum = rowData[1].Trim(),
                                // 作業指示書No
                                Job = rowData[2].Trim(),
                                // 構成品番
                                CmpPrt = rowData[3].Trim(),
                                // 構成品シリアル番号（単品）
                                CmpQr = rowData[4].Trim(),
                                // 構成品シリアル番号（箱）
                                CmpLot = rowData[5].Trim(),
                                // 投入数量
                                QtyUsed = rowData[6].Trim(),
                                // 投入日時
                                DateUsed = rowData[7].Trim(),
                                // 完成品仓库
                                Whse = rowData[8].Trim(),
                                // 完成品番
                                ParPrt = rowData[9].Trim(),
                                // 完成品シリアル番号（単品）
                                ParQr = rowData[10].Trim(),
                                // 完成品シリアル番号（箱）
                                ParLot = rowData[11].Trim(),
                                // 実績数量
                                QtyCompleted = rowData[12].Trim(),
                                // 完成日時
                                DateCompleted = rowData[13].Trim(),
                                // 预定生产日期
                                DatePlanProduct = rowData[14].Trim()
                            };
                            listFile1.Add(cSVFile1);
                        }

                        foreach (var rowData in csvFile2)
                        {
                            //UICmd(string.Format("开始遍历【{0}】csv文件行数据...", file2Infos[i].Name));
                            CSVFile2 cSVFile2 = new CSVFile2
                            {
                                // 設備コード 
                                EquipmentNum = rowData[0].Trim(),
                                // 工程コード 
                                OperNum = rowData[1].Trim(),
                                // 構成品番
                                CmpPrt = rowData[2].Trim(),
                                // 構成品シリアル番号（単品）
                                CmpQr = rowData[3].Trim(),
                                // 構成品シリアル番号（箱）
                                CmpLot = rowData[4].Trim()
                            };
                            listFile2.Add(cSVFile2);
                        }

                        for (int j = 0; j < listFile1.Count; j++)
                        {
                            // 設備コード 验证 => File1.設備コード = File2.設備コード != empty
                            if (string.IsNullOrEmpty(listFile1[j].EquipmentNum) || listFile1[j].EquipmentNum != listFile2[j].EquipmentNum)
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 设备缺失或不一致 + NextFolder.Name);
                                CopyFileTo(file2Infos[i], 设备缺失或不一致 + NextFolder.Name);
                                Error3 = (1 + Int32.Parse(Error3)).ToString();
                                //UICmd(string.Format("出现设备缺失或不一致的csv文件【{0}】与【{1}】...", file1Infos[i].Name, file2Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 工程コード 验证 => File1.工程コード = File2.工程コード != empty
                            if (string.IsNullOrEmpty(listFile1[j].OperNum) || listFile1[j].OperNum != listFile2[j].OperNum)
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 工程缺失或不一致 + NextFolder.Name);
                                CopyFileTo(file2Infos[i], 工程缺失或不一致 + NextFolder.Name);
                                Error4 = (1 + Int32.Parse(Error4)).ToString();
                                //UICmd(string.Format("出现工程缺失或不一致的csv文件【{0}】与【{1}】...", file1Infos[i].Name, file2Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 構成品番 验证 => File.構成品番 = File2.構成品番 != empty
                            if (string.IsNullOrEmpty(listFile1[j].CmpPrt) || listFile1[j].CmpPrt != listFile2[j].CmpPrt)
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 构成品品番缺失或不一致 + NextFolder.Name);
                                CopyFileTo(file2Infos[i], 构成品品番缺失或不一致 + NextFolder.Name);
                                Error5 = (1 + Int32.Parse(Error5)).ToString();
                                //UICmd(string.Format("出现构成品品番缺失或不一致的csv文件【{0}】与【{1}】...", file1Infos[i].Name, file2Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 構成品シリアル番号（箱） 验证 => File.構成品シリアル番号（箱） = File2.構成品シリアル番号（箱） != empty
                            if (string.IsNullOrEmpty(listFile1[j].CmpLot) || listFile1[j].CmpLot != listFile2[j].CmpLot)
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 构成品箱缺失或不一致 + NextFolder.Name);
                                CopyFileTo(file2Infos[i], 构成品箱缺失或不一致 + NextFolder.Name);
                                Error6 = (1 + Int32.Parse(Error6)).ToString();
                                //UICmd(string.Format("出现构成品箱缺失或不一致的csv文件【{0}】与【{1}】...", file1Infos[i].Name, file2Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 構成品シリアル番号（単品） 验证 => File.構成品シリアル番号（単品） = File2.構成品シリアル番号（単品）
                            if (string.IsNullOrEmpty(listFile1[j].CmpQr) && listFile1[j].CmpQr != listFile2[j].CmpQr)
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 构成品单品不一致 + NextFolder.Name);
                                CopyFileTo(file2Infos[i], 构成品单品不一致 + NextFolder.Name);
                                Error7 = (1 + Int32.Parse(Error7)).ToString();
                                //UICmd(string.Format("出现构成品单品不一致的csv文件【{0}】与【{1}】...", file1Infos[i].Name, file2Infos[i].Name));
                                if (!errorStatus) continue;
                            }


                            // File1 单独的
                            // 投入数量
                            if (string.IsNullOrEmpty(listFile1[j].QtyUsed))
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 投入数量 + NextFolder.Name);
                                Error8 = (1 + Int32.Parse(Error8)).ToString();
                                //UICmd(string.Format("出现投入数量缺失的csv文件【{0}】...", file1Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 実績数量
                            if (string.IsNullOrEmpty(listFile1[j].QtyCompleted))
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 实绩数量 + NextFolder.Name);
                                Error9 = (1 + Int32.Parse(Error9)).ToString();
                                //UICmd(string.Format("出现实绩数量缺失的csv文件【{0}】...", file1Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 完成品仓库
                            if (string.IsNullOrEmpty(listFile1[j].Whse))
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 完成品仓库 + NextFolder.Name);
                                Error10 = (1 + Int32.Parse(Error10)).ToString();
                                //UICmd(string.Format("出现完成品仓库缺失的csv文件【{0}】...", file1Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 完成品番
                            if (string.IsNullOrEmpty(listFile1[j].CmpPrt))
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 完成品品番 + NextFolder.Name);
                                Error11 = (1 + Int32.Parse(Error11)).ToString();
                                //UICmd(string.Format("出现完成品品番缺失的csv文件【{0}】...", file1Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 完成品シリアル番号（箱）
                            if (string.IsNullOrEmpty(listFile1[j].CmpLot))
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 完成品箱 + NextFolder.Name);
                                Error12 = (1 + Int32.Parse(Error12)).ToString();
                                //UICmd(string.Format("出现完成品箱缺失的csv文件【{0}】...", file1Infos[i].Name));
                                if (!errorStatus) continue;
                            }

                            // 完成日時
                            if (string.IsNullOrEmpty(listFile1[j].DateCompleted))
                            {
                                errCnt++;
                                CopyFileTo(file1Infos[i], 完成日时 + NextFolder.Name);
                                Error13 = (1 + Int32.Parse(Error13)).ToString();
                                //UICmd(string.Format("出现完成日时缺失的csv文件【{0}】...", file1Infos[i].Name));
                                if (!errorStatus) continue;
                            }
                        }

                    }

                }
                return 1;
            });
        }

        public static List<string[]> ReadCSV(string filePathName)
        {
            List<string[]> ls = new List<string[]>();
            StreamReader fileReader = new StreamReader(filePathName);
            string strLine = "";
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Split(','));
                }
            }
            fileReader.Close();
            return ls;
        }

        public void CopyFileTo(FileInfo fileInfo, string pathName)
        {
            if (!Directory.Exists(@"D:\error\" + pathName))
            {
                Directory.CreateDirectory(@"D:\error\" + pathName);
            }
            fileInfo.CopyTo(@"D:\error\" + pathName + @"\" + fileInfo.Name, true);  // 多行出错时
        }

        /// <summary>
        /// 控制台UI【效率低】
        /// </summary>
        /// <param name="s"></param>
        private void UICmd(string s)
        {
            if (!TextBoxCmd.Dispatcher.CheckAccess())
            {
                AddStringToListBoxDelegate d = UICmd;
                TextBoxCmd.Dispatcher.Invoke(d, s);
            }
            else
            {
                TextBoxCmd.Text += s;
                TextBoxCmd.Text += Environment.NewLine;
                TextBoxCmd.Select(TextBoxCmd.Text.Length, 0);
            }
        }

        private void SetBtn(bool status)
        {
            if (!queryBtn.Dispatcher.CheckAccess())
            {
                SetBtnDelegate d = SetBtn;
                queryBtn.Dispatcher.Invoke(d, status);
            }
            else
            {
                queryBtn.IsEnabled = status;
            }

            if (!checkBtn.Dispatcher.CheckAccess())
            {
                SetBtnDelegate d = SetBtn;
                checkBtn.Dispatcher.Invoke(d, status);
            }
            else
            {
                checkBtn.IsEnabled = status;
            }
        }
    }
}
