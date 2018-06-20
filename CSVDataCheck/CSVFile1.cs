using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDataCheck
{
    public class CSVFile1
    {
        /// <summary>
        /// [ZH] 設備コード。
        /// </summary>
        public string EquipmentNum { get; set; }

        /// <summary>
        /// [ZH] 工程コード。
        /// </summary>
        public string OperNum { get; set; }

        /// <summary>
        /// [ZH] 作業指示書No。
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// [ZH] 预定生产日期。
        /// </summary>
        public string DatePlanProduct { get; set; }

        /// <summary>
        /// [ZH] 構成品番。
        /// </summary>
        public string CmpPrt { get; set; }

        /// <summary>
        /// [ZH] 構成品シリアル番号（単品）。
        /// </summary>
        public string CmpQr { get; set; }

        /// <summary>
        /// [ZH] 構成品シリアル番号（箱）。
        /// </summary>
        public string CmpLot { get; set; }

        /// <summary>
        /// [ZH] 投入数量。
        /// </summary>
        public string QtyUsed { get; set; }

        /// <summary>
        /// [ZH] 投入日時。
        /// </summary>
        public string DateUsed { get; set; }

        /// <summary>
        /// [ZH] 完成品仓库。
        /// </summary>
        public string Whse { get; set; }

        /// <summary>
        /// [ZH] 完成品番。
        /// </summary>
        public string ParPrt { get; set; }

        /// <summary>
        /// [ZH] 完成品シリアル番号（単品）。
        /// </summary>
        public string ParQr { get; set; }

        /// <summary>
        /// [ZH] 完成品シリアル番号（箱）。
        /// </summary>
        public string ParLot { get; set; }

        /// <summary>
        /// [ZH] 実績数量。
        /// </summary>
        public string QtyCompleted { get; set; }

        /// <summary>
        /// [ZH] 完成日時。
        /// </summary>
        public string DateCompleted { get; set; }

    }
}
