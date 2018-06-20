using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSVDataCheck
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> OtherPaths = new List<string>();

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

        public MainWindow()
        {
            InitializeComponent();
            OtherPaths.Add("BACKUP");
            OtherPaths.Add("ERROR_BACKUP");
            OtherPaths.Add("ERROR_LOGS");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.ShowDialog();
            // (@"C:\EDI");//
            int errCnt = 0;
            DirectoryInfo TheFolder = new DirectoryInfo(@"D:\Work\DENSO\客户提供资料\20180531_DMTT-PLCテストデータ\PLC-Data-BACKUP_20180531");
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories().Where(x => !OtherPaths.Contains(x.Name)))
            {
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
                        CopyFileTo(f1, 不成对文件+ NextFolder.Name);
                        errCnt++;
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
                        continue;
                    }
                    foreach (var rowData in csvFile1)
                    {
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
                            continue;
                        }

                        // 工程コード 验证 => File1.工程コード = File2.工程コード != empty
                        if (string.IsNullOrEmpty(listFile1[j].OperNum) || listFile1[j].OperNum != listFile2[j].OperNum)
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 工程缺失或不一致 + NextFolder.Name);
                            CopyFileTo(file2Infos[i], 工程缺失或不一致 + NextFolder.Name);
                            continue;
                        }

                        // 構成品番 验证 => File.構成品番 = File2.構成品番 != empty
                        if (string.IsNullOrEmpty(listFile1[j].CmpPrt) || listFile1[j].CmpPrt != listFile2[j].CmpPrt)
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 构成品品番缺失或不一致 + NextFolder.Name);
                            CopyFileTo(file2Infos[i], 构成品品番缺失或不一致 + NextFolder.Name);
                            continue;
                        }

                        // 構成品シリアル番号（箱） 验证 => File.構成品シリアル番号（箱） = File2.構成品シリアル番号（箱） != empty
                        if (string.IsNullOrEmpty(listFile1[j].CmpLot) || listFile1[j].CmpLot != listFile2[j].CmpLot)
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 构成品箱缺失或不一致 + NextFolder.Name);
                            CopyFileTo(file2Infos[i], 构成品箱缺失或不一致 + NextFolder.Name);
                            continue;
                        }

                        // 構成品シリアル番号（単品） 验证 => File.構成品シリアル番号（単品） = File2.構成品シリアル番号（単品）
                        if (string.IsNullOrEmpty(listFile1[j].CmpQr) && listFile1[j].CmpQr != listFile2[j].CmpQr)
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 构成品单品不一致 + NextFolder.Name);
                            CopyFileTo(file2Infos[i], 构成品单品不一致 + NextFolder.Name);
                            continue;
                        }


                        // File1 单独的
                        // 投入数量
                        if (string.IsNullOrEmpty(listFile1[j].QtyUsed))
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 投入数量 + NextFolder.Name);
                            continue;
                        }

                        // 実績数量
                        if (string.IsNullOrEmpty(listFile1[j].QtyCompleted))
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 实绩数量 + NextFolder.Name);
                            continue;
                        }

                        // 完成品仓库
                        if (string.IsNullOrEmpty(listFile1[j].Whse))
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 完成品仓库 + NextFolder.Name);
                            continue;
                        }

                        // 完成品番
                        if (string.IsNullOrEmpty(listFile1[j].CmpPrt))
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 完成品品番 + NextFolder.Name);
                            continue;
                        }

                        // 完成品シリアル番号（箱）
                        if (string.IsNullOrEmpty(listFile1[j].CmpLot))
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 完成品箱 + NextFolder.Name);
                            continue;
                        }

                        // 完成日時
                        if (string.IsNullOrEmpty(listFile1[j].DateCompleted))
                        {
                            errCnt++;
                            CopyFileTo(file1Infos[i], 完成日时 + NextFolder.Name);
                            continue;
                        }
                    }

                }

            }

            Button.Content = errCnt.ToString();
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

        public static void CopyFileTo(FileInfo fileInfo, string pathName)
        {
            if (!Directory.Exists(@"D:\error\" + pathName))
            {
                Directory.CreateDirectory(@"D:\error\" + pathName);
            }
            fileInfo.CopyTo(@"D:\error\"+ pathName + @"\" + fileInfo.Name, true);  // 多行出错时
        }

    }
}
